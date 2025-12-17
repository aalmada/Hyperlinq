using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListExtensions
{
    extension<T>(List<T> source)
        where T : struct, INumber<T>, IMinMaxValue<T>
    {
        /// <summary>
        /// Computes the average of a list using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Average()
            => CollectionsMarshal.AsSpan(source).Average();

        /// <summary>
        /// Computes the average of elements that satisfy a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Average(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).Average(predicate);

        /// <summary>
        /// Computes the average of a list, returning None if empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> AverageOrNone()
            => CollectionsMarshal.AsSpan(source).AverageOrNone();

        /// <summary>
        /// Computes the average of elements that satisfy a condition, returning None if no matches.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> AverageOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).AverageOrNone(predicate);
    }
}
