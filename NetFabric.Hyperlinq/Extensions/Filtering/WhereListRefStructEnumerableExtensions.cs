using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereListRefStructEnumerableExtensions
    {
        /// <summary>
        /// Computes the sum of a WhereListRefStructEnumerable for numeric types.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Sum<TSource>(this WhereListRefStructEnumerable<TSource> source)
            where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
        {
            var sum = TSource.AdditiveIdentity;
            foreach (var item in source)
            {
                sum += item;
            }
            return sum;
        }
    }
}
