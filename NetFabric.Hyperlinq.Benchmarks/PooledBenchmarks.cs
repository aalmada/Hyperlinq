using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using NetFabric.Hyperlinq;

namespace NetFabric.Hyperlinq.Benchmarks;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class PooledBenchmarks
{
    int[] array = null!;
    List<int> list = null!;

    [Params(20, 10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        array = ValueEnumerable.Range(0, Count).ToArray();
        list = new List<int>(array);
    }

    [BenchmarkCategory("ToList"), Benchmark(Baseline = true)]
    public List<int> ToList_LINQ()
        => Enumerable.ToList(array);

    [BenchmarkCategory("ToList"), Benchmark]
    public List<int> ToList_Hyperlinq()
        => array.AsSpan().ToList();

    [BenchmarkCategory("Where_ToList"), Benchmark(Baseline = true)]
    public List<int> Where_ToList_LINQ()
        => Enumerable.ToList(Enumerable.Where(array, x => x % 2 == 0));

    [BenchmarkCategory("Where_ToList"), Benchmark]
    public List<int> Where_ToList_Hyperlinq()
        => array.AsSpan().Where(x => x % 2 == 0).ToList();

    // ===== List_Where_ToArray =====

    [BenchmarkCategory("List_Where_ToArray"), Benchmark(Baseline = true)]
    public int[] List_Where_ToArray_LINQ()
        => Enumerable.ToArray(Enumerable.Where(list, x => x % 2 == 0));

    [BenchmarkCategory("List_Where_ToArray"), Benchmark]
    public int[] List_Where_ToArray_Hyperlinq()
        => list.AsValueEnumerable().Where(x => x % 2 == 0).ToArray();

    [BenchmarkCategory("Range_ToList"), Benchmark(Baseline = true)]
    public List<int> Range_ToList_LINQ()
        => Enumerable.ToList(Enumerable.Range(0, Count));

    [BenchmarkCategory("Range_ToList"), Benchmark]
    public List<int> Range_ToList_Hyperlinq()
        => ValueEnumerable.Range(0, Count).ToList();

    [BenchmarkCategory("Select_ToList"), Benchmark(Baseline = true)]
    public List<int> Select_ToList_LINQ()
        => Enumerable.ToList(Enumerable.Select(array, x => x * 2));

    [BenchmarkCategory("Select_ToList"), Benchmark]
    public List<int> Select_ToList_Hyperlinq()
        => array.AsSpan().Select(x => x * 2).ToList();

    [BenchmarkCategory("WhereSelect_ToList"), Benchmark(Baseline = true)]
    public List<int> WhereSelect_ToList_LINQ()
        => Enumerable.ToList(Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => x * 2));

    [BenchmarkCategory("WhereSelect_ToList"), Benchmark]
    public List<int> WhereSelect_ToList_Hyperlinq()
        => array.AsSpan().Where(x => x % 2 == 0).Select(x => x * 2).ToList();
}
