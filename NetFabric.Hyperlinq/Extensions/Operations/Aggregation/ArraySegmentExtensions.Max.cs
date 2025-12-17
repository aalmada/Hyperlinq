using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArraySegmentExtensions
{
    extension<T>(ArraySegment<T> source)
        where T : struct, INumber<T>, IMinMaxValue<T>
    {
        /// <summary>
        /// Returns the maximum value in an array segment using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max()
            => source.AsSpan().Max();

        /// <summary>
        /// Returns the maximum value that satisfies a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Max(Func<T, bool> predicate)
            => source.AsSpan().Max(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone()
            => source.AsSpan().MaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MaxOrNone(Func<T, bool> predicate)
            => source.AsSpan().MaxOrNone(predicate);
    }
}
