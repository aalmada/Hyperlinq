using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NetFabric.Hyperlinq.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class UseValueDelegateAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "HLQ011";
    const string Category = "Performance";

    static readonly LocalizableString Title = "Use Value Delegate for better performance";
    static readonly LocalizableString MessageFormat = "Consider using a Value Delegate (struct implementing IFunction) instead of a lambda expression for improved performance significantly in 'Where' and 'Select' operations";
    static readonly LocalizableString Description = "Lambda expressions allocate delegates and prevent inlining. Value Delegates (structs implementing IFunction) avoid allocation and allow aggressive inlining.";

    static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Info, // Info severity as it requires significant refactoring
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
    }

    static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;
        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
        {
            return;
        }

        var methodName = memberAccess.Name.Identifier.Text;
        if (methodName is not "Where" and not "Select")
        {
            return;
        }

        // Check if arguments include a lambda expression
        foreach (var arg in invocation.ArgumentList.Arguments)
        {
            if (arg.Expression.IsKind(SyntaxKind.ParenthesizedLambdaExpression) ||
                arg.Expression.IsKind(SyntaxKind.SimpleLambdaExpression))
            {
                // Check if it's a Hyperlinq method (rough heuristic by checking if result is typically Hyperlinq or if 'AsValueEnumerable' was used in chain)
                // Or simply checking if symbol info comes from Hyperlinq assembly
                var symbol = context.SemanticModel.GetSymbolInfo(invocation).Symbol;
                if (symbol is IMethodSymbol methodSymbol &&
                    methodSymbol.ContainingAssembly.Name.Contains("Hyperlinq"))
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                       Rule,
                       arg.Expression.GetLocation()));
                }
                else if (methodName is "Select" or "Where") // Broad heuristic if symbol resolution fails or for generic awareness
                {
                    // Optional: Only trigger if "NetFabric.Hyperlinq" is used in file
                    var root = context.Node.SyntaxTree.GetRoot(); // Expensive to do every time?
                                                                  // check semantic model for references?
                                                                  // For now rely on symbol check primarily.
                                                                  // Falling back to heuristic if using directive is present might be good?
                                                                  // Let's stick to symbol check to avoid false positives on standard LINQ unless intent is to migrate.
                }
            }
        }
    }
}
