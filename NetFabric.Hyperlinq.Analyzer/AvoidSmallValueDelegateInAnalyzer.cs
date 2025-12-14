using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NetFabric.Hyperlinq.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class AvoidSmallValueDelegateInAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "HLQ012";
    private const string Category = "Performance";

    private static readonly LocalizableString Title = "Use IFunction for small value delegates";
    private static readonly LocalizableString MessageFormat = "Struct '{0}' is {1} bytes. Consider implementing IFunction<T, TResult> (pass-by-value) instead of IFunctionIn<T, TResult> to avoid indirection overhead.";
    private static readonly LocalizableString Description = "Small value delegates (<= 64 bytes) perform better when passed by value (IFunction), avoiding pointer indirection.";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeStructDeclaration, SyntaxKind.StructDeclaration);
    }

    private static void AnalyzeStructDeclaration(SyntaxNodeAnalysisContext context)
    {
        var structDeclaration = (StructDeclarationSyntax)context.Node;

        // Heuristic: If it implements IFunctionIn but NOT IFunction (and is small)
        // Or if it implements BOTH, we might not need to warn, or prefer IFunction usage?
        // If it implements ONLY IFunctionIn, and is small, suggest adding IFunction?

        var symbol = context.SemanticModel.GetDeclaredSymbol(structDeclaration, context.CancellationToken);
        if (symbol == null)
        {
            return;
        }

        var ifunctionInInterface = symbol.AllInterfaces.FirstOrDefault(i => i.Name == "IFunctionIn" && i.ContainingNamespace.Name == "Hyperlinq");
        if (ifunctionInInterface == null)
        {
            return;
        }

        // Check if ALREADY implements IFunction
        var ifunctionInterface = symbol.AllInterfaces.FirstOrDefault(i => i.Name == "IFunction" && i.ContainingNamespace.Name == "Hyperlinq");
        if (ifunctionInterface != null)
        {
            return;
        }

        // Estimate Size
        var size = EstimateSize(symbol);
        if (size <= 64) // Threshold (same as Large analyzer)
        {
            var diagnostic = Diagnostic.Create(
               Rule,
               structDeclaration.Identifier.GetLocation(),
               symbol.Name,
               size);

            context.ReportDiagnostic(diagnostic);
        }
    }

    private static int EstimateSize(ITypeSymbol type)
    {
        // Reusing size estimation logic (should be shared in a helper class ideally)
        switch (type.SpecialType)
        {
            case SpecialType.System_Boolean: return 1;
            case SpecialType.System_Byte: return 1;
            case SpecialType.System_SByte: return 1;
            case SpecialType.System_Int16: return 2;
            case SpecialType.System_UInt16: return 2;
            case SpecialType.System_Char: return 2;
            case SpecialType.System_Int32: return 4;
            case SpecialType.System_UInt32: return 4;
            case SpecialType.System_Single: return 4;
            case SpecialType.System_Int64: return 8;
            case SpecialType.System_UInt64: return 8;
            case SpecialType.System_Double: return 8;
            case SpecialType.System_Decimal: return 16;
            case SpecialType.System_String: return 8;
            case SpecialType.System_Object: return 8;
            case SpecialType.None:
                if (type.IsReferenceType)
                {
                    return 8;
                }

                if (type.TypeKind == TypeKind.Enum && type is INamedTypeSymbol enumType)
                {
                    return EstimateSize(enumType.EnumUnderlyingType ?? type);
                }

                if (type.TypeKind == TypeKind.Struct && type is INamedTypeSymbol structSymbol)
                {
                    var sum = 0;
                    foreach (var member in structSymbol.GetMembers().OfType<IFieldSymbol>())
                    {
                        if (!member.IsStatic && !member.IsConst)
                        {
                            sum += EstimateSize(member.Type);
                        }
                    }
                    return sum == 0 ? 1 : sum;
                }
                return 8;
        }
        return 8;
    }
}
