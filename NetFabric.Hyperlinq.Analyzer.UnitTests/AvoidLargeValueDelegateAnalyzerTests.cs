using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using NetFabric.Hyperlinq.Analyzer.UnitTests.Verifiers;
using TUnit.Core;

namespace NetFabric.Hyperlinq.Analyzer.UnitTests;

public class AvoidLargeValueDelegateAnalyzerTests : DiagnosticVerifier
{
    protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        => new AvoidLargeValueDelegateAnalyzer();

    [Test]
    [Arguments("TestData/HLQ010/NoDiagnostic/SmallStruct.cs")]
    [Arguments("TestData/HLQ010/NoDiagnostic/LargeStructIn.cs")]
    public async Task Verify_NoDiagnostics(string path) => await VerifyCSharpDiagnostic(File.ReadAllText(path));

    [Test]
    [Arguments("TestData/HLQ010/Diagnostic/LargeStruct.cs", "LargeStruct", 80, 5, 8)]
    public async Task Verify_Diagnostics(string path, string structName, int size, int line, int column)
    {
        var expected = new DiagnosticResult("HLQ010", DiagnosticSeverity.Warning)
            .WithMessage($"Struct '{structName}' is {size} bytes. Consider implementing IFunctionIn<T, TResult> to avoid copying overhead.")
            .WithLocation("/0/Test0.cs", line, column);

        await VerifyCSharpDiagnostic(File.ReadAllText(path), expected);
    }
}
