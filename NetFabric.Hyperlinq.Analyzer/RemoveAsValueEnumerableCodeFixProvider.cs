using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetFabric.Hyperlinq.Analyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RemoveAsValueEnumerableCodeFixProvider)), Shared]
    public class RemoveAsValueEnumerableCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(RemoveAsValueEnumerableAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null)
                return;

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the AsValueEnumerable invocation that triggered the diagnostic
            var invocation = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<InvocationExpressionSyntax>().First();
            if (invocation == null)
                return;

            // Verify this is actually an AsValueEnumerable call
            if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess ||
                memberAccess.Name.Identifier.Text != "AsValueEnumerable")
                return;

            // Register a code action that will invoke the fix
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Remove AsValueEnumerable()",
                    createChangedDocument: c => RemoveAsValueEnumerableAsync(context.Document, invocation, memberAccess, c),
                    equivalenceKey: "RemoveAsValueEnumerable"),
                diagnostic);
        }

        private static async Task<Document> RemoveAsValueEnumerableAsync(
            Document document,
            InvocationExpressionSyntax invocation,
            MemberAccessExpressionSyntax memberAccess,
            CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null)
                return document;

            // Replace the entire AsValueEnumerable() invocation with just its receiver
            // For example: list.AsValueEnumerable().Count() becomes list.Count()
            var receiver = memberAccess.Expression;
            var newRoot = root.ReplaceNode(invocation, receiver.WithTriviaFrom(invocation));

            return document.WithSyntaxRoot(newRoot);
        }
    }
}
