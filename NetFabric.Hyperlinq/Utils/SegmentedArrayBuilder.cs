using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

/// <summary>
/// A builder for arrays that uses pooled buffers to minimize allocations.
/// Uses a LINQ-like pattern with scratch buffer and simple per-item adds.
/// </summary>
/// <typeparam name="T">The type of elements in the array.</typeparam>
ref struct SegmentedArrayBuilder<T>
{
    const int ScratchBufferSize = 8;
    const int MinimumRentSize = 16;

    /// <summary>
    /// Stack-allocated scratch buffer for small collections.
    /// </summary>
    [InlineArray(ScratchBufferSize)]
    public struct ScratchBuffer
    {
        T _element0;
    }

    // readonly ArrayPool<T> pool; // Use ArrayPool<T>.Shared instead
    Span<T> firstSegment;
    Span<T> currentSegment;
    int segmentsCount;
    int countInFinishedSegments;
    int countInCurrentSegment;
    BackboneArrays segments;

    [InlineArray(32)]
    struct BackboneArrays
    {
        T[] _element0;
    }


    /// <summary>
    /// Initialize the builder with a scratch buffer.
    /// </summary>
    /// <param name="scratchBuffer">A stack-allocated buffer for small collections.</param>
    public SegmentedArrayBuilder(Span<T> scratchBuffer)
    {
        // this.pool = pool; // Use ArrayPool<T>.Shared instead
        firstSegment = scratchBuffer;
        currentSegment = scratchBuffer;
        segments = default;
        segmentsCount = 0;
        countInFinishedSegments = 0;
        countInCurrentSegment = 0;
    }

    /// <summary>
    /// Gets the total number of elements in the builder.
    /// </summary>
    public readonly int Count => countInFinishedSegments + countInCurrentSegment;

    /// <summary>
    /// Adds an item to the builder.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item)
    {
        if ((uint)countInCurrentSegment < (uint)currentSegment.Length)
        {
            Unsafe.Add(ref MemoryMarshal.GetReference(currentSegment), countInCurrentSegment++) = item;
        }
        else
        {
            AddSlow(item);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    void AddSlow(T item)
    {
        Expand();
        currentSegment[0] = item;
        countInCurrentSegment = 1;
    }




    [MethodImpl(MethodImplOptions.NoInlining)]
    void Expand() => Expand(MinimumRentSize);

    [MethodImpl(MethodImplOptions.NoInlining)]
    void Expand(int minimumRequired)
    {
        if (minimumRequired < MinimumRentSize)
        {
            minimumRequired = MinimumRentSize;
        }

        // Track finished segments
        int currentLength;
        if (segmentsCount == 0)
        {
            // First expansion
            countInFinishedSegments = firstSegment.Length;
            currentLength = firstSegment.Length;
            
            // Check if we're expanding from a capacity-based constructor (segments[0] already has the first buffer)
            if (segments[0] is not null)
            {
                // Capacity-based: segments[0] is the first buffer, add new buffer at segments[1]
                segmentsCount = 1;
            }
            // else: Scratch buffer: segments[0] will get the new buffer
        }
        else
        {
            // Subsequent expansions
            currentLength = currentSegment.Length;
            checked { countInFinishedSegments += currentLength; }
        }

        // Check for overflow
        if (countInFinishedSegments > Array.MaxLength)
        {
            throw new OutOfMemoryException();
        }

        // Use doubling algorithm with min/max constraints (matching LINQ)
        var newSize = (int)long.Min(
            long.Max(minimumRequired, currentLength * 2L),
            Array.MaxLength
        );
        
        var newArray = ArrayPool<T>.Shared.Rent(newSize);
        segments[segmentsCount++] = newArray;
        currentSegment = newArray;
    }

    /// <summary>
    /// Creates an array containing all elements in the builder.
    /// </summary>
    public readonly T[] ToArray()
    {
        var count = Count;
        if (count == 0)
        {
            return [];
        }

        var result = GC.AllocateUninitializedArray<T>(count);
        ToSpanInlined(result);
        return result;
    }

    /// <summary>
    /// Creates a list containing all elements in the builder.
    /// </summary>
    public readonly List<T> ToList()
    {
        var count = Count;
        if (count == 0)
        {
            return [];
        }

        List<T> result = new(count);
        CollectionsMarshal.SetCount(result, count);
        ToSpanInlined(CollectionsMarshal.AsSpan(result));
        return result;
    }





    /// <summary>
    /// Populates the destination span with all of the elements in the builder.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    readonly void ToSpanInlined(Span<T> destination)
    {
        var segmentsCount = this.segmentsCount;
        if (segmentsCount != 0)
        {
            // Copy first segment (scratch buffer)
            ReadOnlySpan<T> firstSegment = this.firstSegment;
            firstSegment.CopyTo(destination);
            destination = destination.Slice(firstSegment.Length);

            // Copy intermediate segments (all but last)
            segmentsCount--;
            if (segmentsCount != 0)
            {
                ReadOnlySpan<T[]> segmentSpan = segments;
                for (var i = 0; i < segmentsCount; i++)
                {
                    var segment = segmentSpan[i];
                    segment.CopyTo(destination);
                    destination = destination.Slice(segment.Length);
                }
            }

            // Copy last segment (only used portion)
            currentSegment.Slice(0, countInCurrentSegment).CopyTo(destination);
        }
        else
        {
            // Only first segment used (either scratch buffer or single pooled buffer)
            currentSegment.Slice(0, countInCurrentSegment).CopyTo(destination);
        }
    }

    /// <summary>
    /// Disposes the builder, returning all rented arrays to the pool.
    /// </summary>
    public void Dispose()
    {
        // Return the first buffer if it's a pooled array (not scratch)
        if (segments[0] is not null)
        {
            ArrayPool<T>.Shared.Return(segments[0], RuntimeHelpers.IsReferenceOrContainsReferences<T>());
        }

        // Return additional segments if we expanded
        if (segmentsCount > 0)
        {
            ReadOnlySpan<T[]> segmentSpan = segments;
            for (var i = 1; i < segmentsCount; i++) // Start at 1, we already returned segments[0]
            {
                ArrayPool<T>.Shared.Return(segmentSpan[i], RuntimeHelpers.IsReferenceOrContainsReferences<T>());
            }
        }
    }
}
