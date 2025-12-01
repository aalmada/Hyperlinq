using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Hyperlinq;

namespace NetFabric.Hyperlinq.Benchmarks
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class PooledBenchmarks
    {
        private int[] array = null!;
        private List<int> list = null!;

        [Params(100, 1_000, 10_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            array = ValueEnumerable.Range(0, Count).ToArray();
            list = new List<int>(array);
        }

        // ===== ToArray =====

        [BenchmarkCategory("ToArray"), Benchmark(Baseline = true)]
        public int[] ToArray_LINQ()
            => Enumerable.ToArray(array);

        [BenchmarkCategory("ToArray"), Benchmark]
        public int[] ToArray_Hyperlinq()
            => array.AsSpan().ToArray();

        [BenchmarkCategory("ToArray"), Benchmark]
        public void ToArrayPooled_Hyperlinq()
        {
            using var buffer = array.AsSpan().ToArrayPooled();
        }

        // ===== ToList =====

        [BenchmarkCategory("ToList"), Benchmark(Baseline = true)]
        public List<int> ToList_LINQ()
            => Enumerable.ToList(array);



        [BenchmarkCategory("ToList"), Benchmark]
        public void ToListPooled_Hyperlinq()
        {
            using var buffer = array.AsSpan().ToListPooled();
        }

        // ===== Where_ToArray =====

        [BenchmarkCategory("Where_ToArray"), Benchmark(Baseline = true)]
        public int[] Where_ToArray_LINQ()
            => Enumerable.ToArray(Enumerable.Where(array, x => x % 2 == 0));

        [BenchmarkCategory("Where_ToArray"), Benchmark]
        public int[] Where_ToArray_Hyperlinq()
            => array.AsSpan().Where(x => x % 2 == 0).ToArray();

        [BenchmarkCategory("Where_ToArray"), Benchmark]
        public void Where_ToArrayPooled_Hyperlinq()
        {
            using var buffer = array.AsSpan().Where(x => x % 2 == 0).ToArrayPooled();
        }

        // ===== Where_ToList =====

        [BenchmarkCategory("Where_ToList"), Benchmark(Baseline = true)]
        public List<int> Where_ToList_LINQ()
            => Enumerable.ToList(Enumerable.Where(array, x => x % 2 == 0));

        [BenchmarkCategory("Where_ToList"), Benchmark]
        public List<int> Where_ToList_Hyperlinq()
            => array.AsSpan().Where(x => x % 2 == 0).ToList();

        [BenchmarkCategory("Where_ToList"), Benchmark]
        public void Where_ToListPooled_Hyperlinq()
        {
            using var buffer = array.AsSpan().Where(x => x % 2 == 0).ToListPooled();
        }

        // ===== List_Where_ToArray =====

        [BenchmarkCategory("List_Where_ToArray"), Benchmark(Baseline = true)]
        public int[] List_Where_ToArray_LINQ()
            => Enumerable.ToArray(Enumerable.Where(list, x => x % 2 == 0));

        [BenchmarkCategory("List_Where_ToArray"), Benchmark]
        public int[] List_Where_ToArray_Hyperlinq()
            => list.AsValueEnumerable().Where(x => x % 2 == 0).ToArray();

        [BenchmarkCategory("List_Where_ToArray"), Benchmark]
        public void List_Where_ToArrayPooled_Hyperlinq()
        {
            using var buffer = list.AsValueEnumerable().Where(x => x % 2 == 0).ToArrayPooled();
        }

        // ===== Range_ToArray =====

        [BenchmarkCategory("Range_ToArray"), Benchmark(Baseline = true)]
        public int[] Range_ToArray_LINQ()
            => Enumerable.ToArray(Enumerable.Range(0, Count));

        [BenchmarkCategory("Range_ToArray"), Benchmark]
        public int[] Range_ToArray_Hyperlinq()
            => ValueEnumerable.Range(0, Count).ToArray();

        [BenchmarkCategory("Range_ToArray"), Benchmark]
        public void Range_ToArrayPooled_Hyperlinq()
        {
            using var buffer = ValueEnumerable.Range(0, Count).ToArrayPooled();
        }

        // ===== Range_ToList =====

        [BenchmarkCategory("Range_ToList"), Benchmark(Baseline = true)]
        public List<int> Range_ToList_LINQ()
            => Enumerable.ToList(Enumerable.Range(0, Count));

        [BenchmarkCategory("Range_ToList"), Benchmark]
        public List<int> Range_ToList_Hyperlinq()
            => ValueEnumerable.Range(0, Count).ToList();

        [BenchmarkCategory("Range_ToList"), Benchmark]
        public void Range_ToListPooled_Hyperlinq()
        {
            using var buffer = ValueEnumerable.Range(0, Count).ToListPooled();
        }
    }
}
