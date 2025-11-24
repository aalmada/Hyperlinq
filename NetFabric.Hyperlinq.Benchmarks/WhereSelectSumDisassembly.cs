using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using System.Linq;

namespace NetFabric.Hyperlinq.Benchmarks
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(maxDepth: 3, exportHtml: true, exportDiff: true)]
    public class WhereSelectSumDisassembly
    {
        private int[] array = null!;

        [Params(10_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            array = Enumerable.Range(0, Count).ToArray();
        }

        [Benchmark(Baseline = true)]
        public int LINQ_WhereSelectSum()
        {
            return array.Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        }

        [Benchmark]
        public int Hyperlinq_WhereSelectSum()
        {
            return array.Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        }
    }
}
