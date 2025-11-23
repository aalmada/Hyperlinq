using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace NetFabric.Hyperlinq.SourceGenerator
{
    class FirstMethodGenerator : IMethodGenerator
    {
        public string MethodName => "First";

        public InvocationInfo? TryGetInvocation(GeneratorSyntaxContext context, MemberAccessExpressionSyntax memberAccess, IMethodSymbol methodSymbol)
        {
            if (methodSymbol.Parameters.Length != 1)
                return null;

            return new InvocationInfo(context.Node as InvocationExpressionSyntax, methodSymbol, "First", methodSymbol.ContainingType.ToDisplayString(), false);
        }

        public void Generate(StringBuilder sb, InvocationInfo invocation, string suffix)
        {
            sb.AppendLine($"        public static T First{suffix}<T>(this IEnumerable<T> source)");
            sb.AppendLine("        {");
            sb.AppendLine("            return NetFabric.Hyperlinq.Optimized.First(source);");
            sb.AppendLine("        }");
        }
    }
}
