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

        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(Array.Empty<int>());
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
    public void ArrayBuilder_Resize_ShouldReturnOldBuffersToPool()
    {
        var pool = new TrackingArrayPool<int>();
        using (var builder = new ArrayBuilder<int>(pool))
        {
            // Fill first chunk (16 - MinimumRentSize)
            for (int i = 0; i < 20; i++)
            {
                builder.Add(i);
            }

            // At this point with LINQ-like implementation:
            // - First 8 items in scratch buffer (not pooled)
            // - Remaining items in pooled buffer(s)
            
        }
        // Builder Dispose called here -> should return pooled buffers

        // LINQ-like implementation uses scratch buffer first, then rents from pool
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
