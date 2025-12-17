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
        /// Computes the average of a memory using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Average()
            => source.Span.Average();

        /// <summary>
        /// Computes the average of elements that satisfy a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Average(Func<T, bool> predicate)
            => source.Span.Average(predicate);

        /// <summary>
        /// Computes the average of a memory, returning None if empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> AverageOrNone()
            => source.Span.AverageOrNone();

        /// <summary>
        /// Computes the average of elements that satisfy a condition, returning None if no matches.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> AverageOrNone(Func<T, bool> predicate)
            => source.Span.AverageOrNone(predicate);
    }
}
