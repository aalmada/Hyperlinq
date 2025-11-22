using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NetFabric.Hyperlinq.Benchmarks.Baseline;
using System.Collections.Generic;
using System.Linq;

namespace NetFabric.Hyperlinq.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmarks>();
        }
    }

    [MemoryDiagnoser]
    public class Benchmarks
    {
        IEnumerable<int> source;

        [Params(10, 10_000, 1_000_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            source = Enumerable.Range(0, Count).ToList();
        }

        [Benchmark(Baseline = true)]
        public int Linq_Where()
        {
            return LinqBenchmarks.Wheresum(source);
        }

        [Benchmark]
        public int Hyperlinq_Where()
        {
            var sum = 0;
            foreach (var item in source.Where(x => x % 2 == 0))
            {
                sum += item;
            }
            return sum;
        }

        [Benchmark]
        public int Linq_Select()
        {
            return LinqBenchmarks.Selectsum(source);
        }

        [Benchmark]
        public int Hyperlinq_Select()
        {
            var sum = 0;
            foreach (var item in source.Select(x => x * 2))
            {
                sum += item;
            }
            return sum;
        }
    }
}
