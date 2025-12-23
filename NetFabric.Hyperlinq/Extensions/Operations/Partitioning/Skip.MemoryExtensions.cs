using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class MemoryExtensions
{
    extension<T>(Memory<T> source)
    {
        /// <summary>
        /// Bypasses a specified number of elements and returns the remaining elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory<T> Skip(int count)
            => count <= 0 ? source : source[(count < source.Length ? count : source.Length)..];
    }
}
