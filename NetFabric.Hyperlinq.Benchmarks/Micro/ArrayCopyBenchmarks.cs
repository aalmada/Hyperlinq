using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace NetFabric.Hyperlinq.Benchmarks;

/// <summary>
/// Benchmark comparing different array copy techniques:
/// - Array.Copy
/// - For loop with source.Length
/// - For loop with Math.Min(source.Length, destination.Length)
/// - For loop with local variables
/// - For loop using ref indexing
/// </summary>
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class ArrayCopyBenchmarks
{
    int[] source = null!;
    int[] destination = null!;

    [Params(10_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        source = new int[Count];
        for (var i = 0; i < Count; i++)
        {
            source[i] = i;
        }
        destination = new int[Count];
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        // Verify all methods produce the same result
        Array.Clear(destination);
    }

    // ===== Array.Copy =====

    [BenchmarkCategory("ArrayCopy"), Benchmark(Baseline = true)]
    public void ArrayCopy()
    {
        Array.Copy(source, destination, source.Length);
    }

    // ===== For Loop - Simple =====

    [BenchmarkCategory("ArrayCopy"), Benchmark]
    public void ForLoopSimple()
    {
        for (var i = 0; i < source.Length; i++)
        {
            destination[i] = source[i];
        }
    }

    // ===== For Loop - Check Both Lengths =====

    [BenchmarkCategory("ArrayCopy"), Benchmark]
    public void ForLoopBothLengths()
    {
        for (var i = 0; i < destination.Length && i < source.Length; i++)
        {
            destination[i] = source[i];
        }
    }

    // ===== For Loop - Local Variables =====

    [BenchmarkCategory("ArrayCopy"), Benchmark]
    public void ForLoopLocalVariables()
    {
        var src = source;
        var dst = destination;
        
        for (var i = 0; i < src.Length; i++)
        {
            dst[i] = src[i];
        }
    }

    // ===== For Loop - Local Variables with Both Lengths =====

    [BenchmarkCategory("ArrayCopy"), Benchmark]
    public void ForLoopLocalVariablesBothLengths()
    {
        var src = source;
        var dst = destination;
        
        for (var i = 0; i < dst.Length && i < src.Length; i++)
        {
            dst[i] = src[i];
        }
    }

    // ===== For Loop - Using (uint) Cast =====

    [BenchmarkCategory("ArrayCopy"), Benchmark]
    public void ForLoopUintCast()
    {
        for (var i = 0; (uint)i < (uint)source.Length; i++)
        {
            destination[i] = source[i];
        }
    }

    // ===== For Loop - Using (uint) Cast with Local Variables =====

    [BenchmarkCategory("ArrayCopy"), Benchmark]
    public void ForLoopUintCastLocalVariables()
    {
        var src = source;
        var dst = destination;
        var length = src.Length;
        
        for (var i = 0; (uint)i < (uint)length; i++)
        {
            dst[i] = src[i];
        }
    }

    // ===== For Loop - Using MemoryMarshal.GetReference =====

    [BenchmarkCategory("ArrayCopy"), Benchmark]
    public void ForLoopMemoryMarshalGetReference()
    {
        ref var srcRef = ref MemoryMarshal.GetReference(source.AsSpan());
        ref var dstRef = ref MemoryMarshal.GetReference(destination.AsSpan());
        
        for (var i = 0; i < source.Length; i++)
        {
            Unsafe.Add(ref dstRef, i) = Unsafe.Add(ref srcRef, i);
        }
    }

    // ===== Span-based Copy =====

    [BenchmarkCategory("ArrayCopy"), Benchmark]
    public void SpanCopy()
    {
        source.AsSpan().CopyTo(destination);
    }

    // ===== Span-based Copy with Slice =====

    [BenchmarkCategory("ArrayCopy"), Benchmark]
    public void SpanCopySlice()
    {
        source.AsSpan(0, source.Length).CopyTo(destination.AsSpan(0, destination.Length));
    }
}
