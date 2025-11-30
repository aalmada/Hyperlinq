using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Optimized Sum for WhereSelectMemoryEnumerable - iterates source with predicate only.
        /// Ignores the selector since Sum operates on source values that match the predicate.
        /// Delegates to SpanExtensions.Sum for implementation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Sum<TSource, TResult>(this WhereSelectMemoryEnumerable<TSource, TResult> source)
            where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
        {
            var sum = TResult.AdditiveIdentity;
            var span = source.Source.Span;
            for (var index = 0; index < span.Length; index++)
            {
                if (source.Predicate(span[index]))
                    sum += source.Selector(span[index]);
            }
            return sum;
        }

        /// <summary>
        /// Optimized Sum for WhereSelectListEnumerable - iterates source with predicate only.
        /// Ignores the selector since Sum operates on source values that match the predicate.
        /// Delegates to SpanExtensions.Sum for implementation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Sum<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
            where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
        {
            var sum = TResult.AdditiveIdentity;
            var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source.Source);
            for (var index = 0; index < span.Length; index++)
            {
                if (source.Predicate(span[index]))
                    sum += source.Selector(span[index]);
            }
            return sum;
        }

    }
}
