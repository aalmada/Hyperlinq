using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Linq;
using NetFabric.Hyperlinq;

namespace NetFabric.Hyperlinq.Benchmarks
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class SpanOperationsBenchmarks
    {
        private int[] array = null!;

        [Params(100, 1_000, 10_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            array = ValueEnumerable.Range(0, Count).ToArray();
        }

        // ===== Span_Select =====

        [BenchmarkCategory("Span_Select"), Benchmark(Baseline = true)]
        public int Span_Select_LINQ()
        {
            var sum = 0;
            foreach (var item in Enumerable.Select(array, x => x * 2))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("Span_Select"), Benchmark]
        public int Span_Select_Hyperlinq_Span()
        {
            var sum = 0;
            foreach (var item in array.AsSpan().Select(x => x * 2))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("Span_Select"), Benchmark]
        public int Span_Select_Hyperlinq_Array()
        {
            var sum = 0;
            foreach (var item in array.Select(x => x * 2))
                sum += item;
            return sum;
        }

        // ===== Span_Where =====

        [BenchmarkCategory("Span_Where"), Benchmark(Baseline = true)]
        public int Span_Where_LINQ()
        {
            var sum = 0;
            foreach (var item in Enumerable.Where(array, x => x % 2 == 0))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("Span_Where"), Benchmark]
        public int Span_Where_Hyperlinq_Span()
        {
            var sum = 0;
            foreach (var item in array.AsSpan().Where(x => x % 2 == 0))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("Span_Where"), Benchmark]
        public int Span_Where_Hyperlinq_Array()
        {
            var sum = 0;
            foreach (var item in array.Where(x => x % 2 == 0))
                sum += item;
            return sum;
        }

        // ===== Span_WhereSelect =====

        [BenchmarkCategory("Span_WhereSelect"), Benchmark(Baseline = true)]
        public int Span_WhereSelect_LINQ()
        {
            var sum = 0;
            foreach (var item in Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => x * 2))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("Span_WhereSelect"), Benchmark]
        public int Span_WhereSelect_Hyperlinq_Span()
        {
            var sum = 0;
            foreach (var item in array.AsSpan().Where(x => x % 2 == 0).Select(x => x * 2))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("Span_WhereSelect"), Benchmark]
        public int Span_WhereSelect_Hyperlinq_Array()
        {
            var sum = 0;
            foreach (var item in array.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2))
                sum += item;
            return sum;
        }

        // ===== Span_WhereSum =====

        [BenchmarkCategory("Span_WhereSum"), Benchmark(Baseline = true)]
        public int Span_WhereSum_LINQ()
        {
            return Enumerable.Sum(Enumerable.Where(array, x => x % 2 == 0));
        }

        [BenchmarkCategory("Span_WhereSum"), Benchmark]
        public int Span_WhereSum_Hyperlinq_Span()
        {
            return array.AsSpan().Where(x => x % 2 == 0).Sum();
        }

        [BenchmarkCategory("Span_WhereSum"), Benchmark]
        public int Span_WhereSum_Hyperlinq_Array()
        {
            return array.Where(x => x % 2 == 0).Sum();
        }

        // ===== Span_WhereSelectSum =====

        [BenchmarkCategory("Span_WhereSelectSum"), Benchmark(Baseline = true)]
        public int Span_WhereSelectSum_LINQ()
        {
            return Enumerable.Sum(Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => x * 2));
        }

        [BenchmarkCategory("Span_WhereSelectSum"), Benchmark]
        public int Span_WhereSelectSum_Hyperlinq_Span()
        {
            return array.AsSpan().Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        }

        [BenchmarkCategory("Span_WhereSelectSum"), Benchmark]
        public int Span_WhereSelectSum_Hyperlinq_Array()
        {
            return array.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();
        }

        // ===== Span_SelectCount =====

        [BenchmarkCategory("Span_SelectCount"), Benchmark(Baseline = true)]
        public int Span_SelectCount_LINQ()
        {
            return Enumerable.Count(Enumerable.Select(array, x => x * 2));
        }

        [BenchmarkCategory("Span_SelectCount"), Benchmark]
        public int Span_SelectCount_Hyperlinq_Span()
        {
            return array.AsSpan().Select(x => x * 2).Count();
        }

        [BenchmarkCategory("Span_SelectCount"), Benchmark]
        public int Span_SelectCount_Hyperlinq_Array()
        {
            return array.Select(x => x * 2).Count();
        }

        // ===== Span_WhereCount =====

        [BenchmarkCategory("Span_WhereCount"), Benchmark(Baseline = true)]
        public int Span_WhereCount_LINQ()
        {
            return Enumerable.Count(Enumerable.Where(array, x => x % 2 == 0));
        }

        [BenchmarkCategory("Span_WhereCount"), Benchmark]
        public int Span_WhereCount_Hyperlinq_Span()
        {
            return array.AsSpan().Where(x => x % 2 == 0).Count();
        }

        [BenchmarkCategory("Span_WhereCount"), Benchmark]
        public int Span_WhereCount_Hyperlinq_Array()
        {
            return array.Where(x => x % 2 == 0).Count();
        }

        // ===== Span_SelectToArray =====

        [BenchmarkCategory("Span_SelectToArray"), Benchmark(Baseline = true)]
        public int[] Span_SelectToArray_LINQ()
        {
            return Enumerable.ToArray(Enumerable.Select(array, x => x * 2));
        }

        [BenchmarkCategory("Span_SelectToArray"), Benchmark]
        public int[] Span_SelectToArray_Hyperlinq_Span()
        {
            return array.AsSpan().Select(x => x * 2).ToArray();
        }

        [BenchmarkCategory("Span_SelectToArray"), Benchmark]
        public int[] Span_SelectToArray_Hyperlinq_Array()
        {
            return array.Select(x => x * 2).ToArray();
        }

        // ===== Span_WhereToArray =====

        [BenchmarkCategory("Span_WhereToArray"), Benchmark(Baseline = true)]
        public int[] Span_WhereToArray_LINQ()
        {
            return Enumerable.ToArray(Enumerable.Where(array, x => x % 2 == 0));
        }

        [BenchmarkCategory("Span_WhereToArray"), Benchmark]
        public int[] Span_WhereToArray_Hyperlinq_Span()
        {
            return array.AsSpan().Where(x => x % 2 == 0).ToArray();
        }

        [BenchmarkCategory("Span_WhereToArray"), Benchmark]
        public int[] Span_WhereToArray_Hyperlinq_Array()
        {
            return array.Where(x => x % 2 == 0).ToArray();
        }
    }
}
