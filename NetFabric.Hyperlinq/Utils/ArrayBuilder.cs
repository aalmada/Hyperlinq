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
ref struct ArrayBuilder<T>
{
    const int ScratchBufferSize = 8;
    const int MinimumRentSize = 16;

    /// <summary>
    /// Stack-allocated scratch buffer for small collections.
    /// </summary>
    [InlineArray(ScratchBufferSize)]
    public struct ScratchBuffer
    {
        private T _element0;
    }

    readonly ArrayPool<T> pool;
    Span<T> firstSegment;
    Span<T> currentSegment;
    BackboneArrays segments;
    int segmentsCount;
    int countInFinishedSegments;
    int countInCurrentSegment;

    [InlineArray(32)]
    struct BackboneArrays
    {
        private T[] _element0;
    }

    /// <summary>
    /// Initialize the builder with a scratch buffer.
    /// </summary>
    /// <param name="pool">The array pool to use for renting arrays.</param>
    /// <param name="scratchBuffer">A stack-allocated buffer for small collections.</param>
    public ArrayBuilder(ArrayPool<T> pool, Span<T> scratchBuffer)
    {
        this.pool = pool;
        firstSegment = scratchBuffer;
        currentSegment = scratchBuffer;
        segments = default;
        segmentsCount = 0;
        countInFinishedSegments = 0;
        countInCurrentSegment = 0;
    }

    /// <summary>
    /// Initialize the builder with a specific capacity (for known-size operations).
    /// </summary>
    /// <param name="pool">The array pool to use for renting arrays.</param>
    /// <param name="capacity">The initial capacity.</param>
    public ArrayBuilder(ArrayPool<T> pool, int capacity)
    {
        this.pool = pool;
        var buffer = pool.Rent(Math.Max(capacity, MinimumRentSize));
        firstSegment = buffer;
        currentSegment = buffer;
        segments = default;
        segments[0] = buffer;
        segmentsCount = 0; // Will be set to 1 on first Expand
        countInFinishedSegments = 0;
        countInCurrentSegment = 0;
    }

    /// <summary>
    /// Initialize the builder with a pool and default capacity.
    /// </summary>
    /// <param name="pool">The array pool to use for renting arrays.</param>
    public ArrayBuilder(ArrayPool<T> pool)
        : this(pool, MinimumRentSize)
    {
    }

    /// <summary>
    /// Initialize the builder with ArrayPool.Shared and a specific capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity.</param>
    public ArrayBuilder(int capacity)
        : this(ArrayPool<T>.Shared, capacity)
    {
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
        Span<T> currentSegment = this.currentSegment;
        int countInCurrentSegment = this.countInCurrentSegment;
        if ((uint)countInCurrentSegment < (uint)currentSegment.Length)
        {
            currentSegment[countInCurrentSegment] = item;
            this.countInCurrentSegment++;
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

    /// <summary>
    /// Adds filtered and projected items from a source span.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<TSource, TPredicate, TSelector>(ReadOnlySpan<TSource> source, in TPredicate predicate, in TSelector selector)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, T>
    {
        Span<T> currentSegment = this.currentSegment;
        int countInCurrentSegment = this.countInCurrentSegment;

        foreach (var item in source)
        {
            if (predicate.Invoke(item))
            {
                if ((uint)countInCurrentSegment < (uint)currentSegment.Length)
                {
                    currentSegment[countInCurrentSegment] = selector.Invoke(item);
                    countInCurrentSegment++;
                }
                else
                {
                    this.countInCurrentSegment = countInCurrentSegment;
                    Expand();
                    currentSegment = this.currentSegment;
                    currentSegment[0] = selector.Invoke(item);
                    countInCurrentSegment = 1;
                }
            }
        }

        this.countInCurrentSegment = countInCurrentSegment;
    }

    /// <summary>
    /// Adds filtered items from a source span.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<TPredicate>(ReadOnlySpan<T> source, in TPredicate predicate)
        where TPredicate : struct, IFunction<T, bool>
    {
        Span<T> currentSegment = this.currentSegment;
        int countInCurrentSegment = this.countInCurrentSegment;

        foreach (var item in source)
        {
            if (predicate.Invoke(item))
            {
                if ((uint)countInCurrentSegment < (uint)currentSegment.Length)
                {
                    currentSegment[countInCurrentSegment] = item;
                    countInCurrentSegment++;
                }
                else
                {
                    this.countInCurrentSegment = countInCurrentSegment;
                    Expand();
                    currentSegment = this.currentSegment;
                    currentSegment[0] = item;
                    countInCurrentSegment = 1;
                }
            }
        }

        this.countInCurrentSegment = countInCurrentSegment;
    }

    /// <summary>
    /// Adds filtered items from a source span using IFunctionIn.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddIn<TPredicate>(ReadOnlySpan<T> source, in TPredicate predicate)
        where TPredicate : struct, IFunctionIn<T, bool>
    {
        Span<T> currentSegment = this.currentSegment;
        int countInCurrentSegment = this.countInCurrentSegment;

        foreach (ref readonly var item in source)
        {
            if (predicate.Invoke(in item))
            {
                if ((uint)countInCurrentSegment < (uint)currentSegment.Length)
                {
                    currentSegment[countInCurrentSegment] = item;
                    countInCurrentSegment++;
                }
                else
                {
                    this.countInCurrentSegment = countInCurrentSegment;
                    Expand();
                    currentSegment = this.currentSegment;
                    currentSegment[0] = item;
                    countInCurrentSegment = 1;
                }
            }
        }

        this.countInCurrentSegment = countInCurrentSegment;
    }

    /// <summary>
    /// Adds a range of items from an enumerable source.
    /// Optimized for arrays, lists, and collections.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddRange(IEnumerable<T> source)
    {
        if (source is ICollection<T> collection)
        {
            int collectionCount = collection.Count;
            if (collectionCount == 0)
            {
                return;
            }

            // Try span-based bulk copy first (fastest path)
            if (TryGetSpan(source, out ReadOnlySpan<T> sourceSpan))
            {
                AddSpan(sourceSpan);
                return;
            }

            // Try ICollection.CopyTo (fast path for collections)
            if (TryAddCollection(collection, collectionCount))
            {
                return;
            }
        }

        // Fallback to enumeration (slowest path)
        AddEnumerable(source);
    }

    /// <summary>
    /// Tries to get a span from common collection types.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool TryGetSpan(IEnumerable<T> source, out ReadOnlySpan<T> span)
    {
        if (source is T[] array)
        {
            span = array;
            return true;
        }

        if (source is List<T> list)
        {
            span = CollectionsMarshal.AsSpan(list);
            return true;
        }

        span = default;
        return false;
    }

    /// <summary>
    /// Adds items from a span using bulk copy operations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void AddSpan(ReadOnlySpan<T> sourceSpan)
    {
        int available = currentSegment.Length - countInCurrentSegment;
        int toCopy = Math.Min(available, sourceSpan.Length);
        
        // Copy what fits in current segment
        ReadOnlySpan<T> firstPart = sourceSpan.Slice(0, toCopy);
        firstPart.CopyTo(currentSegment.Slice(countInCurrentSegment));
        countInCurrentSegment += toCopy;

        // Handle remaining items
        ReadOnlySpan<T> remaining = sourceSpan.Slice(toCopy);
        if (!remaining.IsEmpty)
        {
            Expand(remaining.Length);
            remaining.CopyTo(currentSegment);
            countInCurrentSegment = remaining.Length;
        }
    }

    /// <summary>
    /// Tries to add items from an ICollection using CopyTo.
    /// </summary>
    bool TryAddCollection(ICollection<T> collection, int collectionCount)
    {
        // Can only use CopyTo if we're not using a scratch buffer with remaining space
        bool currentSegmentIsScratchBufferWithRemainingSpace = 
            segmentsCount == 0 && countInCurrentSegment < currentSegment.Length;
        
        if (currentSegmentIsScratchBufferWithRemainingSpace)
        {
            return false; // Can't use CopyTo with scratch buffer
        }

        int remainingSpace = currentSegment.Length - countInCurrentSegment;

        // If no space remaining, expand and copy to new segment
        if (remainingSpace == 0)
        {
            Expand(collectionCount);
            collection.CopyTo(segments[segmentsCount - 1], 0);
            countInCurrentSegment = collectionCount;
            return true;
        }

        // If enough space remaining, copy to current segment
        if (collectionCount <= remainingSpace)
        {
            collection.CopyTo(segments[segmentsCount - 1], countInCurrentSegment);
            countInCurrentSegment += collectionCount;
            return true;
        }

        // Not enough space and can't split - fallback to enumeration
        return false;
    }

    /// <summary>
    /// Adds items from an enumerable using optimized enumeration.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void AddEnumerable(IEnumerable<T> source)
    {
        Span<T> currentSegment = this.currentSegment;
        int countInCurrentSegment = this.countInCurrentSegment;

        foreach (T item in source)
        {
            if ((uint)countInCurrentSegment < (uint)currentSegment.Length)
            {
                currentSegment[countInCurrentSegment] = item;
                countInCurrentSegment++;
            }
            else
            {
                this.countInCurrentSegment = countInCurrentSegment;
                Expand();
                currentSegment = this.currentSegment;
                currentSegment[0] = item;
                countInCurrentSegment = 1;
            }
        }

        this.countInCurrentSegment = countInCurrentSegment;
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
        int newSize = (int)Math.Min(
            Math.Max(minimumRequired, currentLength * 2L),
            Array.MaxLength
        );
        
        T[] newArray = pool.Rent(newSize);
        segments[segmentsCount++] = newArray;
        currentSegment = newArray;
    }

    /// <summary>
    /// Creates an array containing all elements in the builder.
    /// </summary>
    public readonly T[] ToArray()
    {
        int count = Count;
        if (count == 0)
        {
            return [];
        }

        T[] result = GC.AllocateUninitializedArray<T>(count);
        ToSpan(result);
        return result;
    }

    /// <summary>
    /// Creates a list containing all elements in the builder.
    /// </summary>
    public readonly List<T> ToList()
    {
        int count = Count;
        if (count == 0)
        {
            return [];
        }

        List<T> result = new(count);
        CollectionsMarshal.SetCount(result, count);
        ToSpan(CollectionsMarshal.AsSpan(result));
        return result;
    }


    readonly void ToSpan(Span<T> destination)
    {
        if (segmentsCount != 0)
        {
            // Copy first segment (scratch buffer)
            firstSegment.CopyTo(destination);
            destination = destination.Slice(firstSegment.Length);

            // Copy intermediate segments (all but last)
            ReadOnlySpan<T[]> segmentSpan = segments;
            for (int i = 0; i < segmentsCount - 1; i++)
            {
                T[] segment = segmentSpan[i];
                segment.CopyTo(destination);
                destination = destination.Slice(segment.Length);
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
            pool.Return(segments[0], RuntimeHelpers.IsReferenceOrContainsReferences<T>());
        }

        // Return additional segments if we expanded
        if (segmentsCount > 0)
        {
            ReadOnlySpan<T[]> segmentSpan = segments;
            for (int i = 1; i < segmentsCount; i++) // Start at 1, we already returned segments[0]
            {
                pool.Return(segmentSpan[i], RuntimeHelpers.IsReferenceOrContainsReferences<T>());
            }
        }
    }
}
