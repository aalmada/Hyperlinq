using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereArrayRefStructEnumerableExtensions
    {
        /// <summary>
        /// Computes the sum of a WhereArrayRefStructEnumerable for numeric types.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Sum<TSource>(this WhereArrayRefStructEnumerable<TSource> source)
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
