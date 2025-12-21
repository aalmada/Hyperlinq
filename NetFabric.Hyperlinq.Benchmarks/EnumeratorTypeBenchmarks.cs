using System;
using System.Collections;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace NetFabric.Hyperlinq.Benchmarks;

/// <summary>
/// Benchmark comparing performance of different enumerator types:
/// - Reference-type IEnumerator&lt;T&gt; (class-based)
/// - Value-type IEnumerator&lt;T&gt; (struct-based)
/// - Value-type duck-typed enumerator (only Current and MoveNext)
/// </summary>
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class EnumeratorTypeBenchmarks
{
    int[] data = null!;

    [Params(10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        data = new int[Count];
        for (var i = 0; i < Count; i++)
        {
            data[i] = i;
        }
    }

    // ===== Reference Type IEnumerator<T> =====

    [BenchmarkCategory("Enumeration"), Benchmark]
    public int ReferenceTypeEnumerator()
    {
        var sum = 0;
        foreach (var item in new ReferenceEnumerable(data))
        {
            sum += item;
        }
        return sum;
    }

    // ===== Value Type IEnumerator<T> =====

    [BenchmarkCategory("Enumeration"), Benchmark]
    public int ValueTypeEnumerator()
    {
        var sum = 0;
        foreach (var item in new ValueTypeEnumerable(data))
        {
            sum += item;
        }
        return sum;
    }

    // ===== Duck-Typed Value Type Enumerator =====

    [BenchmarkCategory("Enumeration"), Benchmark(Baseline = true)]
    public int DuckTypedEnumerator()
    {
        var sum = 0;
        foreach (var item in new DuckTypedEnumerable(data))
        {
            sum += item;
        }
        return sum;
    }

    // ===== Manual Enumeration (for comparison) =====

    [BenchmarkCategory("Enumeration"), Benchmark]
    public int ForEnumeration()
    {
        var sum = 0;
        for (var i = 0; i < data.Length; i++)
        {
            sum += data[i];
        }
        return sum;
    }

    [BenchmarkCategory("Enumeration"), Benchmark]
    public int ForeachEnumeration()
    {
        var sum = 0;
        foreach (var item in data)
        {
            sum += item;
        }
        return sum;
    }
}

// ===== Reference-Type IEnumerator<T> Implementation =====

public sealed class ReferenceEnumerable : IEnumerable<int>
{
    readonly int[] data;

    public ReferenceEnumerable(int[] data)
    {
        this.data = data;
    }

    public IEnumerator<int> GetEnumerator() => new ReferenceEnumerator(data);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    sealed class ReferenceEnumerator : IEnumerator<int>
    {
        readonly int[] data;
        int index;

        public ReferenceEnumerator(int[] data)
        {
            this.data = data;
            index = -1;
        }

        public int Current => data[index];

        object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            index++;
            return index < data.Length;
        }

        public void Reset() => index = -1;

        public void Dispose() { }
    }
}

// ===== Value-Type IEnumerator<T> Implementation =====

public readonly struct ValueTypeEnumerable : IEnumerable<int>
{
    readonly int[] data;

    public ValueTypeEnumerable(int[] data)
    {
        this.data = data;
    }

    public ValueTypeEnumerator GetEnumerator() => new(data);

    IEnumerator<int> IEnumerable<int>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct ValueTypeEnumerator : IEnumerator<int>
    {
        readonly int[] data;
        int index;

        public ValueTypeEnumerator(int[] data)
        {
            this.data = data;
            index = -1;
        }

        public readonly int Current => data[index];

        readonly object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            index++;
            return index < data.Length;
        }

        public void Reset() => index = -1;

        public readonly void Dispose() { }
    }
}

// ===== Duck-Typed Value Type Enumerator (no interface) =====

public readonly struct DuckTypedEnumerable
{
    readonly int[] data;

    public DuckTypedEnumerable(int[] data)
    {
        this.data = data;
    }

    public DuckTypedEnumerator GetEnumerator() => new(data);

    public struct DuckTypedEnumerator
    {
        readonly int[] data;
        int index;

        public DuckTypedEnumerator(int[] data)
        {
            this.data = data;
            index = -1;
        }

        public int Current => data[index];

        public bool MoveNext()
        {
            index++;
            return index < data.Length;
        }
    }
}
