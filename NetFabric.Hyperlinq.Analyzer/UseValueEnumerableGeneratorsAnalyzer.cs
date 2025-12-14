using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NetFabric.Hyperlinq.Analyzer;

/// <summary>
/// Analyzer that suggests using ValueEnumerable.Range() and ValueEnumerable.Repeat() 
/// instead of Enumerable.Range() and Enumerable.Repeat() when using NetFabric.Hyperlinq.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class UseValueEnumerableGeneratorsAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "NFHYPERLINQ005";
    const string Category = "Performance";

    static readonly LocalizableString Title = "Use ValueEnumerable generator methods";
    static readonly LocalizableString MessageFormat = "Use 'ValueEnumerable.{0}' instead of 'Enumerable.{0}' for better performance";
    static readonly LocalizableString Description = "ValueEnumerable.Range(), ValueEnumerable.Repeat(), ValueEnumerable.Return(), and ValueEnumerable.Empty() provide better performance than their LINQ counterparts.";

    static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Info,
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

        // Check if this is a member access (e.g., Enumerable.Range)
        if (invocation.Expression is not MemberAccessExpressionSyntax memberAccess)
        {
            return;
        }

        var methodName = memberAccess.Name.Identifier.Text;

        // Check if it's Range, Repeat, Return or Empty
        if (methodName is not "Range" and not "Repeat" and not "Return" and not "Empty")
        {
            return;
        }

        // Get the symbol info
        var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken);
        if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
        {
            return;
        }

        // Check if the method is from System.Linq.Enumerable or System.Linq.EnumerableEx
        var containingType = methodSymbol.ContainingType?.ToString();
        if (containingType == "System.Linq.Enumerable")
        {
            if (methodName == "Return") // Return is not in Enumerable
            {
                return;
            }
        }
        else if (containingType == "System.Linq.EnumerableEx")
        {
            if (methodName != "Return") // Only interested in Return from EnumerableEx
            {
                return;
            }
        }
        else
        {
            return;
        }

        // Verify it's the static method we're looking for
        if (!methodSymbol.IsStatic)
        {
            return;
        }

        // Report diagnostic
        var diagnostic = Diagnostic.Create(
            Rule,
            memberAccess.GetLocation(),
            methodName);

        context.ReportDiagnostic(diagnostic);
    }
}
