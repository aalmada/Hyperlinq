using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlyMemoryExtensions
{
    extension<T>(ReadOnlyMemory<T> source)
        where T : struct, INumber<T>, IMinMaxValue<T>
    {
        /// <summary>
        /// Returns the minimum value in a memory using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min()
            => source.Span.Min();

        /// <summary>
        /// Returns the minimum value that satisfies a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Min(Func<T, bool> predicate)
            => source.Span.Min(predicate);

        /// <summary>
        /// Computes both minimum and maximum values in a single iteration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax()
            => source.Span.MinMax();

        /// <summary>
        /// Computes both minimum and maximum values for elements that satisfy a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (T Min, T Max) MinMax(Func<T, bool> predicate)
            => source.Span.MinMax(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone()
            => source.Span.MinOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> MinOrNone(Func<T, bool> predicate)
            => source.Span.MinOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone()
            => source.Span.MinMaxOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<(T Min, T Max)> MinMaxOrNone(Func<T, bool> predicate)
            => source.Span.MinMaxOrNone(predicate);
    }
}
