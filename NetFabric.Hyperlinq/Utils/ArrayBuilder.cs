using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

/// <summary>
/// A builder for arrays that uses pooled buffers to minimize allocations.
/// </summary>
/// <typeparam name="T">The type of elements in the array.</typeparam>
struct ArrayBuilder<T> : IDisposable
{
    const int DefaultCapacity = 4;

    readonly ArrayPool<T> pool;
    T[] currentBuffer;
    int currentCount;
    T[][]? previousBuffers;
    int previousBuffersCount;
    int totalCount;

    public ArrayBuilder(ArrayPool<T> pool)
    {
        this.pool = pool;
        currentBuffer = pool.Rent(DefaultCapacity);
        currentCount = 0;
        previousBuffers = null;
        previousBuffersCount = 0;
        totalCount = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item)
    {
        if (currentCount == currentBuffer.Length)
        {
            Grow();
        }

        currentBuffer[currentCount++] = item;
        totalCount++;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    void Grow()
    {
        if (previousBuffers is null)
        {
            previousBuffers = new T[4][];
        }
        else if (previousBuffersCount == previousBuffers.Length)
        {
            Array.Resize(ref previousBuffers, previousBuffers.Length * 2);
        }

        previousBuffers[previousBuffersCount++] = currentBuffer;

        var newCapacity = currentBuffer.Length * 2;
        currentBuffer = pool.Rent(newCapacity);
        currentCount = 0;
    }

    public readonly T[] ToArray()
    {
        if (totalCount == 0)
        {
            return Array.Empty<T>();
        }

        var result = new T[totalCount];
        var destination = result.AsSpan();

        if (previousBuffers is not null)
        {
            for (var i = 0; i < previousBuffersCount; i++)
            {
                var buffer = previousBuffers[i];
                buffer.AsSpan().CopyTo(destination);
                destination = destination.Slice(buffer.Length);
            }
        }

        currentBuffer.AsSpan(0, currentCount).CopyTo(destination);

        return result;
    }

    public PooledBuffer<T> ToPooledBuffer()
    {
        if (totalCount == 0)
        {
            // Return empty buffer
            if (currentBuffer is not null)
            {
                pool.Return(currentBuffer, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
            }

            // Return previous buffers if any (though typically there won't be if totalCount is 0)
            if (previousBuffers is not null)
            {
                for (var i = 0; i < previousBuffersCount; i++)
                {
                    pool.Return(previousBuffers[i], RuntimeHelpers.IsReferenceOrContainsReferences<T>());
                }
            }

            // Important: set to null so Dispose doesn't double-return
            currentBuffer = null!;
            previousBuffers = null;

            return PooledBuffer.Empty<T>();
        }

        // Optimization: if we have a single chunk, transfer ownership
        if (previousBuffers is null)
        {
            var bufferToReturn = currentBuffer;
            currentBuffer = null!; // Transfer ownership
            return new PooledBuffer<T>(bufferToReturn, totalCount, pool);
        }

        // Multiple chunks: allocate a single contiguous buffer
        var resultBuffer = pool.Rent(totalCount);
        var destination = resultBuffer.AsSpan(0, totalCount);

        for (var i = 0; i < previousBuffersCount; i++)
        {
            var buffer = previousBuffers[i];
            buffer.AsSpan().CopyTo(destination);
            destination = destination.Slice(buffer.Length);

            // Return previous chunk to pool immediately
            pool.Return(buffer, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
        }
        previousBuffers = null; // Prevent double return in Dispose

        currentBuffer.AsSpan(0, currentCount).CopyTo(destination);

        // Return current chunk to pool immediately
        pool.Return(currentBuffer, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
        currentBuffer = null!; // Prevent double return in Dispose

        return new PooledBuffer<T>(resultBuffer, totalCount, pool);
    }

    public void Dispose()
    {
        if (currentBuffer is not null)
        {
            pool.Return(currentBuffer, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
            currentBuffer = null!;
        }

        if (previousBuffers is not null)
        {
            for (var i = 0; i < previousBuffersCount; i++)
            {
                pool.Return(previousBuffers[i], RuntimeHelpers.IsReferenceOrContainsReferences<T>());
            }
            previousBuffers = null;
        }
    }
}
