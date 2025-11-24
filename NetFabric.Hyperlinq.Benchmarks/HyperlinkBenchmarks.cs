using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Hyperlinq.Benchmarks
{
    /// <summary>
    /// Benchmarks comparing LINQ vs Hyperlinq span-based extensions.
    /// Focus: Demonstrating the performance benefits of our optimizations.
    /// </summary>
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class HyperlinkBenchmarks
    {
        private IEnumerable<int> enumerable = null!;
        private List<int> list = null!;
        private int[] array = null!;

        [Params(1_000, 10_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            enumerable = Enumerable.Range(0, Count);
            list = enumerable.ToList();
            array = enumerable.ToArray();
        }

        // ===== Array: Where =====
        
        [BenchmarkCategory("Array_Where"), Benchmark(Baseline = true)]
        public int Array_LINQ_Where()
        {
            var sum = 0;
            foreach (var item in array.Where(x => x % 2 == 0))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("Array_Where"), Benchmark]
        public int Array_Hyperlinq_Where()
        {
            var sum = 0;
            foreach (var item in array.Where(x => x % 2 == 0))  // Uses WhereMemoryEnumerable
                sum += item;
            return sum;
        }

        // ===== Array: Where + Select (Operation Fusion) =====
        
        [BenchmarkCategory("Array_WhereSelect"), Benchmark(Baseline = true)]
        public int Array_LINQ_WhereSelect()
        {
            var sum = 0;
            foreach (var item in array.Where(x => x % 2 == 0).Select(x => x * 2))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("Array_WhereSelect"), Benchmark]
        public int Array_Hyperlinq_WhereSelect()
        {
            var sum = 0;
            foreach (var item in array.Where(x => x % 2 == 0).Select(x => x * 2))  // Uses WhereSelectMemoryEnumerable
                sum += item;
            return sum;
        }

        // ===== List: Where =====
        
        [BenchmarkCategory("List_Where"), Benchmark(Baseline = true)]
        public int List_LINQ_Where()
        {
            var sum = 0;
            foreach (var item in list.Where(x => x % 2 == 0))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("List_Where"), Benchmark]
        public int List_Hyperlinq_Where()
        {
            var sum = 0;
            foreach (var item in list.Where(x => x % 2 == 0))  // Uses WhereListEnumerable
                sum += item;
            return sum;
        }

        // ===== List: Where + Select (Operation Fusion) =====
        
        [BenchmarkCategory("List_WhereSelect"), Benchmark(Baseline = true)]
        public int List_LINQ_WhereSelect()
        {
            var sum = 0;
            foreach (var item in list.Where(x => x % 2 == 0).Select(x => x * 2))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("List_WhereSelect"), Benchmark]
        public int List_Hyperlinq_WhereSelect()
        {
            var sum = 0;
            foreach (var item in list.Where(x => x % 2 == 0).Select(x => x * 2))  // Uses WhereSelectListEnumerable
                sum += item;
            return sum;
        }

        // ===== Array: Sum (SIMD) =====
        
        [BenchmarkCategory("Array_Sum"), Benchmark(Baseline = true)]
        public int Array_LINQ_Sum()
        {
            return Enumerable.Sum(array);
        }

        [BenchmarkCategory("Array_Sum"), Benchmark]
        public int Array_Hyperlinq_Sum()
        {
            return array.Sum();  // Uses TensorPrimitives SIMD
        }

        // ===== List: Sum (SIMD) =====
        
        [BenchmarkCategory("List_Sum"), Benchmark(Baseline = true)]
        public int List_LINQ_Sum()
        {
            return Enumerable.Sum(list);
        }

        [BenchmarkCategory("List_Sum"), Benchmark]
        public int List_Hyperlinq_Sum()
        {
            return list.Sum();  // Uses TensorPrimitives SIMD via CollectionsMarshal
        }

        // ===== Array: Where + Sum (Chaining) =====
        
        [BenchmarkCategory("Array_WhereSum"), Benchmark(Baseline = true)]
        public int Array_LINQ_WhereSum()
        {
            return array.Where(x => x % 2 == 0).Sum();
        }

        [BenchmarkCategory("Array_WhereSum"), Benchmark]
        public int Array_Hyperlinq_WhereSum()
        {
            return array.Where(x => x % 2 == 0).Sum();  // WhereMemoryEnumerable + SIMD Sum
        }

        // ===== List: Where + Sum (Chaining) =====
        
        [BenchmarkCategory("List_WhereSum"), Benchmark(Baseline = true)]
        public int List_LINQ_WhereSum()
        {
            return list.Where(x => x % 2 == 0).Sum();
        }

        [BenchmarkCategory("List_WhereSum"), Benchmark]
        public int List_Hyperlinq_WhereSum()
        {
            return list.Where(x => x % 2 == 0).Sum();  // WhereListEnumerable + SIMD Sum
        }

        // ===== IEnumerable: Where + Select + Sum (AsValueEnumerable) =====
        
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

        // ===== List: Count (O(1) optimization) =====
        
        [BenchmarkCategory("List_Count"), Benchmark(Baseline = true)]
        public int List_LINQ_Count()
        {
            return Enumerable.Count(list);
        }

        [BenchmarkCategory("List_Count"), Benchmark]
        public int List_Hyperlinq_Count()
        {
            return list.Count();  // Direct property access
        }

        // ===== List: Any (O(1) optimization) =====
        
        [BenchmarkCategory("List_Any"), Benchmark(Baseline = true)]
        public bool List_LINQ_Any()
        {
            return Enumerable.Any(list);
        }

        [BenchmarkCategory("List_Any"), Benchmark]
        public bool List_Hyperlinq_Any()
        {
            return list.Any();  // Direct Count > 0 check
        }
    }
}
