using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

/// <summary>
/// A builder for arrays that uses pooled buffers to minimize allocations.
/// </summary>
/// <typeparam name="T">The type of elements in the array.</typeparam>
internal ref struct ArrayBuilder<T>
{
    const int DefaultCapacity = 4;
    const int MaxSubArrays = 32; // Enough for 4 billion elements with doubling strategy

    readonly ArrayPool<T> pool;
    T[] currentBuffer;
    int currentCount;
    BackboneArrays previousBuffers;
    int previousBuffersCount;
    int totalCount;

    [InlineArray(MaxSubArrays)]
    struct BackboneArrays
    {
        private T[] _element0;
    }

    public ArrayBuilder(ArrayPool<T> pool)
    {
        this.pool = pool;
        currentBuffer = pool.Rent(DefaultCapacity);
        currentCount = 0;
        previousBuffers = default;
        previousBuffersCount = 0;
        totalCount = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item)
    {
        if ((uint)currentCount >= (uint)currentBuffer.Length)
        {
            Grow();
        }

        ref var destination = ref MemoryMarshal.GetArrayDataReference(currentBuffer);
        Unsafe.Add(ref destination, currentCount++) = item;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where TPredicate : struct, IFunction<T, bool>
    {
        var count = currentCount;
        var buffer = currentBuffer;
        var length = buffer.Length;
        ref var destination = ref MemoryMarshal.GetArrayDataReference(buffer);

        foreach (var item in source)
        {
            if (predicate.Invoke(item))
            {
                if ((uint)count >= (uint)length)
                {
                    currentCount = count;
                    Grow();
                    buffer = currentBuffer;
                    length = buffer.Length;
                    destination = ref MemoryMarshal.GetArrayDataReference(buffer);
                    count = 0;
                }

                Unsafe.Add(ref destination, count++) = item;
            }
        }

        currentCount = count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<TSource, TPredicate, TSelector>(ReadOnlySpan<TSource> source, in TPredicate predicate, in TSelector selector)
        where TPredicate : struct, IFunction<TSource, bool>
        where TSelector : struct, IFunction<TSource, T>
    {
        var count = currentCount;
        var buffer = currentBuffer;
        var length = buffer.Length;
        ref var destination = ref MemoryMarshal.GetArrayDataReference(buffer);

        foreach (var item in source)
        {
            if (predicate.Invoke(item))
            {
                if ((uint)count >= (uint)length)
                {
                    currentCount = count;
                    Grow();
                    buffer = currentBuffer;
                    length = buffer.Length;
                    destination = ref MemoryMarshal.GetArrayDataReference(buffer);
                    count = 0;
                }

                Unsafe.Add(ref destination, count++) = selector.Invoke(item);
            }
        }

        currentCount = count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddFunc<TPredicate>(ReadOnlySpan<T> source, in TPredicate predicate)
        where TPredicate : struct, IFunction<T, bool>
    {
        var count = currentCount;
        var buffer = currentBuffer;
        var length = buffer.Length;
        ref var destination = ref MemoryMarshal.GetArrayDataReference(buffer);

        foreach (var item in source)
        {
            if (predicate.Invoke(item))
            {
                if ((uint)count >= (uint)length)
                {
                    currentCount = count;
                    Grow();
                    buffer = currentBuffer;
                    length = buffer.Length;
                    destination = ref MemoryMarshal.GetArrayDataReference(buffer);
                    count = 0;
                }

                Unsafe.Add(ref destination, count++) = item;
            }
        }

        currentCount = count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<TPredicate>(ReadOnlySpan<T> source, in TPredicate predicate)
        where TPredicate : struct, IFunctionIn<T, bool>
    {
        var count = currentCount;
        var buffer = currentBuffer;
        var length = buffer.Length;
        ref var destination = ref MemoryMarshal.GetArrayDataReference(buffer);

        foreach (ref readonly var item in source)
        {
            if (predicate.Invoke(in item))
            {
                if ((uint)count >= (uint)length)
                {
                    currentCount = count;
                    Grow();
                    buffer = currentBuffer;
                    length = buffer.Length;
                    destination = ref MemoryMarshal.GetArrayDataReference(buffer);
                    count = 0;
                }

                Unsafe.Add(ref destination, count++) = item;
            }
        }

        currentCount = count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<TPredicate>(List<T> source, TPredicate predicate)
        where TPredicate : struct, IFunction<T, bool>
        => Add(CollectionsMarshal.AsSpan(source), predicate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<TPredicate>(List<T> source, in TPredicate predicate)
        where TPredicate : struct, IFunctionIn<T, bool>
        => Add(CollectionsMarshal.AsSpan(source), in predicate);

    [MethodImpl(MethodImplOptions.NoInlining)]
    void Grow()
    {
        if (previousBuffersCount == MaxSubArrays)
        {
            ThrowOutOfMemory();
        }

        previousBuffers[previousBuffersCount++] = currentBuffer;
        totalCount += currentBuffer.Length;

        var newCapacity = currentBuffer.Length * 2;
        currentBuffer = pool.Rent(newCapacity);
        currentCount = 0;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void ThrowOutOfMemory() => throw new OutOfMemoryException();

    public readonly T[] ToArray()
    {
        var count = totalCount + currentCount;
        if (count == 0)
        {
            return [];
        }

        var result = GC.AllocateUninitializedArray<T>(count);
        var destination = result.AsSpan();

        if (previousBuffersCount > 0)
        {
            ReadOnlySpan<T[]> span = previousBuffers;
            foreach (var buffer in span.Slice(0, previousBuffersCount))
            {
                buffer.AsSpan().CopyTo(destination);
                destination = destination.Slice(buffer.Length);
            }
        }
        currentBuffer.AsSpan(0, currentCount).CopyTo(destination);

        return result;
    }

    public readonly List<T> ToList()
    {
        var count = totalCount + currentCount;
        if (count == 0)
        {
            return [];
        }

        var result = new List<T>(count);
        CollectionsMarshal.SetCount(result, count);
        var destination = CollectionsMarshal.AsSpan(result);

        if (previousBuffersCount > 0)
        {
            ReadOnlySpan<T[]> span = previousBuffers;
            foreach (var buffer in span.Slice(0, previousBuffersCount))
            {
                buffer.AsSpan().CopyTo(destination);
                destination = destination.Slice(buffer.Length);
            }
        }

        currentBuffer.AsSpan(0, currentCount).CopyTo(destination);

        return result;
    }

    public PooledBuffer<T> ToPooledBuffer()
    {
        var count = totalCount + currentCount;
        if (count == 0)
        {
            // Return empty buffer
            if (currentBuffer is not null)
            {
                pool.Return(currentBuffer, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
            }

            // Return previous buffers if any
            for (var i = 0; i < previousBuffersCount; i++)
            {
                pool.Return(previousBuffers[i], RuntimeHelpers.IsReferenceOrContainsReferences<T>());
            }

            // Important: set to null so Dispose doesn't double-return
            currentBuffer = null!;
            previousBuffers = default;
            previousBuffersCount = 0;

            return PooledBuffer.Empty<T>();
        }

        // Optimization: if we have a single chunk, transfer ownership
        if (previousBuffersCount == 0)
        {
            var bufferToReturn = currentBuffer;
            currentBuffer = null!; // Transfer ownership
            return new PooledBuffer<T>(bufferToReturn, count, pool);
        }

        // Multiple chunks: allocate a single contiguous buffer
        var resultBuffer = pool.Rent(count);
        var destination = resultBuffer.AsSpan(0, count);

        if (previousBuffersCount > 0)
        {
            foreach (var buffer in MemoryMarshal.CreateSpan(ref previousBuffers[0], previousBuffersCount))
            {
                buffer.AsSpan().CopyTo(destination);
                destination = destination.Slice(buffer.Length);
                pool.Return(buffer, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
            }
        }
        previousBuffers = default; // Prevent double return in Dispose
        previousBuffersCount = 0;

        currentBuffer.AsSpan(0, currentCount).CopyTo(destination);

        // Return current chunk to pool immediately
        pool.Return(currentBuffer, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
        currentBuffer = null!; // Prevent double return in Dispose

        return new PooledBuffer<T>(resultBuffer, count, pool);
    }

    public void Dispose()
    {
        if (currentBuffer is not null)
        {
            pool.Return(currentBuffer, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
            currentBuffer = null!;
        }

        for (var i = 0; i < previousBuffersCount; i++)
        {
            pool.Return(previousBuffers[i], RuntimeHelpers.IsReferenceOrContainsReferences<T>());
        }
    }
}
