using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using NetFabric.Hyperlinq.Analyzer.UnitTests.Verifiers;
using TUnit.Core;

namespace NetFabric.Hyperlinq.Analyzer.UnitTests;

public class RemoveAsValueEnumerableAnalyzerTests : CodeFixVerifier
{
    protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        => new RemoveAsValueEnumerableAnalyzer();

    protected override CodeFixProvider GetCSharpCodeFixProvider()
        => new RemoveAsValueEnumerableCodeFixProvider();

    [Test]
    [Arguments("TestData/NFHYPERLINQ002/NoDiagnostic/KeepAsValueEnumerable.cs")]
    public async Task Verify_NoDiagnostics(string path) => await VerifyCSharpDiagnostic(File.ReadAllText(path));

    [Test]
    [Arguments("TestData/NFHYPERLINQ002/Diagnostic/AlreadyValueEnumerable.cs", "ArrayValueEnumerable<int>", "it is already a value enumerable", "TestData/NFHYPERLINQ002/Diagnostic/AlreadyValueEnumerable.Fix.cs", 10, 17)]
    // Using "List<int>" because of using System.Collections.Generic
    [Arguments("TestData/NFHYPERLINQ002/Diagnostic/DirectExtension.cs", "List<int>", "direct extension methods exist for Count()", "TestData/NFHYPERLINQ002/Diagnostic/DirectExtension.Fix.cs", 11, 17)]
    public async Task Verify_Diagnostics(string path, string typeName, string reason, string fixPath, int line, int column)
    {
        var expected = new DiagnosticResult("NFHYPERLINQ002", DiagnosticSeverity.Warning)
            .WithMessage($"AsValueEnumerable() is not needed on '{typeName}' because {reason}")
            .WithLocation("/0/Test0.cs", line, column);

        await VerifyCSharpDiagnostic(File.ReadAllText(path), expected);

        await VerifyCSharpFix(File.ReadAllText(path), File.ReadAllText(fixPath), expected);
    }
}
