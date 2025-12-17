using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArraySegmentExtensions
{
    extension<T>(ArraySegment<T> source)
    {
        /// <summary>
        /// Returns the last element of an array segment.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last()
            => source.AsSpan().Last();

        /// <summary>
        /// Returns the last element that satisfies a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last(Func<T, bool> predicate)
            => source.AsSpan().Last(predicate);
    }
}
