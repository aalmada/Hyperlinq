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
    public class SuggestRemoveAsValueEnumerableAnalyzerTests : CodeFixVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
            => new SuggestRemoveAsValueEnumerableAnalyzer();

        protected override CodeFixProvider GetCSharpCodeFixProvider()
            => new SuggestRemoveAsValueEnumerableCodeFixProvider();

        [Test]
        [Arguments("TestData/NFHYPERLINQ003/NoDiagnostic/Chained.cs")]
        public async Task Verify_NoDiagnostics(string path)
        {
            await VerifyCSharpDiagnostic(File.ReadAllText(path));
        }

        [Test]
        [Arguments("TestData/NFHYPERLINQ003/Diagnostic/SingleOperation.cs", "List<int>", "TestData/NFHYPERLINQ003/Diagnostic/SingleOperation.Fix.cs", 11, 17)]
        public async Task Verify_Diagnostics(string path, string typeName, string fixPath, int line, int column)
        {
            var expected = new DiagnosticResult("NFHYPERLINQ003", DiagnosticSeverity.Info)
                .WithMessage($"Remove AsValueEnumerable() on '{typeName}' - direct extension methods provide better performance for non-chained operations")
                .WithLocation("/0/Test0.cs", line, column);

            await VerifyCSharpDiagnostic(File.ReadAllText(path), expected);
            
            await VerifyCSharpFix(File.ReadAllText(path), File.ReadAllText(fixPath), expected);
        }
    }
}
