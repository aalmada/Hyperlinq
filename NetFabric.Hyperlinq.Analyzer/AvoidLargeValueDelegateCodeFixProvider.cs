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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AvoidLargeValueDelegateCodeFixProvider)), Shared]
    public class AvoidLargeValueDelegateCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds => 
            ImmutableArray.Create(AvoidLargeValueDelegateAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root is null) return;

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var token = root.FindToken(diagnosticSpan.Start);
            var declaration = token.Parent?.AncestorsAndSelf().OfType<StructDeclarationSyntax>().FirstOrDefault();
            
            if (declaration is null) return;

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Implement IFunctionIn<T, TResult>",
                    createChangedDocument: c => ImplementIFunctionInAsync(context.Document, declaration, c),
                    equivalenceKey: nameof(AvoidLargeValueDelegateCodeFixProvider)),
                diagnostic);
        }

        private async Task<Document> ImplementIFunctionInAsync(Document document, StructDeclarationSyntax structDeclaration, CancellationToken cancellationToken)
        {
            // 1. Add Interface to Base List (using manual syntax to ensure correct format)
            var ifunctionBase = structDeclaration.BaseList?.Types.FirstOrDefault(t => 
                t.Type is GenericNameSyntax gn && gn.Identifier.Text == "IFunction");
            
            TypeSyntax newInterfaceType;
            if (ifunctionBase?.Type is GenericNameSyntax genericName)
            {
                 newInterfaceType = SyntaxFactory.GenericName("IFunctionIn")
                    .WithTypeArgumentList(genericName.TypeArgumentList);
            }
            else 
            {
                // Fallback: try to infer generics or just abort if structure is unexpected
                return document; 
            }

            var newBaseList = structDeclaration.BaseList!.AddTypes(SyntaxFactory.SimpleBaseType(newInterfaceType));
            
            // 2. Add Invoke(in T) method
            // Find existing Invoke(T)
            var invokeMethod = structDeclaration.Members.OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(md => md.Identifier.Text == "Invoke" && md.ParameterList.Parameters.Count == 1 
                && !md.ParameterList.Parameters[0].Modifiers.Any(mod => mod.IsKind(SyntaxKind.InKeyword)));

            if (invokeMethod is null) return document;

            // Use existing node as template (it's immutable, so we can modify it to create a NEW node)
            var newInvoke = invokeMethod;
            
            // Update parameter to 'in'
            var param = newInvoke.ParameterList.Parameters[0];
            var newParam = param.AddModifiers(SyntaxFactory.Token(SyntaxKind.InKeyword));
            var newParamList = newInvoke.ParameterList.ReplaceNode(param, newParam);
            
            newInvoke = newInvoke.WithParameterList(newParamList);
            
            // Add method to members
            var newMembers = structDeclaration.Members.Add(newInvoke);
            
            var newStruct = structDeclaration
                .WithBaseList(newBaseList)
                .WithMembers(newMembers);

            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root is null) return document;

            var newRoot = root.ReplaceNode(structDeclaration, newStruct);

            return document.WithSyntaxRoot(newRoot)!;
        }
    }
}
