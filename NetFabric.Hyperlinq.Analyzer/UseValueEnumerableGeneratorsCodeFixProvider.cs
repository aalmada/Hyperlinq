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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(UseValueEnumerableGeneratorsCodeFixProvider)), Shared]
public class UseValueEnumerableGeneratorsCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(UseValueEnumerableGeneratorsAnalyzer.DiagnosticId);

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

        // Find the member access expression (Enumerable.Range or Enumerable.Repeat)
        if (root.FindNode(diagnosticSpan) is not MemberAccessExpressionSyntax memberAccess)
        {
            return;
        }

        var methodName = memberAccess.Name.Identifier.Text;

        // Register a code action that will replace Enumerable with ValueEnumerable
        context.RegisterCodeFix(
            CodeAction.Create(
                title: $"Use ValueEnumerable.{methodName}",
                createChangedDocument: c => ReplaceWithValueEnumerableAsync(context.Document, memberAccess, c),
                equivalenceKey: nameof(UseValueEnumerableGeneratorsCodeFixProvider)),
            diagnostic);
    }

    static async Task<Document> ReplaceWithValueEnumerableAsync(
        Document document,
        MemberAccessExpressionSyntax memberAccess,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root == null)
        {
            return document;
        }

        // Create the new member access with ValueEnumerable instead of Enumerable
        var newMemberAccess = memberAccess.WithExpression(
            SyntaxFactory.IdentifierName("ValueEnumerable"));

        // Replace the old member access with the new one
        var newRoot = root.ReplaceNode(memberAccess, newMemberAccess);

        // Check if using NetFabric.Hyperlinq already exists
        if (newRoot is CompilationUnitSyntax compilationUnit)
        {
            var hasHyperlinkUsing = compilationUnit.Usings.Any(u =>
                u.Name?.ToString() == "NetFabric.Hyperlinq");

            if (!hasHyperlinkUsing)
            {
                // Add the using directive
                var usingDirective = SyntaxFactory.UsingDirective(
                    SyntaxFactory.QualifiedName(
                        SyntaxFactory.IdentifierName("NetFabric"),
                        SyntaxFactory.IdentifierName("Hyperlinq")))
                    .WithTrailingTrivia(SyntaxFactory.ElasticLineFeed);

                // Add it after System.Linq if it exists, otherwise at the end
                var systemLinqUsing = compilationUnit.Usings.FirstOrDefault(u =>
                    u.Name?.ToString() == "System.Linq");

                if (systemLinqUsing != null)
                {
                    var index = compilationUnit.Usings.IndexOf(systemLinqUsing);
                    newRoot = compilationUnit.WithUsings(
                        compilationUnit.Usings.Insert(index + 1, usingDirective));
                }
                else
                {
                    newRoot = compilationUnit.AddUsings(usingDirective);
                }
            }
        }

        return document.WithSyntaxRoot(newRoot);
    }
}
