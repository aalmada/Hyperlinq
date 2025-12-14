using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NetFabric.Hyperlinq.Analyzer;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SuggestRemoveAsValueEnumerableCodeFixProvider)), Shared]
public class SuggestRemoveAsValueEnumerableCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(SuggestRemoveAsValueEnumerableAnalyzer.DiagnosticId);

    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null)
        {
            return;
        }

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Find the invocation expression identified by the diagnostic
        var invocation = root.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<InvocationExpressionSyntax>().First();
        if (invocation == null)
        {
            return;
        }

        // Register a code action that will remove the AsValueEnumerable call
        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Remove AsValueEnumerable()",
                createChangedDocument: c => RemoveAsValueEnumerableAsync(context.Document, invocation, c),
                equivalenceKey: nameof(SuggestRemoveAsValueEnumerableCodeFixProvider)),
            diagnostic);
    }

    static async Task<Document> RemoveAsValueEnumerableAsync(
        Document document,
        InvocationExpressionSyntax invocation,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root == null)
        {
            return document;
        }

        // Get the expression that AsValueEnumerable is being called on
        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
        {
            return document;
        }

        var receiver = memberAccess.Expression;

        // Replace the entire invocation with just the receiver
        var newRoot = root.ReplaceNode(invocation, receiver.WithTriviaFrom(invocation));

        return document.WithSyntaxRoot(newRoot);
    }
}
