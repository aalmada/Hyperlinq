using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace NetFabric.Hyperlinq.Analyzer
{
    /// <summary>
    /// Analyzer that suggests using AsValueEnumerable() when chaining is attempted on ref struct enumerables.
    /// Only triggers when using NetFabric.Hyperlinq is present.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SuggestAsValueEnumerableAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "NFHYPERLINQ004";
        private const string Category = "Usage";

        private static readonly LocalizableString Title = "Use AsValueEnumerable to enable chaining";
        private static readonly LocalizableString MessageFormat = "Use AsValueEnumerable() before '{0}' to enable chaining operations";
        private static readonly LocalizableString Description = "Ref struct enumerables cannot be chained. Use AsValueEnumerable() to get a chainable enumerable.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Error,
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

            // Check if this is a LINQ method call
            if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
                return;

            var methodName = memberAccess.Name.Identifier.Text;

            // Check if it's a LINQ method we care about
            if (!IsLinqMethod(methodName))
                return;

            // Check if using NetFabric.Hyperlinq is present
            if (!HasHyperlinkUsing(context))
                return;

            // Get the symbol info
            var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken);
            if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
                return;

            // Check if the method is from NetFabric.Hyperlinq (direct extension on array/List)
            var containingType = methodSymbol.ContainingType?.ToString();
            if (containingType != "NetFabric.Hyperlinq.ArrayValueEnumerableExtensions" &&
                containingType != "NetFabric.Hyperlinq.ListValueEnumerableExtensions")
                return;

            // Get the receiver expression
            var receiverExpression = memberAccess.Expression;

            // Check if the receiver is a direct array or List (not already wrapped)
            var receiverType = context.SemanticModel.GetTypeInfo(receiverExpression, context.CancellationToken).Type;
            if (receiverType == null)
                return;

            // Only trigger for direct arrays and Lists
            if (!IsDirectArrayOrList(receiverType))
                return;

            // Check if there's chaining after this method call
            if (!IsChainingAttempted(invocation))
                return;

            // Suggest using AsValueEnumerable
            var diagnostic = Diagnostic.Create(
                Rule,
                receiverExpression.GetLocation(),
                methodName);

            context.ReportDiagnostic(diagnostic);
        }

        private static bool HasHyperlinkUsing(SyntaxNodeAnalysisContext context)
        {
            var root = context.Node.SyntaxTree.GetRoot(context.CancellationToken);
            var compilationUnit = root as CompilationUnitSyntax;
            if (compilationUnit == null)
                return false;

            return compilationUnit.Usings.Any(u =>
                u.Name?.ToString() == "NetFabric.Hyperlinq");
        }

        private static bool IsDirectArrayOrList(ITypeSymbol type)
        {
            // Check for List<T>
            if (type is INamedTypeSymbol namedType)
            {
                if (namedType.OriginalDefinition.ToString() == "System.Collections.Generic.List<T>")
                    return true;
            }

            // Check for T[]
            if (type is IArrayTypeSymbol)
                return true;

            return false;
        }

        private static bool IsChainingAttempted(InvocationExpressionSyntax invocation)
        {
            // Check if there's another method call after this invocation
            if (invocation.Parent is MemberAccessExpressionSyntax parentMemberAccess &&
                parentMemberAccess.Expression == invocation)
            {
                // There's a method being called on the result - chaining is attempted
                return true;
            }

            // Check if the result is being assigned to IEnumerable<T>
            if (invocation.Parent is EqualsValueClauseSyntax equalsValue)
            {
                if (equalsValue.Parent is VariableDeclaratorSyntax declarator &&
                    declarator.Parent is VariableDeclarationSyntax declaration)
                {
                    var typeInfo = declaration.Type;
                    if (typeInfo != null)
                    {
                        var typeString = typeInfo.ToString();
                        if (typeString.StartsWith("IEnumerable<"))
                            return true;
                    }
                }
            }

            return false;
        }

        private static bool IsLinqMethod(string methodName)
        {
            return methodName is "Where" or "Select";
        }
    }
}
