using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq.InternalAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class OptionOverloadAnalyzer : DiagnosticAnalyzer
{
    private const string Category = "Design";

    public const string DiagnosticIdMissingNonDefault = "HLQInternal005";
    private static readonly LocalizableString TitleMissingNonDefault = "Missing non-default overload";
    private static readonly LocalizableString MessageFormatMissingNonDefault = "The method '{0}' must have a counterpart '{1}'";
    private static readonly DiagnosticDescriptor RuleMissingNonDefault = new DiagnosticDescriptor(
        DiagnosticIdMissingNonDefault,
        TitleMissingNonDefault,
        MessageFormatMissingNonDefault,
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public const string DiagnosticIdMissingOption = "HLQInternal006";
    private static readonly LocalizableString TitleMissingOption = "Missing Option overload";
    private static readonly LocalizableString MessageFormatMissingOption = "The method '{0}' must have a counterpart '{1}' that returns Option<T>. Reason: {2}";
    private static readonly DiagnosticDescriptor RuleMissingOption = new DiagnosticDescriptor(
        DiagnosticIdMissingOption,
        TitleMissingOption,
        MessageFormatMissingOption,
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public const string DiagnosticIdDelegation = "HLQInternal007";
    private static readonly LocalizableString TitleDelegation = "Method must delegate to Option overload";
    private static readonly LocalizableString MessageFormatDelegation = "The method '{0}' must delegate execution to '{1}'";
    private static readonly DiagnosticDescriptor RuleDelegation = new DiagnosticDescriptor(
        DiagnosticIdDelegation,
        TitleDelegation,
        MessageFormatDelegation,
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public const string DiagnosticIdMissingMinMaxGroup = "HLQInternal008";
    private static readonly LocalizableString TitleMissingMinMaxGroup = "Missing Min/Max/MinMax group methods";
    private static readonly LocalizableString MessageFormatMissingMinMaxGroup = "The method '{0}' is part of a Min/Max/MinMax group. Missing counterparts: {1}";
    private static readonly DiagnosticDescriptor RuleMissingMinMaxGroup = new DiagnosticDescriptor(
        DiagnosticIdMissingMinMaxGroup,
        TitleMissingMinMaxGroup,
        MessageFormatMissingMinMaxGroup,
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
        ImmutableArray.Create(RuleMissingNonDefault, RuleMissingOption, RuleDelegation, RuleMissingMinMaxGroup);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(AnalyzeMethod, SymbolKind.Method);
    }

    private void AnalyzeMethod(SymbolAnalysisContext context)
    {
        var methodSymbol = (IMethodSymbol)context.Symbol;
        
        // 1. Check if it's an extension method OR inside an extension type (C# 14)
        if (!methodSymbol.IsExtensionMethod && !IsInExtensionType(context, methodSymbol))
            return;

        var methodName = methodSymbol.Name;

        if (methodName.EndsWith("OrDefault") && methodName.Length > 9)
        {
            var baseName = methodName.Substring(0, methodName.Length - 9); // Remove "OrDefault"
            var orNoneName = baseName + "OrNone";

            var parameters = methodSymbol.Parameters;
            var hasDefaultValueParam = parameters.Length > 0 && parameters[parameters.Length - 1].Name == "defaultValue";

            var containingType = methodSymbol.ContainingType;
            var members = containingType.GetMembers();

            // Check for * (no default) overload
            // If defaultValue is present, we don't expect a non-default counterpart (e.g. First(defaultValue) doesn't make sense)
            if (!hasDefaultValueParam)
            {
                var nonDefaultMethod = members
                    .OfType<IMethodSymbol>()
                    .FirstOrDefault(m => m.Name == baseName && SignaturesMatch(context, methodSymbol, m, out _));

                if (nonDefaultMethod is null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        RuleMissingNonDefault,
                        methodSymbol.Locations[0],
                        methodName,
                        baseName));
                }
                else
                {
                     VerifyDelegation(context, nonDefaultMethod, orNoneName);
                }
            }

            // Check for *OrNone overload
            // If defaultValue is present, we expect *OrNone WITHOUT that parameter
            string failureReason = "Method not found";
            var orNoneMethod = members
                .OfType<IMethodSymbol>()
                .FirstOrDefault(m => 
                {
                    if (m.Name != orNoneName) return false;
                    
                    var match = hasDefaultValueParam 
                        ? SignaturesMatchWithSkipLast(context, methodSymbol, m, out failureReason)
                        : SignaturesMatch(context, methodSymbol, m, out failureReason);
                    
                    return match;
                });

            if (orNoneMethod is null)
            {
                 context.ReportDiagnostic(Diagnostic.Create(
                    RuleMissingOption, 
                    methodSymbol.Locations[0], 
                    methodName, 
                    orNoneName,
                    failureReason));
            }
            else
            {
                VerifyDelegation(context, methodSymbol, orNoneName);
            }
        }
        else if (!methodName.EndsWith("OrNone"))
        {
            var targetOptionName = methodName + "OrNone";
            
             // Only check if it's one of the standard LINQ operators we care about
            if (methodName is "First" or "Single" or "ElementAt" or "Min" or "Max" or "MinMax")
            {
                var optionMethod = methodSymbol.ContainingType.GetMembers(targetOptionName)
                    .OfType<IMethodSymbol>()
                    .FirstOrDefault(m => SignaturesMatch(context, methodSymbol, m, out _));

                if (optionMethod is not null)
                {
                    VerifyDelegation(context, methodSymbol, targetOptionName);
                }
            }
        }

        var cleanName = methodName;
        if (cleanName.EndsWith("OrDefault")) cleanName = cleanName.Substring(0, cleanName.Length - 9);
        if (cleanName.EndsWith("OrNone")) cleanName = cleanName.Substring(0, cleanName.Length - 6);

        if (cleanName is "Min" or "Max" or "MinMax")
        {
            // Verify Min/Max/MinMax group
            var expectedMethods = new[] { "Min", "MinOrNone", "Max", "MaxOrNone", "MinMax", "MinMaxOrNone" };
            var missingMethods = new List<string>();

            var members = methodSymbol.ContainingType.GetMembers();

            foreach (var expectedMethod in expectedMethods)
            {
                if (expectedMethod == methodName) continue;

                var found = members
                    .OfType<IMethodSymbol>()
                    .Any(m => m.Name == expectedMethod && SignaturesMatch(context, methodSymbol, m, out _));

                if (!found)
                {
                    missingMethods.Add(expectedMethod);
                }
            }

            if (missingMethods.Count > 0)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    RuleMissingMinMaxGroup,
                    methodSymbol.Locations[0],
                    methodName,
                    string.Join(", ", missingMethods)));
            }
        }
    }

    private static bool IsInExtensionType(SymbolAnalysisContext context, IMethodSymbol method)
    {
        var typeSymbol = method.ContainingType;
        if (typeSymbol is null) return false;

        foreach (var syntaxRef in typeSymbol.DeclaringSyntaxReferences)
        {
             var syntax = syntaxRef.GetSyntax(context.CancellationToken);
             if (syntax.Kind().ToString() == "ExtensionDeclaration" || 
                 syntax.Kind().ToString() == "ExtensionBlockDeclaration" ||
                 syntax.GetType().Name == "ExtensionDeclarationSyntax")
                 return true;
        }
        return false;
    }

    private static bool SignaturesMatch(SymbolAnalysisContext context, IMethodSymbol method1, IMethodSymbol method2, out string reason)
    {
        reason = "";
        if (method1.TypeParameters.Length != method2.TypeParameters.Length) 
        {
            reason = $"Type params mismatch: {method1.TypeParameters.Length} vs {method2.TypeParameters.Length}";
            return false;
        }

        // Logic for parameter matching considering implied receiver in C# 14
        // If one is "Classic" (static + this) and other is "Instance" (C# 14), we might need offset.
        // BUT, in OptionOverloadAnalyzer, we are usually comparing methods WITHIN the same type.
        // It is unlikely one method in a type is "Classic Extension" and another is "Ordinary Instance" within the SAME type 
        // unless it's a partial class mix (unlikely for Extension Types).
        // For C# 14 Extension types, ALL methods are "instance-like" regarding parameters (receiver is not in param list).
        // For Classic static classes, ALL methods are static (receiver IS in param list).
        // So we can assume the offset is the same for both if they are in the same containing type.
        
        // HOWEVER, to be safe:
        // Use IsExtensionMethod to detect "Classic".
        // Use IsInExtensionType (or just !IsStatic if in extension type) for "New".
        
        // Simpler: Just compare parameters 1-to-1 if they are in the same type structure.
        // Since we are looking for counterparts IN THE SAME TYPE.
        
        if (method1.Parameters.Length != method2.Parameters.Length)
        {
            reason = $"Params length mismatch: {method1.Parameters.Length} vs {method2.Parameters.Length}";
            return false;
        }

        for (var i = 0; i < method1.Parameters.Length; i++)
        {
            var type1 = method1.Parameters[i].Type;
            var type2 = method2.Parameters[i].Type;

            if (!TypesMatch(type1, type2))
            {
                reason = $"Param {i} mismatch: {type1.ToDisplayString()} ({type1.TypeKind}) vs {type2.ToDisplayString()} ({type2.TypeKind})";
                return false;
            }
        }

        return true;
    }

    private static bool SignaturesMatchWithSkipLast(SymbolAnalysisContext context, IMethodSymbol methodWithDefault, IMethodSymbol methodTarget, out string reason)
    {
        reason = "";
        if (methodWithDefault.TypeParameters.Length != methodTarget.TypeParameters.Length)
        {
            reason = $"Type params mismatch: {methodWithDefault.TypeParameters.Length} vs {methodTarget.TypeParameters.Length}";
            return false;
        }

        if (methodWithDefault.Parameters.Length - 1 != methodTarget.Parameters.Length)
        {
             reason = $"Params length mismatch (skip last): {methodWithDefault.Parameters.Length} vs {methodTarget.Parameters.Length}";
            return false;
        }

        for (var i = 0; i < methodTarget.Parameters.Length; i++)
        {
            var type1 = methodWithDefault.Parameters[i].Type;
            var type2 = methodTarget.Parameters[i].Type;

            if (!TypesMatch(type1, type2))
            {
                 reason = $"Param {i} mismatch: {type1.ToDisplayString()} ({type1.TypeKind}) vs {type2.ToDisplayString()} ({type2.TypeKind})";
                return false;
            }
        }

        return true;
    }

    private static bool TypesMatch(ITypeSymbol type1, ITypeSymbol type2)
    {
        if (SymbolEqualityComparer.Default.Equals(type1, type2))
            return true;

        if (type1.TypeKind == TypeKind.TypeParameter && type2.TypeKind == TypeKind.TypeParameter)
        {
            return type1.Name == type2.Name;
        }

        if (type1 is INamedTypeSymbol named1 && type2 is INamedTypeSymbol named2)
        {
            if (!SymbolEqualityComparer.Default.Equals(named1.OriginalDefinition, named2.OriginalDefinition))
                return false;

            if (named1.TypeArguments.Length != named2.TypeArguments.Length)
                return false;

            for (var i = 0; i < named1.TypeArguments.Length; i++)
            {
                if (!TypesMatch(named1.TypeArguments[i], named2.TypeArguments[i]))
                    return false;
            }

            return true;
        }

        return false;
    }

    private void VerifyDelegation(SymbolAnalysisContext context, IMethodSymbol sourceMethod, string targetMethodName)
    {
        var syntaxReference = sourceMethod.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxReference is null) return;

        var root = syntaxReference.SyntaxTree.GetRoot(context.CancellationToken);
        var methodDeclaration = root.FindNode(syntaxReference.Span) as MethodDeclarationSyntax;
        
        if (methodDeclaration is null) return;

        var invokesTarget = methodDeclaration.DescendantNodes()
            .OfType<InvocationExpressionSyntax>()
            .Any(inv => 
            {
               if (inv.Expression is IdentifierNameSyntax identifierName)
               {
                   return identifierName.Identifier.Text == targetMethodName;
               }
               if (inv.Expression is MemberAccessExpressionSyntax memberAccess)
               {
                   return memberAccess.Name.Identifier.Text == targetMethodName;
               }
               
               // Handle generic methods e.g. targetMethodName<T>
               if (inv.Expression is GenericNameSyntax genericName)
               {
                   return genericName.Identifier.Text == targetMethodName;
               }

               return false;
            });

        if (!invokesTarget)
        {
            context.ReportDiagnostic(Diagnostic.Create(
                RuleDelegation,
                sourceMethod.Locations[0],
                sourceMethod.Name,
                targetMethodName));
        }
    }
}
