using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace NetFabric.Hyperlinq.SourceGenerator
{
    class AnyMethodGenerator : IMethodGenerator
    {
        public string MethodName => "Any";

        public InvocationInfo? TryGetInvocation(GeneratorSyntaxContext context, MemberAccessExpressionSyntax memberAccess, IMethodSymbol methodSymbol)
        {
            if (methodSymbol.Parameters.Length != 1)
                return null;

            var receiverType = context.SemanticModel.GetTypeInfo(memberAccess.Expression).Type;
            var isCollection = receiverType is not null && MethodInterceptorGenerator.ImplementsICollection(receiverType);
            return new InvocationInfo(context.Node as InvocationExpressionSyntax, methodSymbol, "Any", methodSymbol.ContainingType.ToDisplayString(), isCollection);
        }

        public void Generate(StringBuilder sb, InvocationInfo invocation, string suffix)
        {
            if (invocation.IsCollection)
            {
                sb.AppendLine($"        public static bool Any{suffix}<T>(this IEnumerable<T> source)");
                sb.AppendLine("        {");
                sb.AppendLine("            return NetFabric.Hyperlinq.Optimized.Any((ICollection<T>)source);");
                sb.AppendLine("        }");
            }
            else
            {
                sb.AppendLine($"        public static bool Any{suffix}<T>(this IEnumerable<T> source)");
                sb.AppendLine("        {");
                sb.AppendLine("            return NetFabric.Hyperlinq.Optimized.Any(source);");
                sb.AppendLine("        }");
            }
        }
    }
}
