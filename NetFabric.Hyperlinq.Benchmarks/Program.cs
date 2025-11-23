using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using NetFabric.Hyperlinq.Benchmarks.Baseline;
using System.Collections.Generic;
using NetFabric.Hyperlinq;

namespace NetFabric.Hyperlinq.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmarks>();
        }
    }

    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [MemoryDiagnoser]
    public class Benchmarks
    {
        IEnumerable<int> enumerableSource;
        List<int> listSource;

        [Params(10, 10_000, 1_000_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            listSource = System.Linq.Enumerable.Range(0, Count).ToList();
            enumerableSource = listSource;
        }

        [BenchmarkCategory("IEnumerable"), Benchmark(Baseline = true)]
        public int Linq_Where_IEnumerable()
        {
            return LinqBenchmarks.WhereSum(enumerableSource);
        }

        [BenchmarkCategory("IEnumerable"), Benchmark]
        public int Hyperlinq_Where_IEnumerable()
        {
            var sum = 0;
            foreach (var item in enumerableSource.Where(x => x % 2 == 0))
            {
                sum += item;
            }
            return sum;
        }

        [BenchmarkCategory("List"), Benchmark(Baseline = true)]
        public int Linq_Where_List()
        {
            return LinqBenchmarks.WhereSum(listSource);
        }

        [BenchmarkCategory("List"), Benchmark]
        public int Hyperlinq_Where_List()
        {
            var sum = 0;
            foreach (var item in listSource.Where(x => x % 2 == 0))
            {
                sum += item;
            }
            return sum;
        }

        [BenchmarkCategory("IEnumerable"), Benchmark]
        public int Linq_Select_IEnumerable()
        {
            return LinqBenchmarks.SelectSum(enumerableSource);
        }

        [BenchmarkCategory("IEnumerable"), Benchmark]
        public int Hyperlinq_Select_IEnumerable()
        {
            var sum = 0;
            foreach (var item in enumerableSource.Select(x => x * 2))
            {
                sum += item;
            }
            return sum;
        }

        [BenchmarkCategory("List"), Benchmark]
        public int Linq_Select_List()
        {
            return LinqBenchmarks.SelectSum(listSource);
        }

        [BenchmarkCategory("List"), Benchmark]
        public int Hyperlinq_Select_List()
        {
            var sum = 0;
            foreach (var item in listSource.Select(x => x * 2))
            {
                sum += item;
            }
            return sum;
        }
    }
}
