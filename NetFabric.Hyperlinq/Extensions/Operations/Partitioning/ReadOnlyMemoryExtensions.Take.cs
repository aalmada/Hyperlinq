using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlyMemoryExtensions
{
    extension<T>(ReadOnlyMemory<T> source)
    {
        /// <summary>
        /// Returns a specified number of contiguous elements from the start.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyMemory<T> Take(int count)
            => count <= 0 ? ReadOnlyMemory<T>.Empty : source[..(count < source.Length ? count : source.Length)];
    }
}
