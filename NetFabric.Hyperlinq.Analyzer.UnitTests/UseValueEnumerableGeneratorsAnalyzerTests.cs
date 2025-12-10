using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using NetFabric.Hyperlinq.Analyzer.UnitTests.Verifiers;
using System.IO;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Hyperlinq.Analyzer;

namespace NetFabric.Hyperlinq.Analyzer.UnitTests
{
    public class UseValueEnumerableGeneratorsAnalyzerTests : CodeFixVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
            => new UseValueEnumerableGeneratorsAnalyzer();

        protected override CodeFixProvider GetCSharpCodeFixProvider()
            => new UseValueEnumerableGeneratorsCodeFixProvider();

        [Test]
        [Arguments("TestData/NFHYPERLINQ005/NoDiagnostic/ValueEnumerable.cs")]
        public async Task Verify_NoDiagnostics(string path)
        {
            await VerifyCSharpDiagnostic(File.ReadAllText(path));
        }

        [Test]
        [Arguments("TestData/NFHYPERLINQ005/Diagnostic/Range.cs", "Range", "TestData/NFHYPERLINQ005/Diagnostic/Range.Fix.cs", 9, 17)]
        [Arguments("TestData/NFHYPERLINQ005/Diagnostic/Repeat.cs", "Repeat", "TestData/NFHYPERLINQ005/Diagnostic/Repeat.Fix.cs", 9, 17)]
        [Arguments("TestData/NFHYPERLINQ005/Diagnostic/Empty.cs", "Empty", "TestData/NFHYPERLINQ005/Diagnostic/Empty.Fix.cs", 9, 17)]
        [Arguments("TestData/NFHYPERLINQ005/Diagnostic/Return.cs", "Return", "TestData/NFHYPERLINQ005/Diagnostic/Return.Fix.cs", 14, 17)]
        public async Task Verify_Diagnostics(string path, string methodName, string fixPath, int line, int column)
        {
            var expected = new DiagnosticResult("NFHYPERLINQ005", DiagnosticSeverity.Info)
                .WithMessage($"Use 'ValueEnumerable.{methodName}' instead of 'Enumerable.{methodName}' for better performance")
                .WithLocation("/0/Test0.cs", line, column);

            await VerifyCSharpDiagnostic(File.ReadAllText(path), expected);
            
            await VerifyCSharpFix(File.ReadAllText(path), File.ReadAllText(fixPath), expected);
        }
    }
}
