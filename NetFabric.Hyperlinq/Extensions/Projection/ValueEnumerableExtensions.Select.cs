using System;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        public static SelectEnumerable<TSource, TResult> Select<TEnumerable, TEnumerator, TSource, TResult>(
            this TEnumerable source,
            Func<TSource, TResult> selector)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            return new SelectEnumerable<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of a collection into a new form.
        /// Preserves Count property for O(1) access.
        /// </summary>
        public static SelectCollectionEnumerable<TEnumerator, TSource, TResult> Select<TEnumerator, TSource, TResult>(
            this IValueReadOnlyCollection<TSource, TEnumerator> source,
            Func<TSource, TResult> selector)
            where TEnumerator : struct, IEnumerator<TSource>
        {
            return new SelectCollectionEnumerable<TEnumerator, TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of a list into a new form.
        /// </summary>
        public static SelectListEnumerable<TSource, TResult> Select<TSource, TResult>(
            this ListValueEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            return new SelectListEnumerable<TSource, TResult>(source.Source, selector);
        }

        /// <summary>
        /// Projects each element of an array into a new form.
        /// </summary>
        public static SelectArrayEnumerable<TSource, TResult> Select<TSource, TResult>(
            this ArrayValueEnumerable<TSource> source,
            Func<TSource, TResult> selector)
        {
            return new SelectArrayEnumerable<TSource, TResult>(source.Source, selector);
        }
    }
}
