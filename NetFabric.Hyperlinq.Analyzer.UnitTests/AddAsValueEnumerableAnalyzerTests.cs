using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using NetFabric.Hyperlinq.Analyzer.UnitTests.Verifiers;
using TUnit.Core;

namespace NetFabric.Hyperlinq.Analyzer.UnitTests;

public class AddAsValueEnumerableAnalyzerTests : CodeFixVerifier
{
    protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        => new AddAsValueEnumerableAnalyzer();

    protected override Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider GetCSharpCodeFixProvider()
        => new AddAsValueEnumerableCodeFixProvider();

    [Test]
    [Arguments("TestData/NFHYPERLINQ001/NoDiagnostic/Optimized.cs")]
    public async Task Verify_NoDiagnostics(string path) => await VerifyCSharpDiagnostic(File.ReadAllText(path));

    [Test]
    // List<int> is printed as simply List<int> by ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
    // because we have using System.Collections.Generic;
    [Arguments("TestData/NFHYPERLINQ001/Diagnostic/List.cs", "List<int>", "TestData/NFHYPERLINQ001/Diagnostic/List.Fix.cs", 11, 17)]
    // int[] is printed as int[]
    [Arguments("TestData/NFHYPERLINQ001/Diagnostic/Array.cs", "int[]", "TestData/NFHYPERLINQ001/Diagnostic/Array.Fix.cs", 10, 17)]
    public async Task Verify_Diagnostics(string path, string typeName, string fixPath, int line, int column)
    {
        var expected = new DiagnosticResult("NFHYPERLINQ001", DiagnosticSeverity.Info)
            .WithMessage($"Consider using AsValueEnumerable() on '{typeName}' for better performance with value-type enumeration")
            .WithLocation("/0/Test0.cs", line, column);

        await VerifyCSharpDiagnostic(File.ReadAllText(path), expected);

        await VerifyCSharpFix(File.ReadAllText(path), File.ReadAllText(fixPath), expected);
    }
}
