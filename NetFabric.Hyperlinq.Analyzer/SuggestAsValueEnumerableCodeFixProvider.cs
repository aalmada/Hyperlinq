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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SuggestAsValueEnumerableCodeFixProvider)), Shared]
public class SuggestAsValueEnumerableCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(SuggestAsValueEnumerableAnalyzer.DiagnosticId);

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

        // Find the expression identified by the diagnostic (the receiver expression)
        var expression = root.FindNode(diagnosticSpan);
        if (expression == null)
        {
            return;
        }

        // Register a code action that will add AsValueEnumerable()
        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Add AsValueEnumerable()",
                createChangedDocument: c => AddAsValueEnumerableAsync(context.Document, expression, c),
                equivalenceKey: nameof(SuggestAsValueEnumerableCodeFixProvider)),
            diagnostic);
    }

    private static async Task<Document> AddAsValueEnumerableAsync(
        Document document,
        SyntaxNode expression,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root == null)
        {
            return document;
        }

        // Create the AsValueEnumerable() call
        var asValueEnumerableCall = SyntaxFactory.InvocationExpression(
            SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                (ExpressionSyntax)expression,
                SyntaxFactory.IdentifierName("AsValueEnumerable")));

        // Replace the expression with the AsValueEnumerable() call
        var newRoot = root.ReplaceNode(expression, asValueEnumerableCall.WithTriviaFrom(expression));

        return document.WithSyntaxRoot(newRoot);
    }
}
