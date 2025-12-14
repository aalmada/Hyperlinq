using System;
using System.Buffers;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests;

public class ArrayBuilderTests
{
    [Test]
    public void ArrayBuilder_Empty_ShouldReturnEmpty()
    {
        using var builder = new ArrayBuilder<int>(ArrayPool<int>.Shared);

        var result = builder.ToArray();
        var pooledBuffer = builder.ToPooledBuffer();

        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(Array.Empty<int>());
        _ = pooledBuffer.Length.Must().BeEqualTo(0);
        pooledBuffer.Dispose();
    }

    [Test]
    public void ArrayBuilder_SingleChunk_ShouldReturnCorrectData()
    {
        using var builder = new ArrayBuilder<int>(ArrayPool<int>.Shared);
        builder.Add(1);
        builder.Add(2);
        builder.Add(3);

        var result = builder.ToArray();

        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 1, 2, 3 });
    }

    [Test]
    public void ArrayBuilder_MultipleChunks_ShouldReturnCorrectData()
    {
        using var builder = new ArrayBuilder<int>(ArrayPool<int>.Shared);
        // Default capacity is 4. Add 10 items to force growth.
        var input = Enumerable.Range(0, 10).ToArray();
        foreach (var item in input)
        {
            builder.Add(item);
        }

        var result = builder.ToArray();

        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(input);
    }

    [Test]
    public void ArrayBuilder_ToPooledBuffer_ShouldReturnCorrectDataAndTransferOwnership()
    {
        var pool = new TrackingArrayPool<int>();
        using (var builder = new ArrayBuilder<int>(pool))
        {
            builder.Add(1);
            builder.Add(2);

            using var buffer = builder.ToPooledBuffer();

            _ = buffer.Length.Must().BeEqualTo(2);
            _ = buffer.AsSpan().ToArray().Must().BeEnumerableOf<int>().BeEqualTo(new[] { 1, 2 });

            // Verify internal pool interactions
            // 1 Rent for initial capacity
            _ = pool.Rents.Count.Must().BeEqualTo(1);
        }
        // Builder Dispose called here

        // Since ToPooledBuffer transfers ownership (for single chunk), builder.Dispose shouldn't return the buffer.
        // But the buffer itself WILL return it when disposed.
        // Wait, inside ToPooledBuffer for single chunk:
        // "return new PooledBuffer<T>(bufferToReturn, totalCount, pool);"
        // So buffer.Dispose() returns it.

        _ = pool.Returns.Count.Must().BeEqualTo(1); // Only the buffer.Dispose() should have returned it
    }

    [Test]
    public void ArrayBuilder_Resize_ShouldReturnOldBuffersToPool()
    {
        var pool = new TrackingArrayPool<int>();
        using (var builder = new ArrayBuilder<int>(pool))
        {
            // Fill first chunk (4)
            builder.Add(1); builder.Add(2); builder.Add(3); builder.Add(4);
            // Trigger resize (new capacity 8)
            builder.Add(5);

            // At this point:
            // Rents: [4, 8]
            // Returns: [] (previous buffers kept in `previousBuffers` list until Dispose or ToPooledBuffer)

            _ = pool.Rents.Count.Must().BeEqualTo(2);
            _ = pool.Rents[0].minimumLength.Must().BeEqualTo(4);
            _ = pool.Rents[1].minimumLength.Must().BeEqualTo(8);
            _ = pool.Returns.Count.Must().BeEqualTo(0);
        }
        // Builder Dispose called here -> should return both chunks

        _ = pool.Returns.Count.Must().BeEqualTo(2);
        _ = pool.Returns.Any(r => r.array.Length == 4).Must().BeTrue();
        _ = pool.Returns.Any(r => r.array.Length == 8).Must().BeTrue();
    }

    [Test]
    public void ArrayBuilder_Dispose_ValueTypes_ShouldNotClearArray()
    {
        var pool = new TrackingArrayPool<int>();
        using (var builder = new ArrayBuilder<int>(pool))
        {
            builder.Add(1);
        }

        _ = pool.Returns.Count.Must().BeEqualTo(1);
        _ = pool.Returns[0].clearArray.Must().BeFalse();
    }

    [Test]
    public void ArrayBuilder_Dispose_ReferenceTypes_ShouldClearArray()
    {
        var pool = new TrackingArrayPool<string>();
        using (var builder = new ArrayBuilder<string>(pool))
        {
            builder.Add("test");
        }

        _ = pool.Returns.Count.Must().BeEqualTo(1);
        _ = pool.Returns[0].clearArray.Must().BeTrue();
    }
    [Test]
    public void ArrayBuilder_ToList_ShouldReturnCorrectData()
    {
        using var builder = new ArrayBuilder<int>(ArrayPool<int>.Shared);
        builder.Add(1);
        builder.Add(2);
        builder.Add(3);

        var result = builder.ToList();

        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 1, 2, 3 });
        _ = result.GetType().Must().BeEqualTo(typeof(List<int>));
    }

    [Test]
    public void ArrayBuilder_ToList_MultipleChunks_ShouldReturnCorrectData()
    {
        using var builder = new ArrayBuilder<int>(ArrayPool<int>.Shared);
        // Default capacity is 4. Add 10 items to force growth.
        var input = Enumerable.Range(0, 10).ToArray();
        foreach (var item in input)
        {
            builder.Add(item);
        }

        var result = builder.ToList();

        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(input);
        _ = result.GetType().Must().BeEqualTo(typeof(List<int>));
    }
}
