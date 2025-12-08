using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace NetFabric.Hyperlinq.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AddAsValueEnumerableAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "NFHYPERLINQ001";
        private const string Category = "Performance";

        private static readonly LocalizableString Title = "Use AsValueEnumerable for better performance";
        private static readonly LocalizableString MessageFormat = "Consider using AsValueEnumerable() on '{0}' for better performance with value-type enumeration";
        private static readonly LocalizableString Description = "Using AsValueEnumerable() enables value-type enumeration which avoids boxing and improves performance.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
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

            // Get the symbol info
            var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken);
            if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
                return;

            // Check if it's from System.Linq.Enumerable
            if (methodSymbol.ContainingType?.ToString() != "System.Linq.Enumerable")
                return;

            // (If it is present, user can choose between direct extensions or AsValueEnumerable)
            // UPDATE: We now suggest it even if present, to encourage performance.
            // if (HasHyperlinkUsing(context))
            //    return;

            // Get the receiver type
            var receiverType = context.SemanticModel.GetTypeInfo(memberAccess.Expression, context.CancellationToken).Type;
            if (receiverType == null)
                return;

            // Check if the receiver is List<T> or T[]
            if (IsOptimizableType(receiverType))
            {
                // Check if AsValueEnumerable is already called
                if (IsAlreadyValueEnumerable(memberAccess.Expression))
                    return;

                var diagnostic = Diagnostic.Create(
                    Rule,
                    memberAccess.Expression.GetLocation(),
                    receiverType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));

                context.ReportDiagnostic(diagnostic);
            }
        }

        private static bool IsLinqMethod(string methodName)
        {
            return methodName is "Where" or "Select" or "Any" or "Count" or "First" or "Single" or "Sum";
        }

        private static bool IsOptimizableType(ITypeSymbol type)
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

        private static bool IsAlreadyValueEnumerable(ExpressionSyntax expression)
        {
            // Check if the expression is already a call to AsValueEnumerable
            if (expression is InvocationExpressionSyntax invocation &&
                invocation.Expression is MemberAccessExpressionSyntax memberAccess)
            {
                return memberAccess.Name.Identifier.Text == "AsValueEnumerable";
            }

            return false;
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
    }
}
