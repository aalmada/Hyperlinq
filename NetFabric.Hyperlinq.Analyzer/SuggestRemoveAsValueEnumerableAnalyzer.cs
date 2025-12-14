using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NetFabric.Hyperlinq.Analyzer;

/// <summary>
/// Analyzer that suggests removing AsValueEnumerable() when chaining is not needed.
/// Only triggers when using NetFabric.Hyperlinq is present.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class SuggestRemoveAsValueEnumerableAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "NFHYPERLINQ003";
    const string Category = "Performance";

    static readonly LocalizableString Title = "Remove AsValueEnumerable for better performance";
    static readonly LocalizableString MessageFormat = "Remove AsValueEnumerable() on '{0}' - direct extension methods provide better performance for non-chained operations";
    static readonly LocalizableString Description = "When chaining is not needed, direct array/List extension methods (ref struct) provide better performance than AsValueEnumerable() (chainable).";

    static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    }

    static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;

        // Check if this is a call to AsValueEnumerable
        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
        {
            return;
        }

        if (memberAccess.Name.Identifier.Text != "AsValueEnumerable")
        {
            return;
        }

        // Get the symbol info to confirm it's our AsValueEnumerable
        var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken);
        if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
        {
            return;
        }

        if (methodSymbol.ContainingType?.ToString() != "NetFabric.Hyperlinq.AsValueEnumerableExtensions")
        {
            return;
        }

        // Check if using NetFabric.Hyperlinq is present
        if (!HasHyperlinkUsing(context))
        {
            return;
        }

        // Get the receiver type (what AsValueEnumerable is being called on)
        var receiverType = context.SemanticModel.GetTypeInfo(memberAccess.Expression, context.CancellationToken).Type;
        if (receiverType == null)
        {
            return;
        }

        // Only suggest removal for arrays and List<T>
        if (!IsOptimizableType(receiverType))
        {
            return;
        }

        // Check if chaining is actually needed
        if (IsChainingNeeded(invocation))
        {
            return;
        }

        // Suggest removing AsValueEnumerable
        var diagnostic = Diagnostic.Create(
            Rule,
            invocation.GetLocation(),
            receiverType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));

        context.ReportDiagnostic(diagnostic);
    }

    static bool HasHyperlinkUsing(SyntaxNodeAnalysisContext context)
    {
        var root = context.Node.SyntaxTree.GetRoot(context.CancellationToken);
        if (root is not CompilationUnitSyntax compilationUnit)
        {
            return false;
        }

        return compilationUnit.Usings.Any(u =>
            u.Name?.ToString() == "NetFabric.Hyperlinq");
    }

    static bool IsOptimizableType(ITypeSymbol type)
    {
        // Check for List<T>
        if (type is INamedTypeSymbol namedType)
        {
            if (namedType.OriginalDefinition.ToString() == "System.Collections.Generic.List<T>")
            {
                return true;
            }
        }

        // Check for T[]
        if (type is IArrayTypeSymbol)
        {
            return true;
        }

        return false;
    }

    static bool IsChainingNeeded(InvocationExpressionSyntax invocation)
    {
        // Check if there's a method call after AsValueEnumerable
        if (invocation.Parent is not MemberAccessExpressionSyntax parentMemberAccess ||
            parentMemberAccess.Expression != invocation)
        {
            return false;
        }

        // Get the method being called after AsValueEnumerable
        var methodName = parentMemberAccess.Name.Identifier.Text;

        // If it's another LINQ operation, check if there's more chaining after it
        if (IsLinqMethod(methodName))
        {
            // Look for the invocation of this method
            if (parentMemberAccess.Parent is not InvocationExpressionSyntax parentInvocation)
            {
                return false;
            }

            // Check if there's another method call after this one
            if (parentInvocation.Parent is MemberAccessExpressionSyntax nextMemberAccess &&
                nextMemberAccess.Expression == parentInvocation)
            {
                // There's chaining - AsValueEnumerable is needed
                return true;
            }

            // No chaining after this method - AsValueEnumerable not needed
            return false;
        }

        // Not a LINQ method, so no chaining
        return false;
    }

    static bool IsLinqMethod(string methodName) => methodName is "Where" or "Select" or "Any" or "Count" or "First" or "Single" or "Sum" or
                             "FirstOrDefault" or "FirstOrNone" or "SingleOrDefault" or "SingleOrNone" or
                             "Last" or "ToArray" or "ToList";
}
