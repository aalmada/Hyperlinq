using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests;

public class PooledBufferTests
{
    [Test]
    public void ReadOnlySpan_ToArrayPooled_EmptySpan_ShouldReturnEmptyBuffer()
    {
        var span = ReadOnlySpan<int>.Empty;

        using var buffer = span.ToArrayPooled();

        _ = buffer.Length.Must().BeEqualTo(0);
        _ = buffer.AsSpan().Length.Must().BeEqualTo(0);
    }

    [Test]
    public void ReadOnlySpan_ToArrayPooled_SingleElement_ShouldReturnCorrectBuffer()
    {
        var array = new[] { 42 };
        var span = array.AsSpan();

        using var buffer = span.ToArrayPooled();

        _ = buffer.Length.Must().BeEqualTo(1);
        _ = buffer.AsSpan()[0].Must().BeEqualTo(42);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ReadOnlySpan_ToArrayPooled_ShouldMatchOriginal(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var span = array.AsSpan();

        using var buffer = span.ToArrayPooled();

        _ = buffer.Length.Must().BeEqualTo(array.Length);
        _ = buffer.AsSpan().ToArray().Must().BeEnumerableOf<int>().BeEqualTo(array);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void ReadOnlySpan_ToArrayPooled_WithPredicate_ShouldFilterCorrectly(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        var span = array.AsSpan();
        var predicate = (int x) => x % 2 == 0;

        using var buffer = span.ToArrayPooled(predicate);

        var expected = array.Where(predicate).ToArray();
        _ = buffer.Length.Must().BeEqualTo(expected.Length);
        _ = buffer.AsSpan().ToArray().Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }

    [Test]
    public void ReadOnlySpan_ToArrayPooled_WithPredicate_LargeCollection_ShouldGrowBuffer()
    {
        // Create array larger than initial capacity (4) to test growth
        var array = Enumerable.Range(0, 100).ToArray();
        var span = array.AsSpan();
        var predicate = (int x) => x % 2 == 0;

        using var buffer = span.ToArrayPooled(predicate);

        var expected = array.Where(predicate).ToArray();
        _ = buffer.Length.Must().BeEqualTo(expected.Length);
        _ = buffer.AsSpan().ToArray().Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }



    [Test]
    public void WhereListEnumerable_ToArrayPooled_ShouldFilterCorrectly()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var whereEnumerable = list.AsValueEnumerable().Where(x => x % 2 == 0);

        using var buffer = whereEnumerable.ToArrayPooled();

        var expected = list.Where(x => x % 2 == 0).ToArray();
        _ = buffer.Length.Must().BeEqualTo(expected.Length);
        _ = buffer.AsSpan().ToArray().Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }



    [Test]
    public void WhereReadOnlySpanEnumerable_ToArrayPooled_ShouldFilterCorrectly()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var span = array.AsSpan();
        var whereEnumerable = span.Where(x => x > 5);

        using var buffer = whereEnumerable.ToArrayPooled();

        var expected = array.Where(x => x > 5).ToArray();
        _ = buffer.Length.Must().BeEqualTo(expected.Length);
        _ = buffer.AsSpan().ToArray().Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }

    [Test]
    public void WhereSelectReadOnlySpanEnumerable_ToArrayPooled_ShouldFilterAndProjectCorrectly()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var span = array.AsSpan();
        var whereSelectEnumerable = span.Where(x => x % 2 == 0).Select(x => x * 3);

        using var buffer = whereSelectEnumerable.ToArrayPooled();

        var expected = array.Where(x => x % 2 == 0).Select(x => x * 3).ToArray();
        _ = buffer.Length.Must().BeEqualTo(expected.Length);
        _ = buffer.AsSpan().ToArray().Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }

    [Test]
    public void PooledBuffer_ToArray_ShouldCreateIndependentCopy()
    {
        var array = new[] { 1, 2, 3 };
        var span = array.AsSpan();

        using var buffer = span.ToArrayPooled();
        var copy = buffer.ToArray();

        // Modify copy
        copy[0] = 999;

        // Original buffer should be unchanged
        _ = buffer.AsSpan()[0].Must().BeEqualTo(1);
    }

    [Test]
    public void PooledBuffer_Dispose_ShouldAllowReuse()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var span = array.AsSpan();

        // Create and dispose buffer
        var buffer = span.ToArrayPooled();
        buffer.Dispose();

        // Create another buffer - should potentially reuse the same pooled array
        using var buffer2 = span.ToArrayPooled();
        _ = buffer2.Length.Must().BeEqualTo(array.Length);
    }

    [Test]
    public void PooledBuffer_WithReferenceTypes_ShouldClearOnDispose()
    {
        var pool = new TrackingArrayPool<string>();
        var array = new[] { "a", "b", "c" };
        var span = array.AsSpan();

        var buffer = span.ToArrayPooled(pool);
        buffer.Dispose();

        _ = pool.Returns.Count.Must().BeEqualTo(1);
        _ = pool.Returns[0].clearArray.Must().BeTrue();
    }

    [Test]
    public void PooledBuffer_WithCustomPool_ShouldUseSpecifiedPool()
    {
        // Create a custom pool
        var customPool = ArrayPool<int>.Create(maxArrayLength: 1024, maxArraysPerBucket: 10);

        var array = new[] { 1, 2, 3, 4, 5 };
        var span = array.AsSpan();

        // Use custom pool
        using var buffer = span.ToArrayPooled(customPool);

        _ = buffer.Length.Must().BeEqualTo(array.Length);
        _ = buffer.AsSpan().ToArray().Must().BeEnumerableOf<int>().BeEqualTo(array);

        // Buffer will be returned to custom pool on dispose
    }

    [Test]
    public void PooledBuffer_WithCustomPool_AndPredicate_ShouldUseSpecifiedPool()
    {
        var customPool = ArrayPool<int>.Create();
        var array = Enumerable.Range(0, 50).ToArray();
        var span = array.AsSpan();
        var predicate = (int x) => x % 2 == 0;

        using var buffer = span.ToArrayPooled(predicate, customPool);

        var expected = array.Where(predicate).ToArray();
        _ = buffer.Length.Must().BeEqualTo(expected.Length);
        _ = buffer.AsSpan().ToArray().Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }

    [Test]
    public void PooledBuffer_LargeCollection_ShouldHandleGrowth()
    {
        // Create a large collection that will require multiple buffer growths
        var array = Enumerable.Range(0, 1000).ToArray();
        var span = array.AsSpan();
        var predicate = (int x) => x % 3 == 0; // ~333 elements

        using var buffer = span.ToArrayPooled(predicate);

        var expected = array.Where(predicate).ToArray();
        _ = buffer.Length.Must().BeEqualTo(expected.Length);
        _ = buffer.AsSpan().ToArray().Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }
}
