using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> source)
    {
        /// <summary>
        /// Returns a specified number of contiguous elements from the start.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> Take(int count)
            => count <= 0 ? ReadOnlySpan<T>.Empty : source[..(count < source.Length ? count : source.Length)];
    }
}
