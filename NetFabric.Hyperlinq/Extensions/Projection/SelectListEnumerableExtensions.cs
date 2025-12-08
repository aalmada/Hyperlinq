using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class SelectListEnumerableExtensions
    {
        /// <summary>
        /// Fuses consecutive Select operations by composing selectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectListEnumerable<TSource, TResult> Select<TSource, TIntermediate, TResult>(
            this SelectListEnumerable<TSource, TIntermediate> source, 
            Func<TIntermediate, TResult> selector)
        {
            var firstSelector = source.Selector;
            return new SelectListEnumerable<TSource, TResult>(source.Source, item => selector(firstSelector(item)));
        }

        /// <summary>
        /// Returns the minimum value after applying the selector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Min<TSource, TResult>(this SelectListEnumerable<TSource, TResult> source)
            where TResult : INumber<TResult>
        {
            if (source.Count == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            
            var selector = source.Selector;
            var list = source.Source;
            var min = selector(list[0]);
            
            for (var i = 1; i < list.Count; i++)
            {
                var value = selector(list[i]);
                if (value < min)
                    min = value;
            }
            
            return min;
        }

        /// <summary>
        /// Returns the maximum value after applying the selector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Max<TSource, TResult>(this SelectListEnumerable<TSource, TResult> source)
            where TResult : INumber<TResult>
        {
            if (source.Count == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            
            var selector = source.Selector;
            var list = source.Source;
            var max = selector(list[0]);
            
            for (var i = 1; i < list.Count; i++)
            {
                var value = selector(list[i]);
                if (value > max)
                    max = value;
            }
            
            return max;
        }

        /// <summary>
        /// Computes the sum after applying the selector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Sum<TSource, TResult>(this SelectListEnumerable<TSource, TResult> source)
            where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
        {
            var sum = TResult.AdditiveIdentity;
            var selector = source.Selector;
            var list = source.Source;
            
            for (var i = 0; i < list.Count; i++)
            {
                sum += selector(list[i]);
            }
            
            return sum;
        }
    }
}
