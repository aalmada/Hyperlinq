using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace NetFabric.Hyperlinq.Benchmarks;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class MinMaxBenchmarks
{
    private int[] array = null!;
    private List<int> list = null!;

    [Params(100, 1000, 10000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        array = ValueEnumerable.Range(0, Count).ToArray();
        list = new List<int>(array);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Array_Min")]
    public int Array_LINQ_Min()
        => array.Min();

    [Benchmark]
    [BenchmarkCategory("Array_Min")]
    public int Array_Hyperlinq_Min()
        => array.AsValueEnumerable().Min();

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Array_Max")]
    public int Array_LINQ_Max()
        => array.Max();

    [Benchmark]
    [BenchmarkCategory("Array_Max")]
    public int Array_Hyperlinq_Max()
        => array.AsValueEnumerable().Max();

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("List_Min")]
    public int List_LINQ_Min()
        => list.Min();

    [Benchmark]
    [BenchmarkCategory("List_Min")]
    public int List_Hyperlinq_Min()
        => list.AsValueEnumerable().Min();

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("List_Max")]
    public int List_LINQ_Max()
        => list.Max();

    [Benchmark]
    [BenchmarkCategory("List_Max")]
    public int List_Hyperlinq_Max()
        => list.AsValueEnumerable().Max();
}
