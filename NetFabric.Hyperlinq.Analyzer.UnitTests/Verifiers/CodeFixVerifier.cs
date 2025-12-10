using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
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
    public abstract class CodeFixVerifier : DiagnosticVerifier
    {
        protected abstract CodeFixProvider GetCSharpCodeFixProvider();

        public async Task VerifyCSharpFix(string source, string fixedSource, params DiagnosticResult[] expected)
            => await VerifyCSharpFix(new[] { source }, fixedSource, expected);

        public async Task VerifyCSharpFix(string[] sources, string fixedSource, params DiagnosticResult[] expected)
        {
            var test = new TestImplementation(GetCSharpDiagnosticAnalyzer(), GetCSharpCodeFixProvider());
            
            foreach (var source in sources)
            {
                test.TestState.Sources.Add(source);
            }
            
            test.FixedState.Sources.Add(fixedSource);
            for (int i = 1; i < sources.Length; i++)
            {
                test.FixedState.Sources.Add(sources[i]);
            }
            
            test.ExpectedDiagnostics.AddRange(expected);

            await test.RunAsync();
        }

        private class TestImplementation : CSharpCodeFixTest<DummyAnalyzer, DummyCodeFixProvider, TUnitVerifier>
        {
            private readonly DiagnosticAnalyzer _analyzer;
            private readonly CodeFixProvider _codeFix;

            public TestImplementation(DiagnosticAnalyzer analyzer, CodeFixProvider codeFix)
            {
                _analyzer = analyzer;
                _codeFix = codeFix;

                this.ReferenceAssemblies = ReferenceAssemblies.Default;
                
                this.SolutionTransforms.Add((solution, projectId) =>
                {
                    var project = solution.GetProject(projectId);
                    
                    var newRefs = new List<MetadataReference>();
                    newRefs.Add(MetadataReference.CreateFromFile(typeof(NetFabric.Hyperlinq.ValueEnumerable).Assembly.Location));
                    
                    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (!asm.IsDynamic && !string.IsNullOrEmpty(asm.Location))
                        {
                            try {
                                newRefs.Add(MetadataReference.CreateFromFile(asm.Location));
                            } catch { }
                        }
                    }
                    
                    return project.WithMetadataReferences(newRefs).Solution;
                });
            }

            protected override IEnumerable<DiagnosticAnalyzer> GetDiagnosticAnalyzers()
            {
                yield return _analyzer;
            }

            protected override IEnumerable<CodeFixProvider> GetCodeFixProviders()
            {
                yield return _codeFix;
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

        private class DummyCodeFixProvider : CodeFixProvider
        {
            public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray<string>.Empty;
            public override Task RegisterCodeFixesAsync(CodeFixContext context) => Task.CompletedTask;
            public sealed override FixAllProvider? GetFixAllProvider() => null;
        }
    }
}
