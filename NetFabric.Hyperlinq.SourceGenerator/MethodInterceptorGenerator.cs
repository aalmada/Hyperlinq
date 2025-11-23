using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq.SourceGenerator
{
    [Generator]
    public class MethodInterceptorGenerator : IIncrementalGenerator
    {
        private static readonly List<IMethodGenerator> methodGenerators = new()
        {
            new AnyMethodGenerator(),
            new CountMethodGenerator(),
            new FirstMethodGenerator(),
            new SingleMethodGenerator(),
            new SelectMethodGenerator(),
            new WhereMethodGenerator(),
        };

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

            foreach (var generator in methodGenerators)
            {
                if (generator.MethodName == methodSymbol.Name)
                {
                    var info = generator.TryGetInvocation(context, memberAccess, methodSymbol);
                    if (info is not null)
                        return info;
                }
            }

            return null;
        }

        public static bool ImplementsICollection(ITypeSymbol typeSymbol)
        {
            if (typeSymbol.OriginalDefinition.ToDisplayString() == "System.Collections.Generic.ICollection<T>")
                return true;

            foreach (var iface in typeSymbol.AllInterfaces)
            {
                if (iface.OriginalDefinition.ToDisplayString() == "System.Collections.Generic.ICollection<T>")
                    return true;
            }
            return false;
        }

        private void Execute(SourceProductionContext context, ImmutableArray<InvocationInfo?> invocations)
        {
            if (invocations.IsDefaultOrEmpty)
                return;

            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Runtime.CompilerServices;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using NetFabric.Hyperlinq;");
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

                var suffix = $"_{line}_{character}";
                sb.AppendLine($"        [InterceptsLocation(@\"{filePath}\", {line}, {character})]");
                
                foreach (var generator in methodGenerators)
                {
                    if (generator.MethodName == invocation.MethodName)
                    {
                        generator.Generate(sb, invocation, suffix);
                        break;
                    }
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
    }

    public record InvocationInfo(InvocationExpressionSyntax Syntax, IMethodSymbol Symbol, string MethodName, string ContainingType, bool IsCollection)
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
