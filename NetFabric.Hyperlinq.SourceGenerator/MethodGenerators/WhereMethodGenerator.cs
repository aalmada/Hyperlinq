using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace NetFabric.Hyperlinq.SourceGenerator
{
    class WhereMethodGenerator : IMethodGenerator
    {
        public string MethodName => "Where";

        public InvocationInfo? TryGetInvocation(GeneratorSyntaxContext context, MemberAccessExpressionSyntax memberAccess, IMethodSymbol methodSymbol)
        {
            if (methodSymbol.Parameters.Length != 2)
                return null;

            return new InvocationInfo(context.Node as InvocationExpressionSyntax, methodSymbol, "Where", methodSymbol.ContainingType.ToDisplayString(), false);
        }

        public void Generate(StringBuilder sb, InvocationInfo invocation, string suffix)
        {
            sb.AppendLine($"        public static IEnumerable<TSource> Where{suffix}<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)");
            sb.AppendLine("        {");
            sb.AppendLine("            return NetFabric.Hyperlinq.Optimized.Where(source, predicate);");
            sb.AppendLine("        }");
        }
    }
}
