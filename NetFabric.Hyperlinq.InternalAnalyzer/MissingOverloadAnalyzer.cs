using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NetFabric.Hyperlinq.InternalAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class MissingOverloadAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "HLQInternal002X";

    private static readonly LocalizableString Title = "Missing overload for ReadOnlySpan extension (Use -X to verify update)";
    private static readonly LocalizableString MessageFormat = "The extension method '{0}' for ReadOnlySpan<T> is missing an equivalent overload for '{1}'";
    private static readonly LocalizableString Description = "All ReadOnlySpan<T> extensions must have overloads for Array, List, ArraySegment, and ReadOnlyMemory";
    private const string Category = "Design";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: Description);

    public const string DiagnosticIdDelegation = "HLQInternal003X"; // Renamed to verify update
    private static readonly LocalizableString TitleDelegation = "Overload must delegate to ReadOnlySpan extension";
    private static readonly LocalizableString MessageFormatDelegation = "The overload '{0}' must delegate execution to the ReadOnlySpan<T> extension method";
    private static readonly DiagnosticDescriptor RuleDelegation = new DiagnosticDescriptor(
        DiagnosticIdDelegation,
        TitleDelegation,
        MessageFormatDelegation,
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public const string DiagnosticIdInlining = "HLQInternal004";
    private static readonly LocalizableString TitleInlining = "Overload must be aggressively inlined";
    private static readonly LocalizableString MessageFormatInlining = "The overload '{0}' must be annotated with [MethodImpl(MethodImplOptions.AggressiveInlining)]";
    private static readonly DiagnosticDescriptor RuleInlining = new DiagnosticDescriptor(
        DiagnosticIdInlining,
        TitleInlining,
        MessageFormatInlining,
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor RuleDebug = new DiagnosticDescriptor(
        "HLQInternalDEBUG",
        "Debug Info",
        "{0}",
        "Debug",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule, RuleDelegation, RuleInlining, RuleDebug);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(AnalyzeMethod, SymbolKind.Method);
    }

    private void AnalyzeMethod(SymbolAnalysisContext context)
    {
        var methodSymbol = (IMethodSymbol)context.Symbol;

        // 1. Check if it's an extension method or C# 14 extension
        // For C# 14, IsExtensionMethod is FALSE, but it's Inside an Extension Type.
        // We defer check to TryGetReceiverType.

        // 2. Check if the receiver is ReadOnlySpan<T>
        if (!TryGetReceiverType(context, methodSymbol, out var firstParamType))
            return;

        if (!IsReadOnlySpan(firstParamType))
            return;

        // 3. Define required overloads
        var compilation = context.Compilation;
        var arrayType = compilation.GetSpecialType(SpecialType.System_Array);
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
        
        // Find candidates: check both extension methods and methods in extension types
        // Note: In C# 14, extension types might be nested within the container class.
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
                     // Check if paramType matches targetSymbol (generic definition)
                    isTypeMatch = SymbolEqualityComparer.Default.Equals(paramType.OriginalDefinition, targetSymbol);
                }

                return isTypeMatch && SignaturesMatch(context, methodSymbol, m);
            });

            if (overload is null)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    Rule, 
                    methodSymbol.Locations[0], 
                    methodSymbol.Name, 
                    simpleName));
            }
            else
            {
                // Check if the method returns a ValueEnumerable
                var returnTypeName = methodSymbol.ReturnType.Name;
                var returnTypeMetadataName = methodSymbol.ReturnType.MetadataName;
                var isValueEnumerable = returnTypeName.Contains("Enumerable") || returnTypeMetadataName.Contains("Enumerable");

                // Check if the method returns a ValueEnumerable (DEBUG LOGGING ENABLED)
                if (!isValueEnumerable && methodSymbol.Name == "Select")
                {
                     context.ReportDiagnostic(Diagnostic.Create(RuleDebug, methodSymbol.Locations[0], 
                         $"Select ReturnType: '{returnTypeName}', Metadata: '{returnTypeMetadataName}'"));
                }

                 // Skip array overload requirement for ValueEnumerables as they likely use Span implementation
                 if (targetKind == TypeKind.Array && isValueEnumerable)
                     continue;

                 // Skip delegation check for ValueEnumerables
                 if (!isValueEnumerable)
                 {
                     VerifyDelegation(context, overload, methodSymbol);
                 }
                 
                 VerifyInlining(context, overload);
            }
        }
    }

    private static bool IsReadOnlySpan(ITypeSymbol typeSymbol)
    {
        return typeSymbol.OriginalDefinition.ToString() == "System.ReadOnlySpan<T>";
    }

    private static bool TryGetReceiverType(SymbolAnalysisContext context, IMethodSymbol method, out ITypeSymbol receiverType)
    {
        receiverType = null!;
        
        // 1. Classic Extension Method (static method with 'this' param)
        if (method.IsExtensionMethod)
        {
            if (method.Parameters.Length > 0)
            {
                receiverType = method.Parameters[0].Type;
                return true;
            }
            return false;
        }

        // 2. C# 14 Explicit Extension Type
        // Start by checking DeclaringSyntaxReferences (usually 1)
        var syntaxRef = method.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxRef is not null)
        {
            var syntax = syntaxRef.GetSyntax(context.CancellationToken);
            
            // Traverse up to find ExtensionDeclaration
            var parent = syntax.Parent;
            while (parent is not null)
            {
                var kindName = parent.Kind().ToString();
                
                if (kindName == "ExtensionDeclaration" || kindName == "ExtensionBlockDeclaration") 
                {
                     // Found it. Use reflection.
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
                     break; // Found extension declaration, stop traversing
                }
                
                parent = parent.Parent;
            }
        }
        
        return false;
    }

    private static bool SignaturesMatch(SymbolAnalysisContext context, IMethodSymbol spanMethod, IMethodSymbol candidate)
    {
        // Compare generic parameters
        if (spanMethod.TypeParameters.Length != candidate.TypeParameters.Length)
            return false;

        // Compare parameters starting from index 1 (skipping 'this' parameter)
        if (!TryGetReceiverType(context, spanMethod, out _)) return false;
        if (!TryGetReceiverType(context, candidate, out _)) return false; // Also verify candidate is compatible extension type
        
        // Offset logic:
        // Classic: calls are static(source, ...). Params: [source, arg1, ...]
        // C# 14: calls are source.Method(...). Params: [arg1, ...] (Instance method of extension type)
        // Wait, C# 14 extension methods in metadata might be different?
        // Actually, for C# 14, the 'source' is in the Type declaration, not the Method parameters?
        // Yes. 
        // So for C# 14 method, Parameters.Length is just the arguments.
        // For Classic method, Parameters.Length is source + arguments.
        
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

    private static bool AreEquivalent(ITypeSymbol t1, ITypeSymbol t2)
    {
        if (SymbolEqualityComparer.Default.Equals(t1, t2))
            return true;

        if (t1.TypeKind == TypeKind.TypeParameter && t2.TypeKind == TypeKind.TypeParameter)
        {
            var tp1 = (ITypeParameterSymbol)t1;
            var tp2 = (ITypeParameterSymbol)t2;

            // Check if both are method type parameters or both are type type parameters
            if (tp1.DeclaringMethod is not null && tp2.DeclaringMethod is not null)
            {
                 // Compare by name as ordinal might differ between extension vs extension type
                 return tp1.Name == tp2.Name; 
            }
            
            if (tp1.DeclaringType is not null && tp2.DeclaringType is not null)
            {
                 // Assuming strict 1:1 mapping of class type parameters for now (e.g. extension<T> vs extension<T>)
                 return tp1.Ordinal == tp2.Ordinal;
            }
            
            return false;
        }

        if (t1 is IArrayTypeSymbol array1 && t2 is IArrayTypeSymbol array2)
        {
            return array1.Rank == array2.Rank && AreEquivalent(array1.ElementType, array2.ElementType);
        }

        if (t1 is INamedTypeSymbol named1 && t2 is INamedTypeSymbol named2)
        {
            if (!SymbolEqualityComparer.Default.Equals(named1.OriginalDefinition, named2.OriginalDefinition))
                return false;

            if (named1.TypeArguments.Length != named2.TypeArguments.Length)
                return false;

            for (var i = 0; i < named1.TypeArguments.Length; i++)
            {
                if (!AreEquivalent(named1.TypeArguments[i], named2.TypeArguments[i]))
                    return false;
            }

            return true;
        }

        return false;
    }

    private void VerifyDelegation(SymbolAnalysisContext context, IMethodSymbol overload, IMethodSymbol targetSpanMethod)
    {
        // HLQInternal007 (OptionOverloadAnalyzer) handles delegation validation for these operations
        if (targetSpanMethod.Name == "First" || 
            targetSpanMethod.Name == "FirstOrDefault" ||
            targetSpanMethod.Name == "Single" || 
            targetSpanMethod.Name == "SingleOrDefault" ||
            targetSpanMethod.Name == "Last" || 
            targetSpanMethod.Name == "LastOrDefault" ||
            targetSpanMethod.Name == "ElementAt" || 
            targetSpanMethod.Name == "ElementAtOrDefault" ||
            targetSpanMethod.Name == "Min" || 
            targetSpanMethod.Name == "Max" ||
            targetSpanMethod.Name == "MinMax" ||
            targetSpanMethod.Name == "Skip" ||
            targetSpanMethod.Name == "Take")
            return;

        // This requires getting the syntax of the overload
        var syntaxReference = overload.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxReference is null) return;

        var root = syntaxReference.SyntaxTree.GetRoot(context.CancellationToken);
        var methodDeclaration = root.FindNode(syntaxReference.Span) as MethodDeclarationSyntax;
        
        if (methodDeclaration is null) return;

        // Very basic check: look for invocation of a method with the same name
        // A more robust check would use IOperation, but this is a good start.
        var invokesTarget = methodDeclaration.DescendantNodes()
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
            context.ReportDiagnostic(Diagnostic.Create(
                RuleDelegation,
                overload.Locations[0],
                overload.Name));
        }
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

        var argument = constructorArguments[0];
        
        // Check if AggressiveInlining (256) is set
        var value = (int)argument.Value!;
        if ((value & 256) != 256) // MethodImplOptions.AggressiveInlining
        {
            context.ReportDiagnostic(Diagnostic.Create(RuleInlining, methodSymbol.Locations[0], methodSymbol.Name));
        }
    }
}
