using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

/// <summary>
/// A disposable wrapper around a pooled array from ArrayPool&lt;T&gt;.
/// Must be disposed to return the buffer to the pool.
/// </summary>
/// <typeparam name="T">The type of elements in the buffer.</typeparam>
public readonly struct PooledBuffer<T> : IDisposable
{
    readonly T[]? buffer;
    readonly int length;
    readonly ArrayPool<T>? pool;

    internal PooledBuffer(T[] buffer, int length, ArrayPool<T>? pool = null)
    {
        this.buffer = buffer;
        this.length = length;
        this.pool = pool;
    }

    /// <summary>
    /// Gets the number of elements in the buffer.
    /// </summary>
    public int Length => length;

    /// <summary>
    /// Returns the buffer contents as a ReadOnlySpan&lt;T&gt;.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<T> AsSpan() => buffer.AsSpan(0, length);

    /// <summary>
    /// Creates a new array containing a copy of the buffer contents.
    /// </summary>
    public T[] ToArray() => AsSpan().ToArray();

    /// <summary>
    /// Returns the buffer to the pool. After calling this method, the buffer should not be used.
    /// </summary>
    public void Dispose()
    {
        if (buffer is not null)
        {
            // Clear the array only if it contains references to prevent memory leaks
            var clearArray = RuntimeHelpers.IsReferenceOrContainsReferences<T>();
            var poolToUse = pool ?? ArrayPool<T>.Shared;
            poolToUse.Return(buffer, clearArray);
        }
    }
}
