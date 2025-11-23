using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace NetFabric.Hyperlinq.SourceGenerator
{
    class SelectMethodGenerator : IMethodGenerator
    {
        public string MethodName => "Select";

        public InvocationInfo? TryGetInvocation(GeneratorSyntaxContext context, MemberAccessExpressionSyntax memberAccess, IMethodSymbol methodSymbol)
        {
            if (methodSymbol.Parameters.Length != 2)
                return null;

            return new InvocationInfo(context.Node as InvocationExpressionSyntax, methodSymbol, "Select", methodSymbol.ContainingType.ToDisplayString(), false);
        }

        public void Generate(StringBuilder sb, InvocationInfo invocation, string suffix)
        {
            sb.AppendLine($"        public static IEnumerable<TResult> Select{suffix}<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)");
            sb.AppendLine("        {");
            sb.AppendLine("            return NetFabric.Hyperlinq.Optimized.Select(source, selector);");
            sb.AppendLine("        }");
        }
    }
}
