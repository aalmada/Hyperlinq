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
        /// Computes the average of an array segment using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Average()
            => source.AsSpan().Average();

        /// <summary>
        /// Computes the average of elements that satisfy a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Average(Func<T, bool> predicate)
            => source.AsSpan().Average(predicate);

        /// <summary>
        /// Computes the average of an array segment, returning None if empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> AverageOrNone()
            => source.AsSpan().AverageOrNone();

        /// <summary>
        /// Computes the average of elements that satisfy a condition, returning None if no matches.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> AverageOrNone(Func<T, bool> predicate)
            => source.AsSpan().AverageOrNone(predicate);
    }
}
