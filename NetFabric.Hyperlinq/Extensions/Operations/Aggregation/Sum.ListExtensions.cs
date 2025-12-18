using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListExtensions
{
    extension<T>(List<T> source)
        where T : struct, INumberBase<T>
    {
        /// <summary>
        /// Computes the sum of a sequence of numeric values using SIMD acceleration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum()
            => CollectionsMarshal.AsSpan(source).Sum();

        /// <summary>
        /// Computes the sum of elements that satisfy a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Sum(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).Sum(predicate);
    }
}
