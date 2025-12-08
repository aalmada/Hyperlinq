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
        public static SelectListEnumerable<TSource, TResult, SelectorCompose<TSource, TIntermediate, TResult, TSelector, FunctionWrapper<TIntermediate, TResult>>> Select<TSource, TIntermediate, TResult, TSelector>(
            this SelectListEnumerable<TSource, TIntermediate, TSelector> source, 
            Func<TIntermediate, TResult> selector)
            where TSelector : struct, IFunction<TSource, TIntermediate>
        {
            var firstSelector = source.Selector;
            return new SelectListEnumerable<TSource, TResult, SelectorCompose<TSource, TIntermediate, TResult, TSelector, FunctionWrapper<TIntermediate, TResult>>>(
                source.Source, 
                new SelectorCompose<TSource, TIntermediate, TResult, TSelector, FunctionWrapper<TIntermediate, TResult>>(
                    firstSelector, 
                    new FunctionWrapper<TIntermediate, TResult>(selector)));
        }

        /// <summary>
        /// Returns the minimum value after applying the selector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Min<TSource, TResult, TSelector>(this SelectListEnumerable<TSource, TResult, TSelector> source)
            where TResult : INumber<TResult>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            if (source.Count == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            
            var selector = source.Selector;
            var list = source.Source;
            var min = selector.Invoke(list[0]);
            
            for (var i = 1; i < list.Count; i++)
            {
                var value = selector.Invoke(list[i]);
                if (value < min)
                    min = value;
            }
            
            return min;
        }

        /// <summary>
        /// Returns the maximum value after applying the selector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Max<TSource, TResult, TSelector>(this SelectListEnumerable<TSource, TResult, TSelector> source)
            where TResult : INumber<TResult>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            if (source.Count == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            
            var selector = source.Selector;
            var list = source.Source;
            var max = selector.Invoke(list[0]);
            
            for (var i = 1; i < list.Count; i++)
            {
                var value = selector.Invoke(list[i]);
                if (value > max)
                    max = value;
            }
            
            return max;
        }

        /// <summary>
        /// Computes the sum after applying the selector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Sum<TSource, TResult, TSelector>(this SelectListEnumerable<TSource, TResult, TSelector> source)
            where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            var sum = TResult.AdditiveIdentity;
            var selector = source.Selector;
            var list = source.Source;
            
            for (var i = 0; i < list.Count; i++)
            {
                sum += selector.Invoke(list[i]);
            }
            
            return sum;
        }
    }
}
