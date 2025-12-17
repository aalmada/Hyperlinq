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
    public const string DiagnosticId = "HLQInternal002";

    private static readonly LocalizableString Title = "Missing overload for ReadOnlySpan extension";
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

    public const string DiagnosticIdDelegation = "HLQInternal003";
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

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule, RuleDelegation, RuleInlining);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(AnalyzeMethod, SymbolKind.Method);
    }

    private void AnalyzeMethod(SymbolAnalysisContext context)
    {
        var methodSymbol = (IMethodSymbol)context.Symbol;

        // 1. Check if it's an extension method
        if (!methodSymbol.IsExtensionMethod)
            return;

        // 2. Check if the first parameter is ReadOnlySpan<T>
        if (methodSymbol.Parameters.Length == 0)
            return;

        var firstParamType = methodSymbol.Parameters[0].Type;
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
        
        var candidates = typesInNamespace
            .SelectMany(t => t.GetMembers(methodSymbol.Name).OfType<IMethodSymbol>())
            .Where(m => m.IsExtensionMethod && m.Parameters.Length == methodSymbol.Parameters.Length)
            .ToList();

        foreach (var (targetSymbol, simpleName, targetKind) in requiredTypes)
        {
            var overload = candidates.FirstOrDefault(m => 
            {
                var paramType = m.Parameters[0].Type;
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

                return isTypeMatch && SignaturesMatch(methodSymbol, m);
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
                VerifyDelegation(context, overload, methodSymbol);
                VerifyInlining(context, overload);
            }
        }
    }

    private static bool IsReadOnlySpan(ITypeSymbol typeSymbol)
    {
        return typeSymbol.OriginalDefinition.ToString() == "System.ReadOnlySpan<T>";
    }

    private static bool SignaturesMatch(IMethodSymbol spanMethod, IMethodSymbol candidate)
    {
        // Compare generic parameters
        if (spanMethod.TypeParameters.Length != candidate.TypeParameters.Length)
            return false;

        // Compare parameters starting from index 1 (skipping 'this' parameter)
        for (var i = 1; i < spanMethod.Parameters.Length; i++)
        {
            // Simple type comparison - strict equality might be too harsh if generic type arguments differ slightly, 
            // but for overloads they usually match exactly.
            if (!SymbolEqualityComparer.Default.Equals(spanMethod.Parameters[i].Type, candidate.Parameters[i].Type))
                return false;
        }

        return true;
    }

    private void VerifyDelegation(SymbolAnalysisContext context, IMethodSymbol overload, IMethodSymbol targetSpanMethod)
    {
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
