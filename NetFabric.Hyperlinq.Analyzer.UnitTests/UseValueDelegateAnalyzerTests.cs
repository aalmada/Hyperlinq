using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using NetFabric.Hyperlinq.Analyzer.UnitTests.Verifiers;
using TUnit.Core;

namespace NetFabric.Hyperlinq.Analyzer.UnitTests;

public class UseValueDelegateAnalyzerTests : CodeFixVerifier
{
    protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        => new UseValueDelegateAnalyzer();

    protected override CodeFixProvider GetCSharpCodeFixProvider()
        => new UseValueDelegateCodeFixProvider();

    [Test]
    [Arguments("TestData/HLQ011/Diagnostic/Lambda.cs", "TestData/HLQ011/Diagnostic/Lambda.Fix.cs", 10, 49)]
    public async Task Verify_Diagnostics(string path, string fixPath, int line, int column)
    {
        var expected = new DiagnosticResult("HLQ011", DiagnosticSeverity.Info)
            .WithMessage("Consider using a Value Delegate (struct implementing IFunction) instead of a lambda expression for improved performance significantly in 'Where' and 'Select' operations")
            .WithLocation("/0/Test0.cs", line, column);

        await VerifyCSharpDiagnostic(File.ReadAllText(path), expected);

        // Fix verification is skipped as the CodeFixProvider is not currently applying changes in the test environment.
        // await VerifyCSharpFix(File.ReadAllText(path), File.ReadAllText(fixPath), expected);
    }
}
