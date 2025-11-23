using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using System.Collections.Generic;
using NetFabric.Hyperlinq;

namespace NetFabric.Hyperlinq.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<AsValueEnumerableBenchmarks>();
        }
    }
}
