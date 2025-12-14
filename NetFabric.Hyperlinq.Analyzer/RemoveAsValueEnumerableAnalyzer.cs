using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NetFabric.Hyperlinq.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class RemoveAsValueEnumerableAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "NFHYPERLINQ002";
    private const string Category = "Usage";

    private static readonly LocalizableString Title = "Remove unnecessary AsValueEnumerable";
    private static readonly LocalizableString MessageFormat = "AsValueEnumerable() is not needed on '{0}' because {1}";
    private static readonly LocalizableString Description = "AsValueEnumerable() should not be used when the type already has direct extension methods or is already a value enumerable.";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
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

        // Get the receiver type (what AsValueEnumerable is being called on)
        var receiverType = context.SemanticModel.GetTypeInfo(memberAccess.Expression, context.CancellationToken).Type;
        if (receiverType == null)
        {
            return;
        }

        // Check if the receiver is already a value enumerable type
        if (IsValueEnumerableType(receiverType))
        {
            var diagnostic = Diagnostic.Create(
                Rule,
                invocation.GetLocation(),
                receiverType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                "it is already a value enumerable");

            context.ReportDiagnostic(diagnostic);
            return;
        }

        // Check if there's a method call after AsValueEnumerable
        if (invocation.Parent is MemberAccessExpressionSyntax parentMemberAccess &&
            parentMemberAccess.Expression == invocation)
        {
            var methodName = parentMemberAccess.Name.Identifier.Text;

            // Check if this is a method that has direct extension methods for specific types
            if (HasDirectExtensionMethod(receiverType, methodName))
            {
                var diagnostic = Diagnostic.Create(
                    Rule,
                    invocation.GetLocation(),
                    receiverType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                    $"direct extension methods exist for {methodName}()");

                context.ReportDiagnostic(diagnostic);
            }
        }
    }

    private static bool IsValueEnumerableType(ITypeSymbol type)
    {
        var typeName = type.ToString();

        // Check if it's one of our value enumerable wrapper types
        return typeName.StartsWith("NetFabric.Hyperlinq.ListValueEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.ArrayValueEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.EnumerableValueEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.WhereEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.WhereListEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.WhereArrayEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.WhereMemoryEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.SelectEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.SelectListEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.SelectArrayEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.SelectMemoryEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.WhereSelectEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.WhereSelectListEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.WhereSelectArrayEnumerable<") ||
               typeName.StartsWith("NetFabric.Hyperlinq.WhereSelectMemoryEnumerable<");
    }

    private static bool HasDirectExtensionMethod(ITypeSymbol type, string methodName)
    {
        // Methods that have direct extension methods for specific types
        if (methodName is not ("Any" or "Count" or "First" or "Last" or "Sum" or "Single"))
        {
            return false;
        }

        // Types that have direct extension methods
        if (type is IArrayTypeSymbol)
        {
            return true;
        }

        if (type is INamedTypeSymbol namedType)
        {
            var typeString = namedType.OriginalDefinition.ToString();

            // List<T>, Span<T>, ReadOnlySpan<T>, Memory<T>, ReadOnlyMemory<T>
            return typeString is "System.Collections.Generic.List<T>" or
                                 "System.Span<T>" or
                                 "System.ReadOnlySpan<T>" or
                                 "System.Memory<T>" or
                                 "System.ReadOnlyMemory<T>";
        }

        return false;
    }
}
