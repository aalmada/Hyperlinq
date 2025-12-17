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
        /// Returns the minimum value in an array segment using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min()
            => source.AsSpan().Min();

        /// <summary>
        /// Returns the minimum value that satisfies a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min(Func<T, bool> predicate)
            => source.AsSpan().Min(predicate);

        /// <summary>
        /// Computes both minimum and maximum values in a single iteration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax()
            => source.AsSpan().MinMax();

        /// <summary>
        /// Computes both minimum and maximum values for elements that satisfy a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax(Func<T, bool> predicate)
            => source.AsSpan().MinMax(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone()
            => source.AsSpan().MinOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone(Func<T, bool> predicate)
            => source.AsSpan().MinOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone()
            => source.AsSpan().MinMaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone(Func<T, bool> predicate)
            => source.AsSpan().MinMaxOrNone(predicate);
    }
}
