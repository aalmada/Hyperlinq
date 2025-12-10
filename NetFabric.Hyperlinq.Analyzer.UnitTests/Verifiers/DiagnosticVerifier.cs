using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace NetFabric.Hyperlinq.Analyzer.UnitTests.Verifiers
{
    public abstract class DiagnosticVerifier
    {
        protected abstract DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer();

        public async Task VerifyCSharpDiagnostic(string source, params DiagnosticResult[] expected)
            => await VerifyCSharpDiagnostic(new[] { source }, expected);

        public async Task VerifyCSharpDiagnostic(string[] sources, params DiagnosticResult[] expected)
        {
            var test = new TestImplementation(GetCSharpDiagnosticAnalyzer());
            foreach (var source in sources)
            {
                test.TestState.Sources.Add(source);
            }
            test.ExpectedDiagnostics.AddRange(expected);
            await test.RunAsync();
        }

        private class TestImplementation : CSharpAnalyzerTest<DummyAnalyzer, TUnitVerifier>
        {
            private readonly DiagnosticAnalyzer _analyzer;

            public TestImplementation(DiagnosticAnalyzer analyzer)
            {
                _analyzer = analyzer;

                // Use Default just to provide a value and avoid NRE.
                this.ReferenceAssemblies = ReferenceAssemblies.Default;
                
                this.SolutionTransforms.Add((solution, projectId) =>
                {
                    var project = solution.GetProject(projectId);
                    
                    // Replace all references with current runtime assemblies + NetFabric.Hyperlinq
                    var newRefs = new List<MetadataReference>();
                    
                    // Add NetFabric.Hyperlinq specifically (if loaded) or from type
                    newRefs.Add(MetadataReference.CreateFromFile(typeof(NetFabric.Hyperlinq.ValueEnumerable).Assembly.Location));
                    
                    // Add all runtime assemblies
                    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (!asm.IsDynamic && !string.IsNullOrEmpty(asm.Location))
                        {
                            // Avoid duplicates or conflicts? 
                            // Roslyn handles duplicates if paths are identical.
                            // But if we have multiple versions of same assembly loaded (unlikely in default context), might be issue.
                            // Safe enough.
                            try {
                                newRefs.Add(MetadataReference.CreateFromFile(asm.Location));
                            } catch { /* ignore */ }
                        }
                    }
                    
                    return project.WithMetadataReferences(newRefs).Solution;
                });
            }

            protected override IEnumerable<DiagnosticAnalyzer> GetDiagnosticAnalyzers()
            {
                yield return _analyzer;
            }
        }

        [DiagnosticAnalyzer(LanguageNames.CSharp)]
        private class DummyAnalyzer : DiagnosticAnalyzer
        {
            public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray<DiagnosticDescriptor>.Empty;
            public override void Initialize(AnalysisContext context) 
            {
                context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
                context.EnableConcurrentExecution();
            }
        }
    }
}
