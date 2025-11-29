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
    public class LastOperationsBenchmarks
    {
        private int[] array = null!;
        private List<int> list = null!;
        private ReadOnlyMemory<int> memory;

        [Params(100, 1_000, 10_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            array = Enumerable.Range(0, Count).ToArray();
            list = array.ToList();
            memory = array.AsMemory();
        }

        // ===== Array_Last =====

        [BenchmarkCategory("Array_Last"), Benchmark(Baseline = true)]
        public int Array_Last_LINQ()
        {
            return Enumerable.Last(array);
        }

        [BenchmarkCategory("Array_Last"), Benchmark]
        public int Array_Last_Hyperlinq()
        {
            return array.Last();
        }

        [BenchmarkCategory("Array_Last"), Benchmark]
        public int Array_Last_Manual()
        {
            return array[^1];
        }

        // ===== Array_LastPredicate =====

        [BenchmarkCategory("Array_LastPredicate"), Benchmark(Baseline = true)]
        public int Array_LastPredicate_LINQ()
        {
            return Enumerable.Last(array, x => x % 2 == 0);
        }

        [BenchmarkCategory("Array_LastPredicate"), Benchmark]
        public int Array_LastPredicate_Hyperlinq()
        {
            return array.Last(x => x % 2 == 0);
        }

        [BenchmarkCategory("Array_LastPredicate"), Benchmark]
        public int Array_LastPredicate_Manual()
        {
            for (var i = array.Length - 1; i >= 0; i--)
            {
                if (array[i] % 2 == 0)
                    return array[i];
            }
            throw new InvalidOperationException();
        }

        // ===== List_Last =====

        [BenchmarkCategory("List_Last"), Benchmark(Baseline = true)]
        public int List_Last_LINQ()
        {
            return Enumerable.Last(list);
        }

        [BenchmarkCategory("List_Last"), Benchmark]
        public int List_Last_Hyperlinq()
        {
            return list.Last();
        }

        [BenchmarkCategory("List_Last"), Benchmark]
        public int List_Last_Manual()
        {
            return list[^1];
        }

        // ===== List_LastPredicate =====

        [BenchmarkCategory("List_LastPredicate"), Benchmark(Baseline = true)]
        public int List_LastPredicate_LINQ()
        {
            return Enumerable.Last(list, x => x % 2 == 0);
        }

        [BenchmarkCategory("List_LastPredicate"), Benchmark]
        public int List_LastPredicate_Hyperlinq()
        {
            return list.Last(x => x % 2 == 0);
        }

        // ===== Memory_Last =====

        [BenchmarkCategory("Memory_Last"), Benchmark(Baseline = true)]
        public int Memory_Last_LINQ()
        {
            return Enumerable.Last(memory.ToArray());
        }

        [BenchmarkCategory("Memory_Last"), Benchmark]
        public int Memory_Last_Hyperlinq()
        {
            return memory.Last();
        }

        [BenchmarkCategory("Memory_Last"), Benchmark]
        public int Memory_Last_Span()
        {
            return memory.Span[^1];
        }

        // ===== Span_Last =====

        [BenchmarkCategory("Span_Last"), Benchmark(Baseline = true)]
        public int Span_Last_Manual()
        {
            var span = array.AsSpan();
            return span[^1];
        }

        [BenchmarkCategory("Span_Last"), Benchmark]
        public int Span_Last_Hyperlinq()
        {
            return array.AsSpan().Last();
        }

        // ===== Span_LastPredicate =====

        [BenchmarkCategory("Span_LastPredicate"), Benchmark(Baseline = true)]
        public int Span_LastPredicate_Manual()
        {
            var span = array.AsSpan();
            for (var i = span.Length - 1; i >= 0; i--)
            {
                if (span[i] % 2 == 0)
                    return span[i];
            }
            throw new InvalidOperationException();
        }

        [BenchmarkCategory("Span_LastPredicate"), Benchmark]
        public int Span_LastPredicate_Hyperlinq()
        {
            return array.AsSpan().Last(x => x % 2 == 0);
        }

        // ===== Where_Last =====

        [BenchmarkCategory("Where_Last"), Benchmark(Baseline = true)]
        public int Where_Last_LINQ()
        {
            return Enumerable.Last(Enumerable.Where(array, x => x % 2 == 0));
        }

        [BenchmarkCategory("Where_Last"), Benchmark]
        public int Where_Last_Hyperlinq_Array()
        {
            return array.Where(x => x % 2 == 0).Last();
        }

        [BenchmarkCategory("Where_Last"), Benchmark]
        public int Where_Last_Hyperlinq_Span()
        {
            return array.AsSpan().Where(x => x % 2 == 0).Last();
        }
    }
}
