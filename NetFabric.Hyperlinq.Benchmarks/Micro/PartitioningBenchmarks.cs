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
public class PartitioningBenchmarks
{
    int[] array = null!;
    List<int> list = null!;
    ReadOnlyMemory<int> memory;

    [Params(100, 1_000, 10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        array = ValueEnumerable.Range(0, Count).ToArray();
        list = array.ToList();
        memory = array.AsMemory();
    }

    // ===== Array_Skip =====

    [BenchmarkCategory("Array_Skip"), Benchmark(Baseline = true)]
    public int Array_Skip_LINQ()
    {
        var sum = 0;
        foreach (var item in Enumerable.Skip(array, Count / 4))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Array_Skip"), Benchmark]
    public int Array_Skip_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in array.AsSpan().Skip(Count / 4))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Array_Skip"), Benchmark]
    public int Array_Skip_Manual()
    {
        var sum = 0;
        var span = array.AsSpan().Slice(Count / 4);
        foreach (var item in span)
        {
            sum += item;
        }

        return sum;
    }

    // ===== Array_Take =====

    [BenchmarkCategory("Array_Take"), Benchmark(Baseline = true)]
    public int Array_Take_LINQ()
    {
        var sum = 0;
        foreach (var item in Enumerable.Take(array, Count / 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Array_Take"), Benchmark]
    public int Array_Take_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in array.AsSpan().Take(Count / 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Array_Take"), Benchmark]
    public int Array_Take_Manual()
    {
        var sum = 0;
        var span = array.AsSpan().Slice(0, Count / 2);
        foreach (var item in span)
        {
            sum += item;
        }

        return sum;
    }

    // ===== Array_SkipTake (Pagination) =====

    [BenchmarkCategory("Array_SkipTake"), Benchmark(Baseline = true)]
    public int Array_SkipTake_LINQ()
    {
        var sum = 0;
        foreach (var item in Enumerable.Take(Enumerable.Skip(array, Count / 4), Count / 4))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Array_SkipTake"), Benchmark]
    public int Array_SkipTake_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in array.AsSpan().Skip(Count / 4).Take(Count / 4))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Array_SkipTake"), Benchmark]
    public int Array_SkipTake_Manual()
    {
        var sum = 0;
        var span = array.AsSpan().Slice(Count / 4, Count / 4);
        foreach (var item in span)
        {
            sum += item;
        }

        return sum;
    }

    // ===== List_Skip =====

    [BenchmarkCategory("List_Skip"), Benchmark(Baseline = true)]
    public int List_Skip_LINQ()
    {
        var sum = 0;
        foreach (var item in Enumerable.Skip(list, Count / 4))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("List_Skip"), Benchmark]
    public int List_Skip_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in list.Skip(Count / 4))
        {
            sum += item;
        }

        return sum;
    }

    // ===== List_Take =====

    [BenchmarkCategory("List_Take"), Benchmark(Baseline = true)]
    public int List_Take_LINQ()
    {
        var sum = 0;
        foreach (var item in Enumerable.Take(list, Count / 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("List_Take"), Benchmark]
    public int List_Take_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in list.Take(Count / 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== List_SkipTake =====

    [BenchmarkCategory("List_SkipTake"), Benchmark(Baseline = true)]
    public int List_SkipTake_LINQ()
    {
        var sum = 0;
        foreach (var item in Enumerable.Take(Enumerable.Skip(list, Count / 4), Count / 4))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("List_SkipTake"), Benchmark]
    public int List_SkipTake_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in list.Skip(Count / 4).Take(Count / 4))
        {
            sum += item;
        }

        return sum;
    }

    // ===== Memory_Skip =====

    [BenchmarkCategory("Memory_Skip"), Benchmark(Baseline = true)]
    public int Memory_Skip_LINQ()
    {
        var sum = 0;
        foreach (var item in Enumerable.Skip(memory.ToArray(), Count / 4))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Memory_Skip"), Benchmark]
    public int Memory_Skip_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in memory.Skip(Count / 4).Span)
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Memory_Skip"), Benchmark]
    public int Memory_Skip_Span()
    {
        var sum = 0;
        foreach (var item in memory.Span.Slice(Count / 4))
        {
            sum += item;
        }

        return sum;
    }

    // ===== Memory_Take =====

    [BenchmarkCategory("Memory_Take"), Benchmark(Baseline = true)]
    public int Memory_Take_LINQ()
    {
        var sum = 0;
        foreach (var item in Enumerable.Take(memory.ToArray(), Count / 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Memory_Take"), Benchmark]
    public int Memory_Take_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in memory.Take(Count / 2).Span)
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("Memory_Take"), Benchmark]
    public int Memory_Take_Span()
    {
        var sum = 0;
        foreach (var item in memory.Span.Slice(0, Count / 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== Skip_ToArray =====

    [BenchmarkCategory("Skip_ToArray"), Benchmark(Baseline = true)]
    public int[] Skip_ToArray_LINQ() => Enumerable.ToArray(Enumerable.Skip(array, Count / 4));

    [BenchmarkCategory("Skip_ToArray"), Benchmark]
    public int[] Skip_ToArray_Hyperlinq() => array.AsSpan().Skip(Count / 4).ToArray();

    // ===== Take_ToArray =====

    [BenchmarkCategory("Take_ToArray"), Benchmark(Baseline = true)]
    public int[] Take_ToArray_LINQ() => Enumerable.ToArray(Enumerable.Take(array, Count / 2));

    [BenchmarkCategory("Take_ToArray"), Benchmark]
    public int[] Take_ToArray_Hyperlinq() => array.AsSpan().Take(Count / 2).ToArray();

    // ===== SkipTake_ToArray =====

    [BenchmarkCategory("SkipTake_ToArray"), Benchmark(Baseline = true)]
    public int[] SkipTake_ToArray_LINQ() => Enumerable.ToArray(Enumerable.Take(Enumerable.Skip(array, Count / 4), Count / 4));

    [BenchmarkCategory("SkipTake_ToArray"), Benchmark]
    public int[] SkipTake_ToArray_Hyperlinq() => array.AsSpan().Skip(Count / 4).Take(Count / 4).ToArray();

    // ===== ValueEnumerable_Skip =====

    [BenchmarkCategory("ValueEnumerable_Skip"), Benchmark(Baseline = true)]
    public int ValueEnumerable_Skip_LINQ()
    {
        var sum = 0;
        foreach (var item in Enumerable.Skip(array.AsValueEnumerable(), Count / 4))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("ValueEnumerable_Skip"), Benchmark]
    public int ValueEnumerable_Skip_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in array.AsValueEnumerable().Skip(Count / 4))
        {
            sum += item;
        }

        return sum;
    }

    // ===== ValueEnumerable_Take =====

    [BenchmarkCategory("ValueEnumerable_Take"), Benchmark(Baseline = true)]
    public int ValueEnumerable_Take_LINQ()
    {
        var sum = 0;
        foreach (var item in Enumerable.Take(array.AsValueEnumerable(), Count / 2))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("ValueEnumerable_Take"), Benchmark]
    public int ValueEnumerable_Take_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in array.AsValueEnumerable().Take(Count / 2))
        {
            sum += item;
        }

        return sum;
    }

    // ===== ValueEnumerable_SkipTake =====

    [BenchmarkCategory("ValueEnumerable_SkipTake"), Benchmark(Baseline = true)]
    public int ValueEnumerable_SkipTake_LINQ()
    {
        var sum = 0;
        foreach (var item in Enumerable.Take(Enumerable.Skip(array.AsValueEnumerable(), Count / 4), Count / 4))
        {
            sum += item;
        }

        return sum;
    }

    [BenchmarkCategory("ValueEnumerable_SkipTake"), Benchmark]
    public int ValueEnumerable_SkipTake_Hyperlinq()
    {
        var sum = 0;
        foreach (var item in array.AsValueEnumerable().Skip(Count / 4).Take(Count / 4))
        {
            sum += item;
        }

        return sum;
    }
}
