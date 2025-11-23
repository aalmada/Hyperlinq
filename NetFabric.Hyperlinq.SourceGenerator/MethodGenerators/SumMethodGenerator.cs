using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace NetFabric.Hyperlinq.SourceGenerator
{
    class SumMethodGenerator : IMethodGenerator
    {
        public string MethodName => "Sum";

        public InvocationInfo? TryGetInvocation(GeneratorSyntaxContext context, MemberAccessExpressionSyntax memberAccess, IMethodSymbol methodSymbol)
        {
            // Only intercept parameterless Sum()
            if (methodSymbol.Parameters.Length != 1)
                return null;

            // Check if it's a supported numeric type
            var returnType = methodSymbol.ReturnType.ToDisplayString();
            if (returnType != "int" && returnType != "long" && returnType != "float" && returnType != "double")
                return null;

            return new InvocationInfo(context.Node as InvocationExpressionSyntax, methodSymbol, "Sum", methodSymbol.ContainingType.ToDisplayString(), false);
        }

        public void Generate(StringBuilder sb, InvocationInfo invocation, string suffix)
        {
            var returnType = invocation.Symbol.ReturnType.ToDisplayString();
            
            sb.AppendLine($"        public static {returnType} Sum{suffix}(this IEnumerable<{returnType}> source)");
            sb.AppendLine("        {");
            sb.AppendLine($"            return NetFabric.Hyperlinq.Optimized.Sum(source);");
            sb.AppendLine("        }");
        }
    }
}
