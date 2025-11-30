using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereSelectReadOnlySpanEnumerableExtensions
    {
        /// <summary>
        /// Computes the sum of a WhereSelectReadOnlySpanEnumerable for numeric result types.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Sum<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source)
            where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
        {
            var sum = TResult.AdditiveIdentity;
            foreach (var item in source)
            {
                sum += item;
            }
            return sum;
        }
    }
}
