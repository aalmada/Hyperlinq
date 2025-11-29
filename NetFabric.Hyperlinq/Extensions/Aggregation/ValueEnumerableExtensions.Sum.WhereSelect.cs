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
        public static TSource Sum<TSource, TResult>(this WhereSelectMemoryEnumerable<TSource, TResult> source)
            where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
            => source.Source.Span.Sum(source.Predicate);

        /// <summary>
        /// Optimized Sum for WhereSelectListEnumerable - iterates source with predicate only.
        /// Ignores the selector since Sum operates on source values that match the predicate.
        /// Delegates to SpanExtensions.Sum for implementation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Sum<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
            where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
            => System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source.Source).Sum(source.Predicate);

    }
}
