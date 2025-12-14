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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AvoidSmallValueDelegateInCodeFixProvider)), Shared]
public class AvoidSmallValueDelegateInCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(AvoidSmallValueDelegateInAnalyzer.DiagnosticId);

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
        var token = root.FindToken(diagnosticSpan.Start);
        var declaration = token.Parent?.AncestorsAndSelf().OfType<StructDeclarationSyntax>().FirstOrDefault();

        if (declaration == null)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Implement IFunction<T, TResult>",
                createChangedDocument: c => ImplementIFunctionAsync(context.Document, declaration, c),
                equivalenceKey: nameof(AvoidSmallValueDelegateInCodeFixProvider)),
            diagnostic);
    }

    async Task<Document> ImplementIFunctionAsync(Document document, StructDeclarationSyntax structDeclaration, CancellationToken cancellationToken)
    {
        // Similar logic to Large delegate fixer, but adding IFunction and Invoke(T)

        // 1. Add Interface IFunction
        var ifunctionInBase = structDeclaration.BaseList?.Types.FirstOrDefault(t =>
            t.Type is GenericNameSyntax gn && gn.Identifier.Text == "IFunctionIn");

        TypeSyntax newInterfaceType;
        if (ifunctionInBase?.Type is GenericNameSyntax genericName)
        {
            newInterfaceType = SyntaxFactory.GenericName("IFunction")
               .WithTypeArgumentList(genericName.TypeArgumentList);
        }
        else
        {
            return document;
        }

        var newBaseList = structDeclaration.BaseList!.AddTypes(SyntaxFactory.SimpleBaseType(newInterfaceType));

        // 2. Add Invoke(T) method (by cloning Invoke(in T) and removing 'in')
        var invokeInMethod = structDeclaration.Members.OfType<MethodDeclarationSyntax>()
            .FirstOrDefault(md => md.Identifier.Text == "Invoke" && md.ParameterList.Parameters.Count == 1
            && md.ParameterList.Parameters[0].Modifiers.Any(mod => mod.IsKind(SyntaxKind.InKeyword)));

        if (invokeInMethod is null)
        {
            return document;
        }

        var newInvoke = invokeInMethod;

        // Remove 'in' modifier
        var param = newInvoke.ParameterList.Parameters[0];
        var modifiers = param.Modifiers.Where(m => !m.IsKind(SyntaxKind.InKeyword));
        var newParam = param.WithModifiers(SyntaxFactory.TokenList(modifiers));
        var newParamList = newInvoke.ParameterList.ReplaceNode(param, newParam);

        newInvoke = newInvoke.WithParameterList(newParamList);

        // Add to members
        var newMembers = structDeclaration.Members.Add(newInvoke);

        var newStruct = structDeclaration
            .WithBaseList(newBaseList)
            .WithMembers(newMembers);

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document; // specific null check
        }

        var newRoot = root.ReplaceNode(structDeclaration, newStruct);

        return document.WithSyntaxRoot(newRoot)!;
    }
}
