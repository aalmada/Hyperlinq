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
public class ReadOnlyMemoryOperationsBenchmarks
{
    int[] array = null!;

    [Params(10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup() 
    {
        array = ValueEnumerable.Range(0, Count).ToArray();
    }


    // ===== ReadOnlyMemory_Select =====

    [BenchmarkCategory("ReadOnlyMemory_Select"), Benchmark]
    public int ReadOnlyMemory_Select_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in new ReadOnlyMemory<int>(array).Select(x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("ReadOnlyMemory_Select"), Benchmark(Baseline = true)]
    public int ReadOnlyMemory_Select_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Select(new ReadOnlyMemory<int>(array).ToArray(), x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== ReadOnlyMemory_Where =====

    [BenchmarkCategory("ReadOnlyMemory_Where"), Benchmark]
    public int ReadOnlyMemory_Where_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in new ReadOnlyMemory<int>(array).Where(x => x % 2 == 0))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("ReadOnlyMemory_Where"), Benchmark(Baseline = true)]
    public int ReadOnlyMemory_Where_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Where(new ReadOnlyMemory<int>(array).ToArray(), x => x % 2 == 0))
        {
            sum += item;
        }

        return sum;
    }

    // ===== ReadOnlyMemory_WhereSelect =====

    [BenchmarkCategory("ReadOnlyMemory_WhereSelect"), Benchmark]
    public int ReadOnlyMemory_WhereSelect_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in new ReadOnlyMemory<int>(array).Where(x => x % 2 == 0).Select(x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("ReadOnlyMemory_WhereSelect"), Benchmark(Baseline = true)]
    public int ReadOnlyMemory_WhereSelect_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(new ReadOnlyMemory<int>(array).ToArray(), x => x % 2 == 0), x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== ReadOnlyMemory_WhereSum =====

    [BenchmarkCategory("ReadOnlyMemory_WhereSum"), Benchmark]
    public int ReadOnlyMemory_WhereSum_Hyperlinq() => new ReadOnlyMemory<int>(array).Where(x => x % 2 == 0).Sum();

    [BenchmarkCategory("ReadOnlyMemory_WhereSum"), Benchmark(Baseline = true)]
    public int ReadOnlyMemory_WhereSum_Linq() => System.Linq.Enumerable.Sum(System.Linq.Enumerable.Where(new ReadOnlyMemory<int>(array).ToArray(), x => x % 2 == 0));

    // ===== ReadOnlyMemory_SelectCount =====

    [BenchmarkCategory("ReadOnlyMemory_SelectCount"), Benchmark]
    public int ReadOnlyMemory_SelectCount_Hyperlinq() => new ReadOnlyMemory<int>(array).Select(x => x * 2).Count();

    [BenchmarkCategory("ReadOnlyMemory_SelectCount"), Benchmark(Baseline = true)]
    public int ReadOnlyMemory_SelectCount_Linq() => System.Linq.Enumerable.Count(System.Linq.Enumerable.Select(new ReadOnlyMemory<int>(array).ToArray(), x => x * 2));

    // ===== ReadOnlyMemory_WhereToArray =====

    [BenchmarkCategory("ReadOnlyMemory_WhereToArray"), Benchmark]
    public int[] ReadOnlyMemory_WhereToArray_Hyperlinq() => new ReadOnlyMemory<int>(array).Where(x => x % 2 == 0).ToArray();

    [BenchmarkCategory("ReadOnlyMemory_WhereToArray"), Benchmark(Baseline = true)]
    public int[] ReadOnlyMemory_WhereToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Where(new ReadOnlyMemory<int>(array).ToArray(), x => x % 2 == 0));
}
