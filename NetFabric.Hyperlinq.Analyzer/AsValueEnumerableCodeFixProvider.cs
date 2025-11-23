using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetFabric.Hyperlinq.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AsValueEnumerableCodeFixProvider)), Shared]
    public class AsValueEnumerableCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AsValueEnumerableAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null)
                return;

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the expression that triggered the diagnostic
            var expression = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<ExpressionSyntax>().First();
            if (expression == null)
                return;

            // Register a code action that will invoke the fix
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Add AsValueEnumerable()",
                    createChangedDocument: c => AddAsValueEnumerableAsync(context.Document, expression, c),
                    equivalenceKey: "AddAsValueEnumerable"),
                diagnostic);
        }

        private static async Task<Document> AddAsValueEnumerableAsync(Document document, ExpressionSyntax expression, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null)
                return document;

            // Create the AsValueEnumerable() invocation
            var asValueEnumerableInvocation = SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    expression,
                    SyntaxFactory.IdentifierName("AsValueEnumerable")));

            // Replace the expression with the new invocation
            var newRoot = root.ReplaceNode(expression, asValueEnumerableInvocation);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}
