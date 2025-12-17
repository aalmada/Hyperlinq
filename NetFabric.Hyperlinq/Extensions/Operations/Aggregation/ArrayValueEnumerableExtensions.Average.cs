using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayValueEnumerableExtensions
{
    extension<T>(ArrayValueEnumerable<T> source)
        where T : struct, INumberBase<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>, IDivisionOperators<T, T, T>
    {
        /// <summary>
        /// Computes the average of an array using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Average()
            => source.Source.Average();

        /// <summary>
        /// Computes the average of elements that satisfy a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Average(Func<T, bool> predicate)
            => source.Source.Average(predicate);

        /// <summary>
        /// Computes the average of an array, returning None if empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> AverageOrNone()
            => source.Source.AverageOrNone();

        /// <summary>
        /// Computes the average of elements that satisfy a condition, returning None if no matches.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> AverageOrNone(Func<T, bool> predicate)
            => source.Source.AverageOrNone(predicate);
    }
}
