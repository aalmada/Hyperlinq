using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NetFabric.Hyperlinq.InternalAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ContiguousMemoryAnalyzer : DiagnosticAnalyzer
{
    // --- HLQInternal002: ReadOnlySpan Extension Parity ---
    public const string DiagnosticIdExtensionParity = "HLQInternal002"; // Renamed from X to final
    private static readonly LocalizableString TitleExtensionParity = "Missing overload for ReadOnlySpan extension";
    private static readonly LocalizableString MessageFormatExtensionParity = "The extension method '{0}' for ReadOnlySpan<T> is missing an equivalent overload for '{1}'";
    private static readonly LocalizableString DescriptionExtensionParity = "All ReadOnlySpan<T> extensions must have overloads for Array, List, ArraySegment, and ReadOnlyMemory";
    private static readonly DiagnosticDescriptor RuleExtensionParity = new DiagnosticDescriptor(
        DiagnosticIdExtensionParity,
        TitleExtensionParity,
        MessageFormatExtensionParity,
        "Design",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: DescriptionExtensionParity);

    // --- HLQInternal003: Delegation Check (Generic) ---
    public const string DiagnosticIdDelegation = "HLQInternal003";
    private static readonly LocalizableString TitleDelegation = "Delegate to ReadOnlySpan<T>";
    private static readonly LocalizableString MessageFormatDelegation = "The method '{0}' does not delegate to the ReadOnlySpan<T> counterpart '{1}'. Ensure it converts the source to a span and calls the optimized implementation.";
    private static readonly LocalizableString DescriptionDelegation = "Implementations for types with contiguous memory must delegate to the ReadOnlySpan<T> counterpart.";
    private static readonly DiagnosticDescriptor RuleDelegation = new DiagnosticDescriptor(
        DiagnosticIdDelegation,
        TitleDelegation,
        MessageFormatDelegation,
        "Performance",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: DescriptionDelegation);

    // --- HLQInternal004: Inlining Check ---
    public const string DiagnosticIdInlining = "HLQInternal004";
    private static readonly LocalizableString TitleInlining = "Overload must be aggressively inlined";
    private static readonly LocalizableString MessageFormatInlining = "The overload '{0}' must be annotated with [MethodImpl(MethodImplOptions.AggressiveInlining)]";
    private static readonly DiagnosticDescriptor RuleInlining = new DiagnosticDescriptor(
        DiagnosticIdInlining,
        TitleInlining,
        MessageFormatInlining,
        "Performance",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    // --- HLQInternal009: ToArray/ToList Parity ---
    public const string DiagnosticIdConversionParity = "HLQInternal009";
    private static readonly LocalizableString TitleConversionParity = "Missing ToArray/ToList parity";
    private static readonly LocalizableString MessageFormatConversionParity = "The method '{0}' is missing an equivalent '{1}' overload";
    private static readonly LocalizableString DescriptionConversionParity = "If ToArray is defined, ToList must be defined with the same signature, and vice-versa.";
    private static readonly DiagnosticDescriptor RuleConversionParity = new DiagnosticDescriptor(
        DiagnosticIdConversionParity,
        TitleConversionParity,
        MessageFormatConversionParity,
        "Design",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: DescriptionConversionParity);

    // --- HLQInternal010: ToArray/ToList Delegation ---
    public const string DiagnosticIdConversionDelegation = "HLQInternal010";
    private static readonly LocalizableString TitleConversionDelegation = "Delegate to ReadOnlySpan<T>";
    private static readonly LocalizableString MessageFormatConversionDelegation = "The method '{0}' does not delegate to the ReadOnlySpan<T> counterpart '{0}'. Ensure it converts the source to a span and calls the optimized implementation.";
    private static readonly LocalizableString DescriptionConversionDelegation = "Implementations for types with contiguous memory must delegate to the ReadOnlySpan<T> counterpart.";
    private static readonly DiagnosticDescriptor RuleConversionDelegation = new DiagnosticDescriptor(
        DiagnosticIdConversionDelegation,
        TitleConversionDelegation,
        MessageFormatConversionDelegation,
        "Performance",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: DescriptionConversionDelegation);

    private static readonly DiagnosticDescriptor RuleDebug = new DiagnosticDescriptor(
        "HLQInternalDEBUG",
        "Debug Info",
        "{0}",
        "Debug",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        RuleExtensionParity, 
        RuleDelegation, 
        RuleInlining, 
        RuleConversionParity, 
        RuleConversionDelegation, 
        RuleDebug);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(AnalyzeMethod, SymbolKind.Method);
    }

    private void AnalyzeMethod(SymbolAnalysisContext context)
    {
        var methodSymbol = (IMethodSymbol)context.Symbol;

        // 1. Check for Conversion Methods (ToArray/ToList)
        if (methodSymbol.Name == "ToArray" || methodSymbol.Name == "ToList")
        {
            AnalyzeConversionMethod(context, methodSymbol);
            return;
        }

        // 2. Check for ReadOnlySpan Extensions (only if receiver is ReadOnlySpan<T>)
        if (IsReadOnlySpanExtension(context, methodSymbol))
        {
            AnalyzeReadOnlySpanExtension(context, methodSymbol);
        }
    }

    // --- Logic for ToArray/ToList (HLQInternal009, HLQInternal010) ---

    private void AnalyzeConversionMethod(SymbolAnalysisContext context, IMethodSymbol methodSymbol)
    {
        if (!TryGetReceiverType(context, methodSymbol, out var receiverType))
            return;

        // HLQInternal009: Parity
        var counterpartName = methodSymbol.Name == "ToArray" ? "ToList" : "ToArray";
        var containingNamespace = methodSymbol.ContainingNamespace;
        var typesInNamespace = containingNamespace.GetMembers().OfType<INamedTypeSymbol>();

        var candidates = typesInNamespace
            .SelectMany(t => 
                t.GetMembers(counterpartName).OfType<IMethodSymbol>()
                .Concat(t.GetTypeMembers().SelectMany(nested => nested.GetMembers(counterpartName).OfType<IMethodSymbol>()))
            )
            .Where(m => 
            {
                 if (m.IsExtensionMethod && TryGetReceiverType(context, m, out var rType) && AreEquivalent(rType, receiverType)) return true;
                 if (!m.IsExtensionMethod && TryGetReceiverType(context, m, out var rType2) && AreEquivalent(rType2, receiverType)) return true;
                 return false;
            })
            .ToList();

        if (!HasEquivalentOverload(context, methodSymbol, candidates))
        {
            context.ReportDiagnostic(Diagnostic.Create(RuleConversionParity, methodSymbol.Locations[0], methodSymbol.Name, counterpartName));
        }

        // HLQInternal010: Delegation
        if (IsContiguousMemoryType(receiverType))
        {
            VerifyDelegationRef(context, methodSymbol, methodSymbol.Name, receiverType, RuleConversionDelegation);
        }
    }

    // --- Logic for ReadOnlySpan Extensions (HLQInternal002, HLQInternal003, HLQInternal004) ---

    private void AnalyzeReadOnlySpanExtension(SymbolAnalysisContext context, IMethodSymbol methodSymbol)
    {
        var compilation = context.Compilation;
        var listType = compilation.GetTypeByMetadataName("System.Collections.Generic.List`1");
        var arraySegmentType = compilation.GetTypeByMetadataName("System.ArraySegment`1");
        var readOnlyMemoryType = compilation.GetTypeByMetadataName("System.ReadOnlyMemory`1");

        var requiredTypes = new[]
        {
            (Symbol: (INamedTypeSymbol?)null, Name: "T[]", Kind: TypeKind.Array), // Array is special
            (Symbol: listType, Name: "List<T>", Kind: TypeKind.Class),
            (Symbol: arraySegmentType, Name: "ArraySegment<T>", Kind: TypeKind.Struct),
            (Symbol: readOnlyMemoryType, Name: "ReadOnlyMemory<T>", Kind: TypeKind.Struct)
        };

        var containingNamespace = methodSymbol.ContainingNamespace;
        var typesInNamespace = containingNamespace.GetMembers().OfType<INamedTypeSymbol>();
        
        var candidates = typesInNamespace
            .SelectMany(t => 
                t.GetMembers(methodSymbol.Name).OfType<IMethodSymbol>()
                .Concat(t.GetTypeMembers().SelectMany(nested => nested.GetMembers(methodSymbol.Name).OfType<IMethodSymbol>()))
            )
            .Where(m => 
            {
                 if (m.IsExtensionMethod) return true;
                 if (TryGetReceiverType(context, m, out _)) return true;
                 return false;
            })
            .ToList();

        foreach (var (targetSymbol, simpleName, targetKind) in requiredTypes)
        {
            var overload = candidates.FirstOrDefault(m => 
            {
                if (!TryGetReceiverType(context, m, out var paramType)) return false;
                
                bool isTypeMatch = false;

                if (targetKind == TypeKind.Array)
                {
                    isTypeMatch = paramType.TypeKind == TypeKind.Array;
                }
                else if (targetSymbol is not null)
                {
                    isTypeMatch = SymbolEqualityComparer.Default.Equals(paramType.OriginalDefinition, targetSymbol);
                }

                return isTypeMatch && SignaturesMatch(context, methodSymbol, m);
            });

            if (overload is null)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    RuleExtensionParity, 
                    methodSymbol.Locations[0], 
                    methodSymbol.Name, 
                    simpleName));
            }
            else
            {
                var returnTypeName = methodSymbol.ReturnType.Name;
                var returnTypeMetadataName = methodSymbol.ReturnType.MetadataName;
                var isValueEnumerable = returnTypeName.Contains("Enumerable") || returnTypeMetadataName.Contains("Enumerable");

                if (!isValueEnumerable && methodSymbol.Name == "Select")
                {
                     context.ReportDiagnostic(Diagnostic.Create(RuleDebug, methodSymbol.Locations[0], 
                         $"Select ReturnType: '{returnTypeName}', Metadata: '{returnTypeMetadataName}'"));
                }

                 // Skip array requirement for ValueEnumerables
                 if (targetKind == TypeKind.Array && isValueEnumerable)
                     continue;

                 // Skip delegation check for ValueEnumerables
                 if (!isValueEnumerable)
                 {
                     // Use the extension verification logic which checks body invoke
                     VerifyDelegation(context, overload, methodSymbol, RuleDelegation);
                 }
                 
                 VerifyInlining(context, overload);
            }
        }
    }

    // --- Helpers ---

    private static bool IsReadOnlySpanExtension(SymbolAnalysisContext context, IMethodSymbol method)
    {
        if (!TryGetReceiverType(context, method, out var receiverType))
            return false;

        return receiverType.OriginalDefinition.ToString() == "System.ReadOnlySpan<T>";
    }

    private static bool IsContiguousMemoryType(ITypeSymbol type)
    {
        if (type.TypeKind == TypeKind.Array)
            return true;
        
        if (type.Name == "List" && type.ContainingNamespace.ToString() == "System.Collections.Generic")
            return true;

        if (type.Name == "ArraySegment" && type.ContainingNamespace.ToString() == "System")
            return true;

        if ((type.Name == "ReadOnlyMemory" || type.Name == "Memory") && type.ContainingNamespace.ToString() == "System")
            return true;

        if (type.Name == "ImmutableArray" && type.ContainingNamespace.ToString() == "System.Collections.Immutable")
            return true;

        return false;
    }

    private static bool TryGetReceiverType(SymbolAnalysisContext context, IMethodSymbol method, out ITypeSymbol receiverType)
    {
        receiverType = null!;
        
        if (method.IsExtensionMethod)
        {
            if (method.Parameters.Length > 0)
            {
                receiverType = method.Parameters[0].Type;
                return true;
            }
            return false;
        }

        var syntaxRef = method.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxRef is not null)
        {
            var syntax = syntaxRef.GetSyntax(context.CancellationToken);
            var parent = syntax.Parent;
            while (parent is not null)
            {
                var kindName = parent.Kind().ToString();
                if (kindName == "ExtensionDeclaration" || kindName == "ExtensionBlockDeclaration") 
                {
                     var type = parent.GetType();
                     var paramListProp = type.GetProperty("ParameterList");
                     if (paramListProp is not null)
                     {
                          var paramList = paramListProp.GetValue(parent);
                          if (paramList is not null)
                          {
                               var parametersProp = paramList.GetType().GetProperty("Parameters");
                               var parameters = parametersProp?.GetValue(paramList) as System.Collections.IEnumerable;
                               if (parameters is not null)
                               {
                                   object? firstParam = null;
                                   foreach(var p in parameters) { firstParam = p; break; }
                                   if (firstParam is not null)
                                   {
                                        var typeProp = firstParam.GetType().GetProperty("Type");
                                        var typeSyntax = typeProp?.GetValue(firstParam) as TypeSyntax;
                                        if (typeSyntax is not null)
                                        {
                                             var semanticModel = context.Compilation.GetSemanticModel(parent.SyntaxTree);
                                             var typeInfo = semanticModel.GetTypeInfo(typeSyntax, context.CancellationToken);
                                             if (typeInfo.Type is not null)
                                             {
                                                 receiverType = typeInfo.Type;
                                                 return true;
                                             }
                                        }
                                   }
                               }
                          }
                     }
                     break; 
                }
                parent = parent.Parent;
            }
        }
        return false;
    }

    private static bool SignaturesMatch(SymbolAnalysisContext context, IMethodSymbol spanMethod, IMethodSymbol candidate)
    {
        if (spanMethod.TypeParameters.Length != candidate.TypeParameters.Length)
            return false;
        
        if (!TryGetReceiverType(context, spanMethod, out _)) return false;
        if (!TryGetReceiverType(context, candidate, out _)) return false; 
        
        var spanIsClassic = spanMethod.IsExtensionMethod;
        var candidateIsClassic = candidate.IsExtensionMethod;
        
        var spanOffset = spanIsClassic ? 1 : 0;
        var candidateOffset = candidateIsClassic ? 1 : 0;

        if ((spanMethod.Parameters.Length - spanOffset) != (candidate.Parameters.Length - candidateOffset))
             return false;

        for (var i = 0; i < (spanMethod.Parameters.Length - spanOffset); i++)
        {
            var spanParam = spanMethod.Parameters[i + spanOffset];
            var candidateParam = candidate.Parameters[i + candidateOffset];

            if (!AreEquivalent(spanParam.Type, candidateParam.Type))
                return false;
        }

        return true;
    }

    // Specialized logic for HLQInternal009 (ToArray/ToList)
    private static bool HasEquivalentOverload(SymbolAnalysisContext context, IMethodSymbol target, System.Collections.Generic.List<IMethodSymbol> candidates)
    {
        return candidates.Any(candidate => 
        {
            if (target.TypeParameters.Length != candidate.TypeParameters.Length) return false;
            
            var params1 = GetRelevantParameters(target).ToList();
            var params2 = GetRelevantParameters(candidate).ToList();
            
            if (params1.Count != params2.Count) return false;
            
            for (var i = 0; i < params1.Count; i++)
            {
                if (!AreEquivalent(params1[i].Type, params2[i].Type)) return false;
            }
            return true;
        });
    }

    private static IEnumerable<IParameterSymbol> GetRelevantParameters(IMethodSymbol method)
    {
        var isExtension = method.IsExtensionMethod;
        var parameters = method.Parameters;
        
        for (var i = 0; i < parameters.Length; i++)
        {
            if (isExtension && i == 0) continue; // Skip receiver
            var param = parameters[i];
            var typeName = param.Type.Name;
            var typeMetadata = param.Type.MetadataName;
            if (typeName == "ArrayPool" || typeMetadata == "ArrayPool`1") continue;
            yield return param;
        }
    }

    private static bool AreEquivalent(ITypeSymbol t1, ITypeSymbol t2)
    {
        if (SymbolEqualityComparer.Default.Equals(t1, t2)) return true;

        if (t1.TypeKind == TypeKind.TypeParameter && t2.TypeKind == TypeKind.TypeParameter)
        {
            var tp1 = (ITypeParameterSymbol)t1;
            var tp2 = (ITypeParameterSymbol)t2;
            if (tp1.DeclaringMethod is not null && tp2.DeclaringMethod is not null) return tp1.Name == tp2.Name;
            if (tp1.DeclaringType is not null && tp2.DeclaringType is not null) return tp1.Ordinal == tp2.Ordinal;
            return tp1.Name == tp2.Name;
        }

        if (t1 is IArrayTypeSymbol array1 && t2 is IArrayTypeSymbol array2)
        {
             return array1.Rank == array2.Rank && AreEquivalent(array1.ElementType, array2.ElementType);
        }

        if (t1 is INamedTypeSymbol named1 && t2 is INamedTypeSymbol named2)
        {
            if (!SymbolEqualityComparer.Default.Equals(named1.OriginalDefinition, named2.OriginalDefinition)) return false;
            if (named1.TypeArguments.Length != named2.TypeArguments.Length) return false;
            for (var i = 0; i < named1.TypeArguments.Length; i++)
            {
                if (!AreEquivalent(named1.TypeArguments[i], named2.TypeArguments[i])) return false;
            }
            return true;
        }

        return false;
    }

    private void VerifyDelegationRef(SymbolAnalysisContext context, IMethodSymbol overload, string targetName, ITypeSymbol receiverType, DiagnosticDescriptor rule)
    {
        // Used by HLQInternal010
        var syntaxRef = overload.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxRef is not null)
        {
            var root = syntaxRef.SyntaxTree.GetRoot(context.CancellationToken);
            var node = root.FindNode(syntaxRef.Span);
            if (node is MethodDeclarationSyntax methodDecl)
            {
                var semanticModel = context.Compilation.GetSemanticModel(syntaxRef.SyntaxTree);
                if (!CallsCorrectCounterpart(semanticModel, methodDecl, targetName, receiverType))
                {
                     context.ReportDiagnostic(Diagnostic.Create(rule, overload.Locations[0], overload.Name));
                }
            }
        }
    }
    
    private void VerifyDelegation(SymbolAnalysisContext context, IMethodSymbol overload, IMethodSymbol targetSpanMethod, DiagnosticDescriptor rule)
    {
        // Used by HLQInternal003
        if (IsExemptFromDelegation(targetSpanMethod.Name)) return;

        var syntaxRef = overload.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxRef is null) return;

        var root = syntaxRef.SyntaxTree.GetRoot(context.CancellationToken);
        var methodDecl = root.FindNode(syntaxRef.Span) as MethodDeclarationSyntax;
        if (methodDecl is null) return;

        var invokesTarget = methodDecl.DescendantNodes()
            .OfType<InvocationExpressionSyntax>()
            .Any(inv => 
            {
                if (inv.Expression is MemberAccessExpressionSyntax memberAccess)
                {
                    return memberAccess.Name.Identifier.Text == targetSpanMethod.Name;
                }
                return false;
            });

        if (!invokesTarget)
        {
            context.ReportDiagnostic(Diagnostic.Create(rule, overload.Locations[0], overload.Name, targetSpanMethod.Name));
        }
    }

    private static bool IsExemptFromDelegation(string name)
    {
         return name == "First" || name == "FirstOrDefault" ||
             name == "Single" || name == "SingleOrDefault" ||
             name == "Last" || name == "LastOrDefault" ||
             name == "ElementAt" || name == "ElementAtOrDefault" ||
             name == "Min" || name == "Max" || name == "MinMax" ||
             name == "Skip" || name == "Take";
    }

    private static bool CallsCorrectCounterpart(SemanticModel semanticModel, MethodDeclarationSyntax method, string methodName, ITypeSymbol receiverType)
    {
        ExpressionSyntax? expression = null;
        if (method.ExpressionBody is not null) expression = method.ExpressionBody.Expression;
        else if (method.Body is not null && method.Body.Statements.Count == 1)
        {
            if (method.Body.Statements[0] is ReturnStatementSyntax returnStmt) expression = returnStmt.Expression;
        }

        if (expression is null) return false;
        if (expression is InvocationExpressionSyntax invocation)
        {
            var symbol = semanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
            if (symbol is null) return false;
            if (symbol.Name != methodName) return false;

            var containingType = symbol.ContainingType;
            if (containingType is null) return false;

            // For Memory<T>, check if it delegates to ReadOnlyMemory<T>
            if (receiverType.Name == "Memory" && receiverType.ContainingNamespace.ToString() == "System")
            {
                // Should delegate to ReadOnlyMemory extensions
                return MatchesHyperlinqReadOnlyMemoryExtensions(containingType);
            }

            // For other contiguous types, check if it delegates to ReadOnlySpan<T>
            if (containingType.ContainingNamespace.ToString() == "System")
            {
                var name = containingType.Name;
                if (name == "ReadOnlySpan" || name == "Span" || name == "MemoryExtensions") return true;
            }
            if (MatchesHyperlinqReadOnlySpanExtensions(containingType)) return true;
            if (containingType.ContainingType is not null && MatchesHyperlinqReadOnlySpanExtensions(containingType.ContainingType)) return true;
        }
        return false;
    }

    private static bool MatchesHyperlinqReadOnlyMemoryExtensions(INamedTypeSymbol type)
    {
        // Check if this is ReadOnlyMemoryExtensions or nested within it
        if (type.Name == "ReadOnlyMemoryExtensions" && type.ContainingNamespace.ToString() == "NetFabric.Hyperlinq")
            return true;
        
        // Check if the containing type is ReadOnlyMemoryExtensions (for extension blocks)
        if (type.ContainingType is INamedTypeSymbol containingType &&
            containingType.Name == "ReadOnlyMemoryExtensions" && 
            containingType.ContainingNamespace.ToString() == "NetFabric.Hyperlinq")
            return true;
            
        return false;
    }

    private static bool MatchesHyperlinqReadOnlySpanExtensions(INamedTypeSymbol type)
    {
        return type.Name == "ReadOnlySpanExtensions" && type.ContainingNamespace.ToString() == "NetFabric.Hyperlinq";
    }

    private void VerifyInlining(SymbolAnalysisContext context, IMethodSymbol methodSymbol)
    {
        var methodImplAttribute = methodSymbol.GetAttributes()
            .FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == "System.Runtime.CompilerServices.MethodImplAttribute");
        if (methodImplAttribute is null)
        {
            context.ReportDiagnostic(Diagnostic.Create(RuleInlining, methodSymbol.Locations[0], methodSymbol.Name));
            return;
        }
        var constructorArguments = methodImplAttribute.ConstructorArguments;
        if (constructorArguments.Length == 0) return;
        var value = (int)constructorArguments[0].Value!;
        if ((value & 256) != 256)
        {
            context.ReportDiagnostic(Diagnostic.Create(RuleInlining, methodSymbol.Locations[0], methodSymbol.Name));
        }
    }
}
