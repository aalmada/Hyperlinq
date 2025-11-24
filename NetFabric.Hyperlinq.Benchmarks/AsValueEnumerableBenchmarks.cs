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
        private IEnumerable<int> enumerable = null!;
        private List<int> list = null!;
        private int[] array = null!;

        [Params(10_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            enumerable = Enumerable.Range(0, Count);
            list = enumerable.ToList();
            array = enumerable.ToArray();
        }

        // ===== Array_Sum =====
        
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

        // ===== Array_WhereSelectSum =====
        
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

        // ===== IEnumerable_Sum =====
        
        [BenchmarkCategory("IEnumerable_Sum"), Benchmark(Baseline = true)]
        public int IEnumerable_LINQ_Sum()
        {
            return enumerable.Sum();
        }

        [BenchmarkCategory("IEnumerable_Sum"), Benchmark]
        public int IEnumerable_AsValueEnumerable_Sum()
        {
            return enumerable.AsValueEnumerable().Sum();
        }

        // ===== IEnumerable_WhereSelectSum =====
        
        [BenchmarkCategory("IEnumerable_WhereSelectSum"), Benchmark(Baseline = true)]
        public int IEnumerable_LINQ_WhereSelectSum()
        {
            return enumerable.Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        }

        [BenchmarkCategory("IEnumerable_WhereSelectSum"), Benchmark]
        public int IEnumerable_AsValueEnumerable_WhereSelectSum()
        {
            return enumerable.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        }

        // ===== List_Any =====
        
        [BenchmarkCategory("List_Any"), Benchmark(Baseline = true)]
        public bool List_LINQ_Any()
        {
            return list.Any();
        }

        [BenchmarkCategory("List_Any"), Benchmark]
        public bool List_AsValueEnumerable_Any()
        {
            return list.AsValueEnumerable().Any();
        }

        // ===== List_Count =====
        
        [BenchmarkCategory("List_Count"), Benchmark(Baseline = true)]
        public int List_LINQ_Count()
        {
            return list.Count();
        }

        [BenchmarkCategory("List_Count"), Benchmark]
        public int List_AsValueEnumerable_Count()
        {
            return list.AsValueEnumerable().Count();
        }

        // ===== List_Sum =====
        
        [BenchmarkCategory("List_Sum"), Benchmark(Baseline = true)]
        public int List_LINQ_Sum()
        {
            return list.Sum();
        }

        [BenchmarkCategory("List_Sum"), Benchmark]
        public int List_AsValueEnumerable_Sum()
        {
            return list.AsValueEnumerable().Sum();
        }

        // ===== List_WhereSelectSum =====
        
        [BenchmarkCategory("List_WhereSelectSum"), Benchmark(Baseline = true)]
        public int List_LINQ_WhereSelectSum()
        {
            return list.Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        }

        [BenchmarkCategory("List_WhereSelectSum"), Benchmark]
        public int List_AsValueEnumerable_WhereSelectSum()
        {
            return list.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        }
    }
}
