using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArraySegmentExtensions
{
    extension<T>(ArraySegment<T> source)
    {
        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any()
            => source.Count > 0;

        /// <summary>
        /// Determines whether any element satisfies a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any(Func<T, bool> predicate)
            => source.AsSpan().Any(predicate);
    }
}
