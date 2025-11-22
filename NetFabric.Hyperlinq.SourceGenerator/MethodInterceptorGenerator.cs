using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;
using System.Linq;

namespace NetFabric.Hyperlinq.SourceGenerator
{
    [Generator]
    public class MethodInterceptorGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var invocations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (s, _) => s is InvocationExpressionSyntax,
                    transform: (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null);

            context.RegisterSourceOutput(invocations.Collect(), Execute);
        }

        private static InvocationInfo? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;
            var semanticModel = context.SemanticModel;

            if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
                return null;

            var methodSymbol = semanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
            if (methodSymbol is null)
                return null;

            if (methodSymbol.IsExtensionMethod && methodSymbol.ReducedFrom is not null)
            {
                methodSymbol = methodSymbol.ReducedFrom;
            }

            if (methodSymbol.ContainingType.ToDisplayString() != "System.Linq.Enumerable")
                return null;

            var methodName = methodSymbol.Name;
            if (methodName == "Any" && methodSymbol.Parameters.Length == 1)
            {
                return new InvocationInfo(invocation, methodSymbol, "Any", methodSymbol.ContainingType.ToDisplayString());
            }
            if (methodName == "First" && methodSymbol.Parameters.Length == 1)
            {
                return new InvocationInfo(invocation, methodSymbol, "First", methodSymbol.ContainingType.ToDisplayString());
            }
            if (methodName == "Single" && methodSymbol.Parameters.Length == 1)
            {
                return new InvocationInfo(invocation, methodSymbol, "Single", methodSymbol.ContainingType.ToDisplayString());
            }
            if (methodName == "Select" && methodSymbol.Parameters.Length == 2)
            {
                return new InvocationInfo(invocation, methodSymbol, "Select", methodSymbol.ContainingType.ToDisplayString());
            }
            if (methodName == "Where" && methodSymbol.Parameters.Length == 2)
            {
                return new InvocationInfo(invocation, methodSymbol, "Where", methodSymbol.ContainingType.ToDisplayString());
            }

            return null;
        }

        private void Execute(SourceProductionContext context, ImmutableArray<InvocationInfo?> invocations)
        {
            if (invocations.IsDefaultOrEmpty)
                return;

            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Runtime.CompilerServices;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine("namespace NetFabric.Hyperlinq.Generated");
            sb.AppendLine("{");
            sb.AppendLine("    public static class Interceptors");
            sb.AppendLine("    {");

            foreach (var invocation in invocations)
            {
                if (invocation is null) continue;

                var location = invocation.GetLocation();
                var filePath = location.SourceTree?.FilePath;
                var lineSpan = location.GetLineSpan();
                var line = lineSpan.StartLinePosition.Line + 1;
                var character = lineSpan.StartLinePosition.Character + 1;

                sb.AppendLine($"        [InterceptsLocation(@\"{filePath}\", {line}, {character})]");
                
                switch (invocation.MethodName)
                {
                    case "Any":
                        sb.AppendLine($"        public static bool Any<T>(this IEnumerable<T> source)");
                        sb.AppendLine("        {");
                        sb.AppendLine("            return NetFabric.Hyperlinq.Optimized.Any(source);");
                        sb.AppendLine("        }");
                        break;
                    case "First":
                        sb.AppendLine($"        public static T First<T>(this IEnumerable<T> source)");
                        sb.AppendLine("        {");
                        sb.AppendLine("            return NetFabric.Hyperlinq.Optimized.First(source);");
                        sb.AppendLine("        }");
                        break;
                    case "Single":
                        sb.AppendLine($"        public static T Single<T>(this IEnumerable<T> source)");
                        sb.AppendLine("        {");
                        sb.AppendLine("            return NetFabric.Hyperlinq.Optimized.Single(source);");
                        sb.AppendLine("        }");
                        break;
                    case "Select":
                        sb.AppendLine($"        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)");
                        sb.AppendLine("        {");
                        sb.AppendLine("            return NetFabric.Hyperlinq.Optimized.Select(source, selector);");
                        sb.AppendLine("        }");
                        break;
                    case "Where":
                        sb.AppendLine($"        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)");
                        sb.AppendLine("        {");
                        sb.AppendLine("            return NetFabric.Hyperlinq.Optimized.Where(source, predicate);");
                        sb.AppendLine("        }");
                        break;
                }
                sb.AppendLine();
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");
            
            // Attribute definition
            sb.AppendLine();
            sb.AppendLine("namespace System.Runtime.CompilerServices");
            sb.AppendLine("{");
            sb.AppendLine("    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]");
            sb.AppendLine("    file sealed class InterceptsLocationAttribute : Attribute");
            sb.AppendLine("    {");
            sb.AppendLine("        public InterceptsLocationAttribute(string filePath, int line, int character)");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            context.AddSource("Interceptors.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        private record InvocationInfo(InvocationExpressionSyntax Syntax, IMethodSymbol Symbol, string MethodName, string ContainingType)
        {
            public Location GetLocation()
            {
                if (Syntax.Expression is MemberAccessExpressionSyntax memberAccess)
                {
                    return memberAccess.Name.GetLocation();
                }
                return Syntax.GetLocation();
            }
        }
    }
}
