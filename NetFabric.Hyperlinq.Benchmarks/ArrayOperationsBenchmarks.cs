using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using NetFabric.Hyperlinq;

namespace NetFabric.Hyperlinq.Benchmarks;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ArrayOperationsBenchmarks
{
    int[] array = null!;

    [Params(10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup() 
    {
        array = ValueEnumerable.Range(0, Count).ToArray();
    }


    // ===== Array_Select =====

    [BenchmarkCategory("Array_Select"), Benchmark]
    public int Array_Select_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in array.AsValueEnumerable().Select(x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Array_Select"), Benchmark(Baseline = true)]
    public int Array_Select_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Select(array, x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== Array_Where =====

    [BenchmarkCategory("Array_Where"), Benchmark]
    public int Array_Where_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in array.AsValueEnumerable().Where(x => x % 2 == 0))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Array_Where"), Benchmark(Baseline = true)]
    public int Array_Where_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Where(array, x => x % 2 == 0))
        {
            sum += item;
        }

        return sum;
    }

    // ===== Array_WhereSelect =====

    [BenchmarkCategory("Array_WhereSelect"), Benchmark]
    public int Array_WhereSelect_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in array.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Array_WhereSelect"), Benchmark(Baseline = true)]
    public int Array_WhereSelect_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(array, x => x % 2 == 0), x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== Array_WhereSum =====

    [BenchmarkCategory("Array_WhereSum"), Benchmark]
    public int Array_WhereSum_Hyperlinq() => array.AsValueEnumerable().Where(x => x % 2 == 0).Sum();

    [BenchmarkCategory("Array_WhereSum"), Benchmark(Baseline = true)]
    public int Array_WhereSum_Linq() => System.Linq.Enumerable.Sum(System.Linq.Enumerable.Where(array, x => x % 2 == 0));

    // ===== Array_WhereSelectSum =====

    [BenchmarkCategory("Array_WhereSelectSum"), Benchmark]
    public int Array_WhereSelectSum_Hyperlinq() => array.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();

    [BenchmarkCategory("Array_WhereSelectSum"), Benchmark(Baseline = true)]
    public int Array_WhereSelectSum_Linq() => System.Linq.Enumerable.Sum(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(array, x => x % 2 == 0), x => x * 2));

    // ===== Array_SelectCount =====

    [BenchmarkCategory("Array_SelectCount"), Benchmark]
    public int Array_SelectCount_Hyperlinq() => array.AsValueEnumerable().Select(x => x * 2).Count();

    [BenchmarkCategory("Array_SelectCount"), Benchmark(Baseline = true)]
    public int Array_SelectCount_Linq() => System.Linq.Enumerable.Count(System.Linq.Enumerable.Select(array, x => x * 2));

    // ===== Array_WhereCount =====

    [BenchmarkCategory("Array_WhereCount"), Benchmark]
    public int Array_WhereCount_Hyperlinq() => array.AsValueEnumerable().Where(x => x % 2 == 0).Count();

    [BenchmarkCategory("Array_WhereCount"), Benchmark(Baseline = true)]
    public int Array_WhereCount_Linq() => System.Linq.Enumerable.Count(System.Linq.Enumerable.Where(array, x => x % 2 == 0));

    // ===== Array_SelectToArray =====

    [BenchmarkCategory("Array_SelectToArray"), Benchmark]
    public int[] Array_SelectToArray_Hyperlinq() => array.AsValueEnumerable().Select(x => x * 2).ToArray();

    [BenchmarkCategory("Array_SelectToArray"), Benchmark(Baseline = true)]
    public int[] Array_SelectToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(array, x => x * 2));

    // ===== Array_WhereToArray =====

    [BenchmarkCategory("Array_WhereToArray"), Benchmark]
    public int[] Array_WhereToArray_Hyperlinq() => array.AsValueEnumerable().Where(x => x % 2 == 0).ToArray();

    [BenchmarkCategory("Array_WhereToArray"), Benchmark(Baseline = true)]
    public int[] Array_WhereToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Where(array, x => x % 2 == 0));

    // ===== Array_WhereSelectToArray =====

    [BenchmarkCategory("Array_WhereSelectToArray"), Benchmark]
    public int[] Array_WhereSelectToArray_Hyperlinq() => array.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).ToArray();

    [BenchmarkCategory("Array_WhereSelectToArray"), Benchmark(Baseline = true)]
    public int[] Array_WhereSelectToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(array, x => x % 2 == 0), x => x * 2));
}
