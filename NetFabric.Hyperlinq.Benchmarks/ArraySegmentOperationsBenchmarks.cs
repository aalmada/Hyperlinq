using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using NetFabric.Hyperlinq;

namespace NetFabric.Hyperlinq.Benchmarks;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ArraySegmentOperationsBenchmarks
{
    int[] array = null!; // Keep reference to array to prevent GC
    ArraySegment<int> segment;

    [Params(10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup() 
    {
        array = ValueEnumerable.Range(0, Count).ToArray();
        segment = new ArraySegment<int>(array);
    }

    // ===== ArraySegment_Select =====

    [BenchmarkCategory("ArraySegment_Select"), Benchmark]
    public int ArraySegment_Select_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in segment.AsValueEnumerable().Select(x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("ArraySegment_Select"), Benchmark(Baseline = true)]
    public int ArraySegment_Select_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Select(segment, x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== ArraySegment_Where =====

    [BenchmarkCategory("ArraySegment_Where"), Benchmark]
    public int ArraySegment_Where_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in segment.AsValueEnumerable().Where(x => x % 2 == 0))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("ArraySegment_Where"), Benchmark(Baseline = true)]
    public int ArraySegment_Where_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Where(segment, x => x % 2 == 0))
        {
            sum += item;
        }

        return sum;
    }

    // ===== ArraySegment_WhereSelect =====

    [BenchmarkCategory("ArraySegment_WhereSelect"), Benchmark]
    public int ArraySegment_WhereSelect_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in segment.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("ArraySegment_WhereSelect"), Benchmark(Baseline = true)]
    public int ArraySegment_WhereSelect_Linq()
    {
        var sum = 0;
        foreach (var item in System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(segment, x => x % 2 == 0), x => x * 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== ArraySegment_WhereSum =====

    [BenchmarkCategory("ArraySegment_WhereSum"), Benchmark]
    public int ArraySegment_WhereSum_Hyperlinq() => segment.AsValueEnumerable().Where(x => x % 2 == 0).Sum();

    [BenchmarkCategory("ArraySegment_WhereSum"), Benchmark(Baseline = true)]
    public int ArraySegment_WhereSum_Linq() => System.Linq.Enumerable.Sum(System.Linq.Enumerable.Where(segment, x => x % 2 == 0));

    // ===== ArraySegment_WhereSelectSum =====

    [BenchmarkCategory("ArraySegment_WhereSelectSum"), Benchmark]
    public int ArraySegment_WhereSelectSum_Hyperlinq() => segment.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).Sum();

    [BenchmarkCategory("ArraySegment_WhereSelectSum"), Benchmark(Baseline = true)]
    public int ArraySegment_WhereSelectSum_Linq() => System.Linq.Enumerable.Sum(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(segment, x => x % 2 == 0), x => x * 2));

    // ===== ArraySegment_SelectCount =====

    [BenchmarkCategory("ArraySegment_SelectCount"), Benchmark]
    public int ArraySegment_SelectCount_Hyperlinq() => segment.AsValueEnumerable().Select(x => x * 2).Count();

    [BenchmarkCategory("ArraySegment_SelectCount"), Benchmark(Baseline = true)]
    public int ArraySegment_SelectCount_Linq() => System.Linq.Enumerable.Count(System.Linq.Enumerable.Select(segment, x => x * 2));

    // ===== ArraySegment_WhereCount =====

    [BenchmarkCategory("ArraySegment_WhereCount"), Benchmark]
    public int ArraySegment_WhereCount_Hyperlinq() => segment.AsValueEnumerable().Where(x => x % 2 == 0).Count();

    [BenchmarkCategory("ArraySegment_WhereCount"), Benchmark(Baseline = true)]
    public int ArraySegment_WhereCount_Linq() => System.Linq.Enumerable.Count(System.Linq.Enumerable.Where(segment, x => x % 2 == 0));

    // ===== ArraySegment_SelectToArray =====

    [BenchmarkCategory("ArraySegment_SelectToArray"), Benchmark]
    public int[] ArraySegment_SelectToArray_Hyperlinq() => segment.AsValueEnumerable().Select(x => x * 2).ToArray();

    [BenchmarkCategory("ArraySegment_SelectToArray"), Benchmark(Baseline = true)]
    public int[] ArraySegment_SelectToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(segment, x => x * 2));

    // ===== ArraySegment_WhereToArray =====

    [BenchmarkCategory("ArraySegment_WhereToArray"), Benchmark]
    public int[] ArraySegment_WhereToArray_Hyperlinq() => segment.AsValueEnumerable().Where(x => x % 2 == 0).ToArray();

    [BenchmarkCategory("ArraySegment_WhereToArray"), Benchmark(Baseline = true)]
    public int[] ArraySegment_WhereToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Where(segment, x => x % 2 == 0));

    // ===== ArraySegment_WhereSelectToArray =====

    [BenchmarkCategory("ArraySegment_WhereSelectToArray"), Benchmark]
    public int[] ArraySegment_WhereSelectToArray_Hyperlinq() => segment.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).ToArray();

    [BenchmarkCategory("ArraySegment_WhereSelectToArray"), Benchmark(Baseline = true)]
    public int[] ArraySegment_WhereSelectToArray_Linq() => System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(System.Linq.Enumerable.Where(segment, x => x % 2 == 0), x => x * 2));
}
