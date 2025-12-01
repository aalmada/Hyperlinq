using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// A disposable wrapper around a pooled array from ArrayPool&lt;T&gt;.
    /// Must be disposed to return the buffer to the pool.
    /// </summary>
    /// <typeparam name="T">The type of elements in the buffer.</typeparam>
    public readonly struct PooledBuffer<T> : IDisposable
    {
        private readonly T[]? buffer;
        private readonly int length;

        // Growth strategy constants
        private const int DefaultInitialCapacity = 4;
        private const int MaxArrayLength = 0x7FFFFFC7; // Array.MaxLength

        internal PooledBuffer(T[] buffer, int length)
        {
            this.buffer = buffer;
            this.length = length;
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
                ArrayPool<T>.Shared.Return(buffer, clearArray);
            }
        }

        /// <summary>
        /// Calculates the next capacity using a doubling strategy, capped at Array.MaxLength.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetNextCapacity(int currentCapacity)
        {
            var newCapacity = currentCapacity * 2;

            // Prevent overflow and respect Array.MaxLength
            if ((uint)newCapacity > MaxArrayLength)
                newCapacity = MaxArrayLength;

            return newCapacity;
        }

        /// <summary>
        /// Gets the default initial capacity for dynamic growth scenarios.
        /// </summary>
        internal static int GetDefaultInitialCapacity() => DefaultInitialCapacity;
    }
}
