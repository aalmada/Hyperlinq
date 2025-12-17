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
public class SegmentedArrayBuilderBenchmarks
{
    int[] array = null!;
    List<int> list = null!;
    IEnumerable<int> enumerable = null!;

    [Params(10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        enumerable = PureRange(0, Count);
        array = enumerable.ToArray();
        list = enumerable.ToList();
    }

    static IEnumerable<int> PureRange(int start, int count)
    {
        for (var i = 0; i < count; i++)
            yield return start + i;
    }

    // ===== Array Where ToArray =====
 
     [BenchmarkCategory("Array_Where_ToArray"), Benchmark(Baseline = true)]
     public int[] Array_Where_ToArray_LINQ()
         => Enumerable.ToArray(Enumerable.Where(array, x => x % 2 == 0));
 
     [BenchmarkCategory("Array_Where_ToArray"), Benchmark]
     public int[] Array_Where_ToArray_Hyperlinq()
         => array.Where(x => x % 2 == 0).ToArray();

    // ===== Array WhereSelect ToArray =====

    [BenchmarkCategory("Array_WhereSelect_ToArray"), Benchmark(Baseline = true)]
    public int[] Array_WhereSelect_ToArray_LINQ()
        => Enumerable.ToArray(Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => x * 2));

    [BenchmarkCategory("Array_WhereSelect_ToArray"), Benchmark]
    public int[] Array_WhereSelect_ToArray_Hyperlinq()
        => array.Where(x => x % 2 == 0).Select(x => x * 2).ToArray();

    // ===== ArrayValueEnumerable Where ToArray =====

    [BenchmarkCategory("ArrayValueEnumerable_Where_ToArray"), Benchmark(Baseline = true)]
    public int[] ArrayValueEnumerable_Where_ToArray_LINQ()
        => Enumerable.ToArray(Enumerable.Where(array.AsValueEnumerable(), x => x % 2 == 0));

    [BenchmarkCategory("ArrayValueEnumerable_Where_ToArray"), Benchmark]
    public int[] ArrayValueEnumerable_Where_ToArray_Hyperlinq()
        => array.AsValueEnumerable().Where(x => x % 2 == 0).ToArray();

    // ===== ArrayValueEnumerable WhereSelect ToArray =====

    [BenchmarkCategory("ArrayValueEnumerable_WhereSelect_ToArray"), Benchmark(Baseline = true)]
    public int[] ArrayValueEnumerable_WhereSelect_ToArray_LINQ()
        => Enumerable.ToArray(Enumerable.Select(Enumerable.Where(array.AsValueEnumerable(), x => x % 2 == 0), x => x * 2));

    [BenchmarkCategory("ArrayValueEnumerable_WhereSelect_ToArray"), Benchmark]
    public int[] ArrayValueEnumerable_WhereSelect_ToArray_Hyperlinq()
        => array.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).ToArray();

    // ===== List Where ToArray =====

    [BenchmarkCategory("List_Where_ToArray"), Benchmark(Baseline = true)]
    public int[] List_Where_ToArray_LINQ()
        => Enumerable.ToArray(Enumerable.Where(list, x => x % 2 == 0));

    [BenchmarkCategory("List_Where_ToArray"), Benchmark]
    public int[] List_Where_ToArray_Hyperlinq()
        => list.Where(x => x % 2 == 0).ToArray();

    // ===== List WhereSelect ToArray =====

    [BenchmarkCategory("List_WhereSelect_ToArray"), Benchmark(Baseline = true)]
    public int[] List_WhereSelect_ToArray_LINQ()
        => Enumerable.ToArray(Enumerable.Select(Enumerable.Where(list, x => x % 2 == 0), x => x * 2));

    [BenchmarkCategory("List_WhereSelect_ToArray"), Benchmark]
    public int[] List_WhereSelect_ToArray_Hyperlinq()
        => list.Where(x => x % 2 == 0).Select(x => x * 2).ToArray();

    // ===== ListValueEnumerable Where ToArray =====

    [BenchmarkCategory("ListValueEnumerable_Where_ToArray"), Benchmark(Baseline = true)]
    public int[] ListValueEnumerable_Where_ToArray_LINQ()
        => Enumerable.ToArray(Enumerable.Where(list.AsValueEnumerable(), x => x % 2 == 0));

    [BenchmarkCategory("ListValueEnumerable_Where_ToArray"), Benchmark]
    public int[] ListValueEnumerable_Where_ToArray_Hyperlinq()
        => list.AsValueEnumerable().Where(x => x % 2 == 0).ToArray();

    // ===== ListValueEnumerable WhereSelect ToArray =====

    [BenchmarkCategory("ListValueEnumerable_WhereSelect_ToArray"), Benchmark(Baseline = true)]
    public int[] ListValueEnumerable_WhereSelect_ToArray_LINQ()
        => Enumerable.ToArray(Enumerable.Select(Enumerable.Where(list.AsValueEnumerable(), x => x % 2 == 0), x => x * 2));

    [BenchmarkCategory("ListValueEnumerable_WhereSelect_ToArray"), Benchmark]
    public int[] ListValueEnumerable_WhereSelect_ToArray_Hyperlinq()
        => list.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).ToArray();
    

    
    // ===== Enumerable ToArray (uses SegmentedArrayBuilder) =====

    [BenchmarkCategory("Enumerable_ToArray"), Benchmark(Baseline = true)]
    public int[] Enumerable_ToArray_LINQ()
        => Enumerable.ToArray(enumerable);

    [BenchmarkCategory("Enumerable_ToArray"), Benchmark]
    public int[] Enumerable_ToArray_Hyperlinq()
        => enumerable.AsValueEnumerable().ToArray();
    
    
    
    // ===== Enumerable Where ToArray (uses SegmentedArrayBuilder) =====

    [BenchmarkCategory("Enumerable_Where_ToArray"), Benchmark(Baseline = true)]
    public int[] Enumerable_Where_ToArray_LINQ()
        => Enumerable.ToArray(Enumerable.Where(enumerable, x => x % 2 == 0));

    [BenchmarkCategory("Enumerable_Where_ToArray"), Benchmark]
    public int[] Enumerable_Where_ToArray_Hyperlinq()
        => enumerable.AsValueEnumerable().Where(x => x % 2 == 0).ToArray();
    
    // ===== Enumerable WhereSelect ToArray (uses SegmentedArrayBuilder) =====

    [BenchmarkCategory("Enumerable_WhereSelect_ToArray"), Benchmark(Baseline = true)]
    public int[] Enumerable_WhereSelect_ToArray_LINQ()
        => Enumerable.ToArray(Enumerable.Select(Enumerable.Where(enumerable, x => x % 2 == 0), x => x * 2));

    [BenchmarkCategory("Enumerable_WhereSelect_ToArray"), Benchmark]
    public int[] Enumerable_WhereSelect_ToArray_Hyperlinq()
        => enumerable.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2).ToArray();
}
