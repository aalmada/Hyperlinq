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
            => source.MinOrNone().Value;

        /// <summary>
        /// Returns the maximum value after applying the selector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Max<TSource, TResult, TSelector>(this SelectListEnumerable<TSource, TResult, TSelector> source)
            where TResult : INumber<TResult>
            where TSelector : struct, IFunction<TSource, TResult>
            => source.MaxOrNone().Value;

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
            var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source.Source);
            
            foreach (ref readonly var item in span)
            {
                sum += selector.Invoke(item);
            }
            
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> MinOrNone<TSource, TResult, TSelector>(this SelectListEnumerable<TSource, TResult, TSelector> source)
            where TResult : INumber<TResult>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            if (source.Count == 0)
                return Option<TResult>.None();
            
            var selector = source.Selector;
            var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source.Source);
            var min = selector.Invoke(span[0]);
            
            foreach (ref readonly var item in span[1..])
            {
                var value = selector.Invoke(item);
                if (value < min)
                    min = value;
            }
            
            return Option<TResult>.Some(min);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> MaxOrNone<TSource, TResult, TSelector>(this SelectListEnumerable<TSource, TResult, TSelector> source)
            where TResult : INumber<TResult>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            if (source.Count == 0)
                return Option<TResult>.None();
            
            var selector = source.Selector;
            var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source.Source);
            var max = selector.Invoke(span[0]);
            
            foreach (ref readonly var item in span[1..])
            {
                var value = selector.Invoke(item);
                if (value > max)
                    max = value;
            }
            
            return Option<TResult>.Some(max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (TResult Min, TResult Max) MinMax<TSource, TResult, TSelector>(this SelectListEnumerable<TSource, TResult, TSelector> source)
            where TResult : INumber<TResult>
            where TSelector : struct, IFunction<TSource, TResult>
            => source.MinMaxOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<(TResult Min, TResult Max)> MinMaxOrNone<TSource, TResult, TSelector>(this SelectListEnumerable<TSource, TResult, TSelector> source)
            where TResult : INumber<TResult>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            if (source.Count == 0)
                return Option<(TResult Min, TResult Max)>.None();
            
            var selector = source.Selector;
            var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source.Source);
            var value = selector.Invoke(span[0]);
            var min = value;
            var max = value;
            
            foreach (ref readonly var item in span[1..])
            {
                value = selector.Invoke(item);
                if (value < min)
                    min = value;
                else if (value > max)
                    max = value;
            }
            
            return Option<(TResult Min, TResult Max)>.Some((min, max));
        }
    }
}
