using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayValueEnumerableExtensions
{
    extension<T>(ArrayValueEnumerable<T> source)
    {
        /// <summary>
        /// Bypasses a specified number of elements and returns the remaining elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> Skip(int count)
            => source.Source.AsSpan().Skip(count);
    }
}
