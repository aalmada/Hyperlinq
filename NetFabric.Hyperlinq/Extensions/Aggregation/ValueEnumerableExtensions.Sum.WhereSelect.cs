using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Optimized Sum for WhereMemoryEnumerable - uses optimized enumeration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this WhereMemoryEnumerable<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            var sum = T.AdditiveIdentity;
            foreach (var item in source)
                sum += item;
            return sum;
        }

        /// <summary>
        /// Optimized Sum for WhereSelectMemoryEnumerable - uses optimized enumeration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Sum<TSource, TResult>(this WhereSelectMemoryEnumerable<TSource, TResult> source)
            where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
        {
            var sum = TResult.AdditiveIdentity;
            foreach (var item in source)
                sum += item;
            return sum;
        }

        /// <summary>
        /// Optimized Sum for WhereListEnumerable - uses optimized enumeration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this WhereListEnumerable<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            var sum = T.AdditiveIdentity;
            foreach (var item in source)
                sum += item;
            return sum;
        }

        /// <summary>
        /// Optimized Sum for WhereSelectListEnumerable - uses optimized enumeration.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Sum<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
            where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
        {
            var sum = TResult.AdditiveIdentity;
            foreach (var item in source)
                sum += item;
            return sum;
        }
    }
}
