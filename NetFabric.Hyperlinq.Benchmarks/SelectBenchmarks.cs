using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Hyperlinq.Benchmarks
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [MemoryDiagnoser]
    public class SelectBenchmarks
    {
        int[] array;
        List<int> list;

        [Params(10_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            array = Enumerable.Range(0, Count).ToArray();
            list = array.ToList();
        }

        [BenchmarkCategory("Array")]
        [Benchmark(Baseline = true)]
        public int Array_LINQ()
        {
            var sum = 0;
            foreach (var item in array.Select(item => item * 2))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("Array")]
        [Benchmark]
        public int Array_Hyperlinq()
        {
            var sum = 0;
            foreach (var item in array.AsValueEnumerable().Select(item => item * 2))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("List")]
        [Benchmark(Baseline = true)]
        public int List_LINQ()
        {
            var sum = 0;
            foreach (var item in list.Select(item => item * 2))
                sum += item;
            return sum;
        }

        [BenchmarkCategory("List")]
        [Benchmark]
        public int List_Hyperlinq()
        {
            var sum = 0;
            foreach (var item in list.AsValueEnumerable().Select(item => item * 2))
                sum += item;
            return sum;
        }
    }
}
