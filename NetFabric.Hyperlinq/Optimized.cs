using System;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public static class Optimized
    {
        public static bool Any<TSource>(IEnumerable<TSource> source)
        {
            if (source is ICollection<TSource> collection)
                return collection.Count != 0;

            using var enumerator = source.GetEnumerator();
            return enumerator.MoveNext();
        }

        public static TSource First<TSource>(IEnumerable<TSource> source)
        {
            using var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current;
            
            throw new InvalidOperationException("Sequence contains no elements");
        }

        public static TSource Single<TSource>(IEnumerable<TSource> source)
        {
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements");
            
            var first = enumerator.Current;
            if (enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains more than one element");

            return first;
        }

        public static IValueEnumerable<TResult, SelectEnumerable<TSource, TResult>.Enumerator> Select<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
            => new SelectEnumerable<TSource, TResult>(source, selector);

        public static IValueEnumerable<TSource, WhereEnumerable<TSource>.Enumerator> Where<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
            => new WhereEnumerable<TSource>(source, predicate);
    }
}
