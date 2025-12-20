using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using NetFabric.Hyperlinq;

namespace NetFabric.Hyperlinq.Benchmarks;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ReadOnlySpanOperationsBenchmarks
{
    int[] array = null!;

    [Params(10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup() 
    {
        array = ValueEnumerable.Range(0, Count).ToArray();
    }


    // ===== Span_Select =====

    [BenchmarkCategory("Span_Select"), Benchmark]
    public int Span_Select_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in array.AsSpan().Select(x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Span_Select"), Benchmark(Baseline = true)]
    public int Span_Select_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Select(array, x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== Span_Where =====

    [BenchmarkCategory("Span_Where"), Benchmark]
    public int Span_Where_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in array.AsSpan().Where(x => x % 2 == 0))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Span_Where"), Benchmark(Baseline = true)]
    public int Span_Where_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Where(array, x => x % 2 == 0))
        {
            sum += item;
        }

        return sum;
    }

    // ===== Span_WhereSelect =====

    [BenchmarkCategory("Span_WhereSelect"), Benchmark]
    public int Span_WhereSelect_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in array.AsSpan().Where(x => x % 2 == 0).Select(x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Span_WhereSelect"), Benchmark(Baseline = true)]
    public int Span_WhereSelect_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(array, x => x % 2 == 0), x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== Span_WhereSum =====

    [BenchmarkCategory("Span_WhereSum"), Benchmark]
    public int Span_WhereSum_Hyperlinq() => array.AsSpan().Where(x => x % 2 == 0).Sum();

    [BenchmarkCategory("Span_WhereSum"), Benchmark(Baseline = true)]
    public int Span_WhereSum_Linq() => System.Linq.Enumerable.Sum(System.Linq.Enumerable.Where(array, x => x % 2 == 0));

    // ===== Span_WhereSelectSum =====

    [BenchmarkCategory("Span_WhereSelectSum"), Benchmark]
    public int Span_WhereSelectSum_Hyperlinq() => array.AsSpan().Where(x => x % 2 == 0).Select(x => x * 2).Sum();

    [BenchmarkCategory("Span_WhereSelectSum"), Benchmark(Baseline = true)]
    public int Span_WhereSelectSum_Linq() => System.Linq.Enumerable.Sum(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(array, x => x % 2 == 0), x => x * 2));

    // ===== Span_SelectCount =====

    [BenchmarkCategory("Span_SelectCount"), Benchmark]
    public int Span_SelectCount_Hyperlinq() => array.AsSpan().Select(x => x * 2).Count();

    [BenchmarkCategory("Span_SelectCount"), Benchmark(Baseline = true)]
    public int Span_SelectCount_Linq() => System.Linq.Enumerable.Count(System.Linq.Enumerable.Select(array, x => x * 2));

    // ===== Span_WhereCount =====

    [BenchmarkCategory("Span_WhereCount"), Benchmark]
    public int Span_WhereCount_Hyperlinq() => array.AsSpan().Where(x => x % 2 == 0).Count();

    [BenchmarkCategory("Span_WhereCount"), Benchmark(Baseline = true)]
    public int Span_WhereCount_Linq() => System.Linq.Enumerable.Count(System.Linq.Enumerable.Where(array, x => x % 2 == 0));

    // ===== Span_SelectToArray =====

    [BenchmarkCategory("Span_SelectToArray"), Benchmark]
    public int[] Span_SelectToArray_Hyperlinq() => array.AsSpan().Select(x => x * 2).ToArray();

    [BenchmarkCategory("Span_SelectToArray"), Benchmark(Baseline = true)]
    public int[] Span_SelectToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(array, x => x * 2));

    // ===== Span_WhereToArray =====

    [BenchmarkCategory("Span_WhereToArray"), Benchmark]
    public int[] Span_WhereToArray_Hyperlinq() => array.AsSpan().Where(x => x % 2 == 0).ToArray();

    [BenchmarkCategory("Span_WhereToArray"), Benchmark(Baseline = true)]
    public int[] Span_WhereToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Where(array, x => x % 2 == 0));

    // ===== Span_WhereSelectToArray =====

    [BenchmarkCategory("Span_WhereSelectToArray"), Benchmark]
    public int[] Span_WhereSelectToArray_Hyperlinq() => array.AsSpan().Where(x => x % 2 == 0).Select(x => x * 2).ToArray();

    [BenchmarkCategory("Span_WhereSelectToArray"), Benchmark(Baseline = true)]
    public int[] Span_WhereSelectToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(array, x => x % 2 == 0), x => x * 2));
}
