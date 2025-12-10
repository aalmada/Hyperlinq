using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using NetFabric.Hyperlinq.Analyzer.UnitTests.Verifiers;
using TUnit.Core;

namespace NetFabric.Hyperlinq.Analyzer.UnitTests
{
    public class AvoidSmallValueDelegateInAnalyzerTests : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
            => new AvoidSmallValueDelegateInAnalyzer();

        [Test]
        [Arguments("TestData/HLQ012/NoDiagnostic/LargeStructIn.cs")]
        [Arguments("TestData/HLQ012/NoDiagnostic/SmallStructWithBoth.cs")]
        public async Task Verify_NoDiagnostics(string path)
        {
            await VerifyCSharpDiagnostic(File.ReadAllText(path));
        }

        [Test]
        [Arguments("TestData/HLQ012/Diagnostic/SmallStructIn.cs", "SmallStructIn", 8, 5, 8)]
        public async Task Verify_Diagnostics(string path, string structName, int size, int line, int column)
        {
            var expected = new DiagnosticResult("HLQ012", DiagnosticSeverity.Warning)
                .WithMessage($"Struct '{structName}' is {size} bytes. Consider implementing IFunction<T, TResult> (pass-by-value) instead of IFunctionIn<T, TResult> to avoid indirection overhead.")
                .WithLocation("/0/Test0.cs", line, column);

            await VerifyCSharpDiagnostic(File.ReadAllText(path), expected);
        }
    }
}
