using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetFabric.Hyperlinq.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AddAsValueEnumerableCodeFixProvider)), Shared]
    public class AddAsValueEnumerableCodeFixProvider : CodeFixProvider
    {
        private const string NetFabricHyperlinkNamespace = "NetFabric.Hyperlinq";

        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AddAsValueEnumerableAnalyzer.DiagnosticId);

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
                    createChangedDocument: c => AddAsValueEnumerableAsync(context.Document, expression, root, c),
                    equivalenceKey: "AddAsValueEnumerable"),
                diagnostic);
        }

        private static async Task<Document> AddAsValueEnumerableAsync(
            Document document,
            ExpressionSyntax expression,
            SyntaxNode root,
            CancellationToken cancellationToken)
        {
            // Create the AsValueEnumerable() invocation
            var asValueEnumerableInvocation = SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    expression,
                    SyntaxFactory.IdentifierName("AsValueEnumerable")));

            // Replace the expression with the new invocation
            var newRoot = root.ReplaceNode(expression, asValueEnumerableInvocation);

            // Check if we need to add the using directive
            if (root is CompilationUnitSyntax compilationUnit)
            {
                var hasUsingDirective = compilationUnit.Usings
                    .Any(u => u.Name?.ToString() == NetFabricHyperlinkNamespace);

                if (!hasUsingDirective)
                {
                    // Add the using directive
                    var usingDirective = SyntaxFactory.UsingDirective(
                        SyntaxFactory.ParseName(NetFabricHyperlinkNamespace))
                        .WithTrailingTrivia(SyntaxFactory.ElasticLineFeed);

                    var newCompilationUnit = ((CompilationUnitSyntax)newRoot).AddUsings(usingDirective);
                    newRoot = newCompilationUnit;
                }
            }

            return document.WithSyntaxRoot(newRoot);
        }
    }
}
