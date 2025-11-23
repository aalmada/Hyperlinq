using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Hyperlinq.Benchmarks
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class AsValueEnumerableBenchmarks
    {
        private List<int> list = null!;
        private int[] array = null!;

        [Params(100, 10_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            list = Enumerable.Range(0, Count).ToList();
            array = Enumerable.Range(0, Count).ToArray();
        }

        // Sum benchmarks
        [BenchmarkCategory("Sum"), Benchmark(Baseline = true)]
        public int List_LINQ_Sum()
        {
            return list.Sum();
        }

        [BenchmarkCategory("Sum"), Benchmark]
        public int List_AsValueEnumerable_Sum()
        {
            return list.AsValueEnumerable().Sum();
        }

        // WhereSelectSum benchmarks
        [BenchmarkCategory("WhereSelectSum"), Benchmark(Baseline = true)]
        public int List_LINQ_WhereSelectSum()
        {
            return list.Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        }

        [BenchmarkCategory("WhereSelectSum"), Benchmark]
        public int List_AsValueEnumerable_WhereSelectSum()
        {
            return list.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        }

        // Array Sum benchmarks
        [BenchmarkCategory("Array_Sum"), Benchmark(Baseline = true)]
        public int Array_LINQ_Sum()
        {
            return array.Sum();
        }

        [BenchmarkCategory("Array_Sum"), Benchmark]
        public int Array_AsValueEnumerable_Sum()
        {
            return array.AsValueEnumerable().Sum();
        }

        // Array WhereSelectSum benchmarks
        [BenchmarkCategory("Array_WhereSelectSum"), Benchmark(Baseline = true)]
        public int Array_LINQ_WhereSelectSum()
        {
            return array.Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        }

        [BenchmarkCategory("Array_WhereSelectSum"), Benchmark]
        public int Array_AsValueEnumerable_WhereSelectSum()
        {
            return array.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        }

        // Count benchmarks
        [BenchmarkCategory("Count"), Benchmark(Baseline = true)]
        public int List_LINQ_Count()
        {
            return list.Count();
        }

        [BenchmarkCategory("Count"), Benchmark]
        public int List_AsValueEnumerable_Count()
        {
            return list.AsValueEnumerable().Count();
        }

        // Any benchmarks
        [BenchmarkCategory("Any"), Benchmark(Baseline = true)]
        public bool List_LINQ_Any()
        {
            return list.Any();
        }

        [BenchmarkCategory("Any"), Benchmark]
        public bool List_AsValueEnumerable_Any()
        {
            return list.AsValueEnumerable().Any();
        }
    }
}
