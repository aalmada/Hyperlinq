using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NetFabric.Hyperlinq.InternalAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class FusionRulesAnalyzer : DiagnosticAnalyzer
{
    // HLQInternal011: Property Exposure
    public const string DiagnosticIdProperties = "HLQInternal011";
    private static readonly LocalizableString TitleProperties = "Missing required properties for Fusion";
    private static readonly LocalizableString MessageFormatProperties = "The type '{0}' is missing the required property '{1}'";
    private static readonly LocalizableString DescriptionProperties = "Enumerables involved in Fusion must expose specific properties (Source, Predicate, Selector) to allow optimization.";
    private static readonly DiagnosticDescriptor RuleProperties = new DiagnosticDescriptor(
        DiagnosticIdProperties,
        TitleProperties,
        MessageFormatProperties,
        "Design",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: DescriptionProperties);

    // HLQInternal012: Extensions Class Existence
    public const string DiagnosticIdExtensionsClass = "HLQInternal012";
    private static readonly LocalizableString TitleExtensionsClass = "Missing Extensions class";
    private static readonly LocalizableString MessageFormatExtensionsClass = "The type '{0}' is missing a corresponding Extensions class '{1}'";
    private static readonly LocalizableString DescriptionExtensionsClass = "Enumerables must have a corresponding Extensions class to host Fusion methods.";
    private static readonly DiagnosticDescriptor RuleExtensionsClass = new DiagnosticDescriptor(
        DiagnosticIdExtensionsClass,
        TitleExtensionsClass,
        MessageFormatExtensionsClass,
        "Design",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: DescriptionExtensionsClass);

    // HLQInternal013: Fusion Methods Existence
    public const string DiagnosticIdFusionMethods = "HLQInternal013";
    private static readonly LocalizableString TitleFusionMethods = "Missing Fusion method";
    private static readonly LocalizableString MessageFormatFusionMethods = "The extensions class '{0}' is missing the required Fusion method '{1}'";
    private static readonly LocalizableString DescriptionFusionMethods = "The Extensions class must implement specific Fusion methods (Where, Select, Min, Max) to enable optimizations.";
    private static readonly DiagnosticDescriptor RuleFusionMethods = new DiagnosticDescriptor(
        DiagnosticIdFusionMethods,
        TitleFusionMethods,
        MessageFormatFusionMethods,
        "Design",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: DescriptionFusionMethods);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        RuleProperties,
        RuleExtensionsClass,
        RuleFusionMethods);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
    }

    private void AnalyzeNamedType(SymbolAnalysisContext context)
    {
        var namedType = (INamedTypeSymbol)context.Symbol;

        // Filter: Must be Enumerable struct/class (heuristic: name ends with "Enumerable")
        // The original test uses `t.Name.EndsWith("Enumerable")` and `IsEnumerable`.
        // We will stick to name check + ensuring it's not Abstract/Interface for simplicity & performance, 
        // matching the test's scope.
        if (namedType.IsAbstract || namedType.TypeKind == TypeKind.Interface)
            return;

        if (!namedType.Name.EndsWith("Enumerable"))
            return;

        var name = namedType.Name;
        var isWhere = name.Contains("Where");
        var isSelect = name.Contains("Select");
        var isWhereSelect = name.Contains("WhereSelect");

        if (!isWhere && !isSelect)
            return;

        // --- HLQInternal011: Property Checks ---
        // Always check Source
        CheckProperty(context, namedType, "Source");

        if (isWhere || isWhereSelect)
        {
            CheckProperty(context, namedType, "Predicate");
        }

        if (isSelect || isWhereSelect)
        {
            CheckProperty(context, namedType, "Selector");
        }

        // --- HLQInternal012 & 013: Extensions & Fusion Methods ---
        // Exclude WhereSelect from these checks as per original tests
        if (isWhereSelect)
            return;

        var extensionsName = $"{namedType.Name}Extensions";
        var extensionsType = context.Compilation.GetTypeByMetadataName($"{namedType.ContainingNamespace}.{extensionsName}");
        
        // If not found directly, try looking in the same namespace by name (source context)
        if (extensionsType is null)
        {
            extensionsType = namedType.ContainingNamespace.GetTypeMembers(extensionsName).FirstOrDefault();
        }

        if (extensionsType is null)
        {
            context.ReportDiagnostic(Diagnostic.Create(RuleExtensionsClass, namedType.Locations[0], namedType.Name, extensionsName));
            return;
        }

        if (isWhere) // And not WhereSelect
        {
            CheckMethodExists(context, extensionsType, "Where", 2);
            CheckMethodExists(context, extensionsType, "Min", 1);
            CheckMethodExists(context, extensionsType, "Max", 1);
        }

        if (isSelect) // And not WhereSelect
        {
            CheckMethodExists(context, extensionsType, "Select", 2);
        }
    }

    private static void CheckProperty(SymbolAnalysisContext context, INamedTypeSymbol type, string propertyName)
    {
        var property = type.GetMembers(propertyName).OfType<IPropertySymbol>().FirstOrDefault();
        if (property is null)
        {
             context.ReportDiagnostic(Diagnostic.Create(RuleProperties, type.Locations[0], type.Name, propertyName));
        }
    }

    private static void CheckMethodExists(SymbolAnalysisContext context, INamedTypeSymbol extensionsType, string methodName, int parameterCount)
    {
        var methods = extensionsType.GetMembers(methodName).OfType<IMethodSymbol>();
        var hasMatch = methods.Any(m => 
            m.IsStatic && 
            m.Parameters.Length == parameterCount);

        if (!hasMatch)
        {
             context.ReportDiagnostic(Diagnostic.Create(RuleFusionMethods, extensionsType.Locations[0], extensionsType.Name, methodName));
        }
    }
}
