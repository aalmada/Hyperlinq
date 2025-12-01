using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Hyperlinq;

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
            enumerable = ValueEnumerable.Range(0, Count);
            list = enumerable.ToList();
            array = enumerable.ToArray();
        }

        // ===== Array_Sum =====
        
        [BenchmarkCategory("Array_Sum"), Benchmark(Baseline = true)]
        public int Array_LINQ_Sum()
        {
            return Enumerable.Sum(array);
        }

        [BenchmarkCategory("Array_Sum"), Benchmark]
        public int Array_Hyperlinq_Sum()
        {
            return array.Sum();  // Hyperlinq extension
        }

        // ===== Array_WhereSelectSum =====
        
        [BenchmarkCategory("Array_WhereSelectSum"), Benchmark(Baseline = true)]
        public int Array_LINQ_WhereSelectSum()
        {
            return Enumerable.Sum(Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => x * 2));
        }

        [BenchmarkCategory("Array_WhereSelectSum"), Benchmark]
        public int Array_Hyperlinq_WhereSelectSum()
        {
            return array.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();  // Hyperlinq extensions
        }

        // ===== IEnumerable_Sum =====
        
        [BenchmarkCategory("IEnumerable_Sum"), Benchmark(Baseline = true)]
        public int IEnumerable_LINQ_Sum()
        {
            return Enumerable.Sum(enumerable);
        }

        [BenchmarkCategory("IEnumerable_Sum"), Benchmark]
        public int IEnumerable_Hyperlinq_Sum()
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
        public int IEnumerable_Hyperlinq_WhereSelectSum()
        {
            return enumerable.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        }

        // ===== List_Any =====
        
        [BenchmarkCategory("List_Any"), Benchmark(Baseline = true)]
        public bool List_LINQ_Any()
        {
            return Enumerable.Any(list);
        }

        [BenchmarkCategory("List_Any"), Benchmark]
        public bool List_Hyperlinq_Any()
        {
            return list.Any();  // Hyperlinq extension
        }

        // ===== List_Count =====
        
        [BenchmarkCategory("List_Count"), Benchmark(Baseline = true)]
        public int List_LINQ_Count()
        {
            return Enumerable.Count(list);
        }

        [BenchmarkCategory("List_Count"), Benchmark]
        public int List_Hyperlinq_Count()
        {
            return list.Count();  // Hyperlinq extension (property access)
        }

        // ===== List_Sum =====
        
        [BenchmarkCategory("List_Sum"), Benchmark(Baseline = true)]
        public int List_LINQ_Sum()
        {
            return Enumerable.Sum(list);
        }

        [BenchmarkCategory("List_Sum"), Benchmark]
        public int List_Hyperlinq_Sum()
        {
            return list.Sum();  // Hyperlinq extension
        }

        // ===== List_WhereSelectSum =====
        
        [BenchmarkCategory("List_WhereSelectSum"), Benchmark(Baseline = true)]
        public int List_LINQ_WhereSelectSum()
        {
            return Enumerable.Sum(Enumerable.Select(Enumerable.Where(list, x => x % 2 == 0), x => x * 2));
        }

        [BenchmarkCategory("List_WhereSelectSum"), Benchmark]
        public int List_Hyperlinq_WhereSelectSum()
        {
            return list.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();  // Hyperlinq extensions
        }

        // ===== IEnumerable_Select =====

        [BenchmarkCategory("IEnumerable_Select"), Benchmark(Baseline = true)]
        public int IEnumerable_LINQ_Select()
        {
            var sum = 0;
            foreach (var item in Enumerable.Select(enumerable, x => x * 2))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("IEnumerable_Select"), Benchmark]
        public int IEnumerable_Hyperlinq_Select()
        {
            var sum = 0;
            foreach (var item in enumerable.AsValueEnumerable().Select(x => x * 2))
                sum += item;
            return sum;
        }

        // ===== IEnumerable_Where =====

        [BenchmarkCategory("IEnumerable_Where"), Benchmark(Baseline = true)]
        public int IEnumerable_LINQ_Where()
        {
            var sum = 0;
            foreach (var item in Enumerable.Where(enumerable, x => x % 2 == 0))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("IEnumerable_Where"), Benchmark]
        public int IEnumerable_Hyperlinq_Where()
        {
            var sum = 0;
            foreach (var item in enumerable.AsValueEnumerable().Where(x => x % 2 == 0))
                sum += item;
            return sum;
        }

        // ===== IEnumerable_WhereSelect =====

        [BenchmarkCategory("IEnumerable_WhereSelect"), Benchmark(Baseline = true)]
        public int IEnumerable_LINQ_WhereSelect()
        {
            var sum = 0;
            foreach (var item in Enumerable.Select(Enumerable.Where(enumerable, x => x % 2 == 0), x => x * 2))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("IEnumerable_WhereSelect"), Benchmark]
        public int IEnumerable_Hyperlinq_WhereSelect()
        {
            var sum = 0;
            foreach (var item in enumerable.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2))
                sum += item;
            return sum;
        }

        // ===== IEnumerable_Count =====

        [BenchmarkCategory("IEnumerable_Count"), Benchmark(Baseline = true)]
        public int IEnumerable_LINQ_Count()
        {
            return Enumerable.Count(enumerable);
        }

        [BenchmarkCategory("IEnumerable_Count"), Benchmark]
        public int IEnumerable_Hyperlinq_Count()
        {
            return enumerable.AsValueEnumerable().Count();
        }

        // ===== IEnumerable_Any =====

        [BenchmarkCategory("IEnumerable_Any"), Benchmark(Baseline = true)]
        public bool IEnumerable_LINQ_Any()
        {
            return Enumerable.Any(enumerable);
        }

        [BenchmarkCategory("IEnumerable_Any"), Benchmark]
        public bool IEnumerable_Hyperlinq_Any()
        {
            return enumerable.AsValueEnumerable().Any();
        }
    }
}
