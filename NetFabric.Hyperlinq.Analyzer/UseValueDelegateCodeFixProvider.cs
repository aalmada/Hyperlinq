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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(UseValueDelegateCodeFixProvider)), Shared]
public class UseValueDelegateCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(UseValueDelegateAnalyzer.DiagnosticId);

    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return;
        }

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        var node = root.FindNode(diagnosticSpan);

        // Only Refine for simple lambdas (Parenthesized or Simple)
        if (node is not LambdaExpressionSyntax lambda)
        {
            return;
        }

        // Check if captures exist (simplified check: if it refers to variables outside scope)
        // For MVP: We only fix if it can be static (no method/property access of 'this' or locals)
        var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
        if (semanticModel is null)
        {
            return;
        }

        var dataFlow = semanticModel.AnalyzeDataFlow(lambda);
        if (dataFlow is null)
        {
            return;
        }

        if (dataFlow.CapturedInside.Any() || dataFlow.CapturedOutside.Any())
        {
            // If captures exist, refactoring is complex (need to create fields). 
            // We skip automatic fix for now or provide a "Skeleton Only" fix? 
            // Let's stick to Safe Fix only.
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Convert to Value Delegate (IFunction)",
                createChangedDocument: c => ConvertToValueDelegateAsync(context.Document, lambda, c),
                equivalenceKey: nameof(UseValueDelegateCodeFixProvider)),
            diagnostic);
    }

    async Task<Document> ConvertToValueDelegateAsync(Document document, LambdaExpressionSyntax lambda, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return document;
        }

        var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
        if (semanticModel is null)
        {
            return document;
        }

        // 1. Identify Input/Output types
        if (semanticModel.GetSymbolInfo(lambda, cancellationToken).Symbol is not IMethodSymbol lambdaSymbol)
        {
            return document;
        }

        var inputType = lambdaSymbol.Parameters.First().Type; // Assume single parameter for Select/Where
        var returnType = lambdaSymbol.ReturnType;

        // 2. Generate Struct Name
        var structName = "Function" + inputType.Name; // Basic naming strategy
        // Ensure uniqueness? 
        // Better: "MyFunction" and let user rename, or hash based?
        structName = "GeneratedFunction"; // Generic

        // 3. Create Struct Declaration
        // struct GeneratedFunction : IFunction<T, TResult>

        var invokeMethod = SyntaxFactory.MethodDeclaration(
            SyntaxFactory.ParseTypeName(returnType.ToDisplayString()),
            "Invoke")
            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithParameterList(SyntaxFactory.ParameterList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier(lambdaSymbol.Parameters[0].Name))
                    .WithType(SyntaxFactory.ParseTypeName(inputType.ToDisplayString()))
                 )))
            .WithBody(lambda.Body as BlockSyntax)
            .WithExpressionBody(lambda.ExpressionBody != null ? SyntaxFactory.ArrowExpressionClause(lambda.ExpressionBody) : null)
            .WithSemicolonToken(lambda.ExpressionBody != null ? SyntaxFactory.Token(SyntaxKind.SemicolonToken) : default);

        // If lambda was block body, use it. If expression body, use it.

        var baseType = SyntaxFactory.SimpleBaseType(
            SyntaxFactory.ParseTypeName($"NetFabric.Hyperlinq.IFunction<{inputType.ToDisplayString()}, {returnType.ToDisplayString()}>"));

        var structDecl = SyntaxFactory.StructDeclaration(structName)
            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword), SyntaxFactory.Token(SyntaxKind.FileKeyword))) // generic 'internal' or 'file local' in C# 11
            .WithBaseList(SyntaxFactory.BaseList(SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(baseType)))
            .WithMembers(SyntaxFactory.SingletonList<MemberDeclarationSyntax>(invokeMethod));

        // 4. Insert Struct
        // We need to place it in the containing type or namespace. 
        // Safest: Inside the class invoking it? Or adjacent?
        // "File" scoped types are great here if C# 11+. Assuming standard 2.0 analyzer? 
        // Analyzer targets netstandard2.0 but consuming project is NET 8/9/10?
        // Let's use 'private' and put inside containing class.

        var containingType = lambda.FirstAncestorOrSelf<TypeDeclarationSyntax>();
        if (containingType == null)
        {
            return document; // Top level statements?
        }

        // Add struct to containing type members
        structDecl = structDecl.WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword), SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword)));

        var newContainingType = containingType.AddMembers(structDecl);
        var rootWithStruct = root.ReplaceNode(containingType, newContainingType);

        // 5. Replace Lambda with 'new GeneratedFunction()'
        // We need to find the lambda in the NEW root (since we modified the type).
        // Actually, ReplaceNode works on Original nodes if done right?
        // No, we replaced containingType. The lambda is inside it. 
        // We must track the lambda to the new tree.

        // Alternative strategy: Replace lambda FIRST, then insert struct.
        var newExpression = SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(structName))
            .WithArgumentList(SyntaxFactory.ArgumentList());

        root = root.ReplaceNode(lambda, newExpression);

        // NOW insert struct into the containing type in the NEW root.
        var newRootContainingType = root.DescendantNodes().OfType<TypeDeclarationSyntax>()
            .First(t => t.Identifier.Text == containingType.Identifier.Text); // Heuristic match by name

        var finalContainingType = newRootContainingType.AddMembers(structDecl);
        var finalRoot = root.ReplaceNode(newRootContainingType, finalContainingType);

        return document.WithSyntaxRoot(finalRoot);
    }
}
