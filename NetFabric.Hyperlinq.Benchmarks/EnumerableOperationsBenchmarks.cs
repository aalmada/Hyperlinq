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
public class EnumerableOperationsBenchmarks
{
    IEnumerable<int> enumerable = null!;

    [Params(10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup() 
    {
        enumerable = CustomRange(Count);
    }

    static IEnumerable<int> CustomRange(int count)
    {
        for (var i = 0; i < count; i++)
            yield return i;
    }

    // ===== Enumerable_Select =====

    [BenchmarkCategory("Enumerable_Select"), Benchmark]
    public int Enumerable_Select_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in enumerable.AsValueEnumerable().Select(x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Enumerable_Select"), Benchmark(Baseline = true)]
    public int Enumerable_Select_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Select(enumerable, x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== Enumerable_Where =====

    [BenchmarkCategory("Enumerable_Where"), Benchmark]
    public int Enumerable_Where_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in enumerable.AsValueEnumerable().Where(x => x % 2 == 0))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Enumerable_Where"), Benchmark(Baseline = true)]
    public int Enumerable_Where_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Where(enumerable, x => x % 2 == 0))
        {
            sum += item;
        }

        return sum;
    }

    // ===== Enumerable_WhereSelect =====

    [BenchmarkCategory("Enumerable_WhereSelect"), Benchmark]
    public int Enumerable_WhereSelect_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in enumerable.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Enumerable_WhereSelect"), Benchmark(Baseline = true)]
    public int Enumerable_WhereSelect_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(enumerable, x => x % 2 == 0), x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== Enumerable_WhereSum =====

    [BenchmarkCategory("Enumerable_WhereSum"), Benchmark]
    public int Enumerable_WhereSum_Hyperlinq() => enumerable.AsValueEnumerable().Where(x => x % 2 == 0).Sum();

    [BenchmarkCategory("Enumerable_WhereSum"), Benchmark(Baseline = true)]
    public int Enumerable_WhereSum_Linq() => System.Linq.Enumerable.Sum(System.Linq.Enumerable.Where(enumerable, x => x % 2 == 0));

    // ===== Enumerable_WhereSelectSum =====

    [BenchmarkCategory("Enumerable_WhereSelectSum"), Benchmark]
    public int Enumerable_WhereSelectSum_Hyperlinq() => enumerable.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();

    [BenchmarkCategory("Enumerable_WhereSelectSum"), Benchmark(Baseline = true)]
    public int Enumerable_WhereSelectSum_Linq() => System.Linq.Enumerable.Sum(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(enumerable, x => x % 2 == 0), x => x * 2));

    // ===== Enumerable_SelectCount =====

    [BenchmarkCategory("Enumerable_SelectCount"), Benchmark]
    public int Enumerable_SelectCount_Hyperlinq() => enumerable.AsValueEnumerable().Select(x => x * 2).Count();

    [BenchmarkCategory("Enumerable_SelectCount"), Benchmark(Baseline = true)]
    public int Enumerable_SelectCount_Linq() => System.Linq.Enumerable.Count(System.Linq.Enumerable.Select(enumerable, x => x * 2));

    // ===== Enumerable_WhereCount =====

    [BenchmarkCategory("Enumerable_WhereCount"), Benchmark]
    public int Enumerable_WhereCount_Hyperlinq() => enumerable.AsValueEnumerable().Where(x => x % 2 == 0).Count();

    [BenchmarkCategory("Enumerable_WhereCount"), Benchmark(Baseline = true)]
    public int Enumerable_WhereCount_Linq() => System.Linq.Enumerable.Count(System.Linq.Enumerable.Where(enumerable, x => x % 2 == 0));

    // ===== Enumerable_SelectToArray =====

    [BenchmarkCategory("Enumerable_SelectToArray"), Benchmark]
    public int[] Enumerable_SelectToArray_Hyperlinq() => enumerable.AsValueEnumerable().Select(x => x * 2).ToArray();

    [BenchmarkCategory("Enumerable_SelectToArray"), Benchmark(Baseline = true)]
    public int[] Enumerable_SelectToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(enumerable, x => x * 2));

    // ===== Enumerable_WhereToArray =====

    [BenchmarkCategory("Enumerable_WhereToArray"), Benchmark]
    public int[] Enumerable_WhereToArray_Hyperlinq() => enumerable.AsValueEnumerable().Where(x => x % 2 == 0).ToArray();

    [BenchmarkCategory("Enumerable_WhereToArray"), Benchmark(Baseline = true)]
    public int[] Enumerable_WhereToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Where(enumerable, x => x % 2 == 0));

    // ===== Enumerable_WhereSelectToArray =====

    [BenchmarkCategory("Enumerable_WhereSelectToArray"), Benchmark]
    public int[] Enumerable_WhereSelectToArray_Hyperlinq() => enumerable.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).ToArray();

    [BenchmarkCategory("Enumerable_WhereSelectToArray"), Benchmark(Baseline = true)]
    public int[] Enumerable_WhereSelectToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(enumerable, x => x % 2 == 0), x => x * 2));
}
