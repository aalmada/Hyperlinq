using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using NetFabric.Hyperlinq.Analyzer.UnitTests.Verifiers;
using TUnit.Core;

namespace NetFabric.Hyperlinq.Analyzer.UnitTests
{
    public class SuggestAsValueEnumerableAnalyzerTests : CodeFixVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
            => new SuggestAsValueEnumerableAnalyzer();

        protected override CodeFixProvider GetCSharpCodeFixProvider()
            => new SuggestAsValueEnumerableCodeFixProvider();

        [Test]
        [Arguments("TestData/NFHYPERLINQ004/NoDiagnostic/Single.cs")]
        public async Task Verify_NoDiagnostics(string path)
        {
            await VerifyCSharpDiagnostic(File.ReadAllText(path));
        }

        [Test]
        [Arguments("TestData/NFHYPERLINQ004/Diagnostic/Chained.cs", "Where", "TestData/NFHYPERLINQ004/Diagnostic/Chained.Fix.cs", 11, 17)]
        public async Task Verify_Diagnostics(string path, string methodName, string fixPath, int line, int column)
        {
            var expected = new DiagnosticResult("NFHYPERLINQ004", DiagnosticSeverity.Error)
                .WithMessage($"Use AsValueEnumerable() before '{methodName}' to enable chaining operations")
                .WithLocation("/0/Test0.cs", line, column);

            await VerifyCSharpDiagnostic(File.ReadAllText(path), expected);
            
            await VerifyCSharpFix(File.ReadAllText(path), File.ReadAllText(fixPath), expected);
        }
    }
}
