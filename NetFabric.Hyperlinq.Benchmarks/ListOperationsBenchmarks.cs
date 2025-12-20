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
public class ListOperationsBenchmarks
{
    List<int> list = null!;

    [Params(10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup() 
    {
        list = ValueEnumerable.Range(0, Count).ToList();
    }

    // ===== List_Select =====

    [BenchmarkCategory("List_Select"), Benchmark]
    public int List_Select_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in list.AsValueEnumerable().Select(x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("List_Select"), Benchmark(Baseline = true)]
    public int List_Select_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Select(list, x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== List_Where =====

    [BenchmarkCategory("List_Where"), Benchmark]
    public int List_Where_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in list.AsValueEnumerable().Where(x => x % 2 == 0))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("List_Where"), Benchmark(Baseline = true)]
    public int List_Where_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Where(list, x => x % 2 == 0))
        {
            sum += item;
        }

        return sum;
    }

    // ===== List_WhereSelect =====

    [BenchmarkCategory("List_WhereSelect"), Benchmark]
    public int List_WhereSelect_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in list.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("List_WhereSelect"), Benchmark(Baseline = true)]
    public int List_WhereSelect_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(list, x => x % 2 == 0), x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== List_WhereSum =====

    [BenchmarkCategory("List_WhereSum"), Benchmark]
    public int List_WhereSum_Hyperlinq() => list.AsValueEnumerable().Where(x => x % 2 == 0).Sum();

    [BenchmarkCategory("List_WhereSum"), Benchmark(Baseline = true)]
    public int List_WhereSum_Linq() => System.Linq.Enumerable.Sum(System.Linq.Enumerable.Where(list, x => x % 2 == 0));

    // ===== List_WhereSelectSum =====

    [BenchmarkCategory("List_WhereSelectSum"), Benchmark]
    public int List_WhereSelectSum_Hyperlinq() => list.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();

    [BenchmarkCategory("List_WhereSelectSum"), Benchmark(Baseline = true)]
    public int List_WhereSelectSum_Linq() => System.Linq.Enumerable.Sum(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(list, x => x % 2 == 0), x => x * 2));

    // ===== List_SelectCount =====

    [BenchmarkCategory("List_SelectCount"), Benchmark]
    public int List_SelectCount_Hyperlinq() => list.AsValueEnumerable().Select(x => x * 2).Count();

    [BenchmarkCategory("List_SelectCount"), Benchmark(Baseline = true)]
    public int List_SelectCount_Linq() => System.Linq.Enumerable.Count(System.Linq.Enumerable.Select(list, x => x * 2));

    // ===== List_WhereCount =====

    [BenchmarkCategory("List_WhereCount"), Benchmark]
    public int List_WhereCount_Hyperlinq() => list.AsValueEnumerable().Where(x => x % 2 == 0).Count();

    [BenchmarkCategory("List_WhereCount"), Benchmark(Baseline = true)]
    public int List_WhereCount_Linq() => System.Linq.Enumerable.Count(System.Linq.Enumerable.Where(list, x => x % 2 == 0));

    // ===== List_SelectToArray =====

    [BenchmarkCategory("List_SelectToArray"), Benchmark]
    public int[] List_SelectToArray_Hyperlinq() => list.AsValueEnumerable().Select(x => x * 2).ToArray();

    [BenchmarkCategory("List_SelectToArray"), Benchmark(Baseline = true)]
    public int[] List_SelectToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(list, x => x * 2));

    // ===== List_WhereToArray =====

    [BenchmarkCategory("List_WhereToArray"), Benchmark]
    public int[] List_WhereToArray_Hyperlinq() => list.AsValueEnumerable().Where(x => x % 2 == 0).ToArray();

    [BenchmarkCategory("List_WhereToArray"), Benchmark(Baseline = true)]
    public int[] List_WhereToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Where(list, x => x % 2 == 0));

    // ===== List_WhereSelectToArray =====

    [BenchmarkCategory("List_WhereSelectToArray"), Benchmark]
    public int[] List_WhereSelectToArray_Hyperlinq() => list.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).ToArray();

    [BenchmarkCategory("List_WhereSelectToArray"), Benchmark(Baseline = true)]
    public int[] List_WhereSelectToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(list, x => x % 2 == 0), x => x * 2));
}
