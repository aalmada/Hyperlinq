using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class SelectReadOnlySpanEnumerableExtensions
    {
        /// <summary>
        /// Fuses consecutive Select operations by composing selectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectReadOnlySpanEnumerable<TSource, TResult> Select<TSource, TIntermediate, TResult>(
            this SelectReadOnlySpanEnumerable<TSource, TIntermediate> source, 
            Func<TIntermediate, TResult> selector)
        {
            var firstSelector = source.Selector;
            return new SelectReadOnlySpanEnumerable<TSource, TResult>(source.Source, item => selector(firstSelector(item)));
        }

        /// <summary>
        /// Returns the minimum value after applying the selector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Min<TSource, TResult>(this SelectReadOnlySpanEnumerable<TSource, TResult> source)
            where TResult : INumber<TResult>
        {
            var span = source.Source;
            if (span.Length == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            
            var selector = source.Selector;
            var min = selector(span[0]);
            
            for (var i = 1; i < span.Length; i++)
            {
                var value = selector(span[i]);
                if (value < min)
                    min = value;
            }
            
            return min;
        }

        /// <summary>
        /// Returns the maximum value after applying the selector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Max<TSource, TResult>(this SelectReadOnlySpanEnumerable<TSource, TResult> source)
            where TResult : INumber<TResult>
        {
            var span = source.Source;
            if (span.Length == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            
            var selector = source.Selector;
            var max = selector(span[0]);
            
            for (var i = 1; i < span.Length; i++)
            {
                var value = selector(span[i]);
                if (value > max)
                    max = value;
            }
            
            return max;
        }

        /// <summary>
        /// Computes the sum after applying the selector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Sum<TSource, TResult>(this SelectReadOnlySpanEnumerable<TSource, TResult> source)
            where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
        {
            var sum = TResult.AdditiveIdentity;
            var selector = source.Selector;
            var span = source.Source;
            
            for (var i = 0; i < span.Length; i++)
            {
                sum += selector(span[i]);
            }
            
            return sum;
        }
    }
}
