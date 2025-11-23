using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace NetFabric.Hyperlinq.SourceGenerator
{
    interface IMethodGenerator
    {
        string MethodName { get; }
        InvocationInfo? TryGetInvocation(GeneratorSyntaxContext context, MemberAccessExpressionSyntax memberAccess, IMethodSymbol methodSymbol);
        void Generate(StringBuilder sb, InvocationInfo invocation, string suffix);
    }
}
