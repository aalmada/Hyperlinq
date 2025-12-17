using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListValueEnumerableExtensions
{
    extension<T>(ListValueEnumerable<T> source)
        where T : struct, INumberBase<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>, IDivisionOperators<T, T, T>
    {
        /// <summary>
        /// Computes the average of a list using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Average()
            => CollectionsMarshal.AsSpan(source.Source).Average();

        /// <summary>
        /// Computes the average of elements that satisfy a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Average(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).Average(predicate);

        /// <summary>
        /// Computes the average of a list, returning None if empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> AverageOrNone()
            => CollectionsMarshal.AsSpan(source.Source).AverageOrNone();

        /// <summary>
        /// Computes the average of elements that satisfy a condition, returning None if no matches.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> AverageOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source.Source).AverageOrNone(predicate);
    }
}
