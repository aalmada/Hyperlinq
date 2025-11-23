using System;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public static class Optimized
    {
        public static bool Any<TSource>(this IEnumerable<TSource> source)
        {
            if (source is ICollection<TSource> collection)
                return collection.Count != 0;

            using var enumerator = source.GetEnumerator();
            return enumerator.MoveNext();
        }

        public static bool Any<TSource>(this ICollection<TSource> source)
            => source.Count != 0;

        public static int Count<TSource>(this IEnumerable<TSource> source)
        {
            if (source is ICollection<TSource> collection)
                return collection.Count;

            var count = 0;
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                count++;
            return count;
        }

        public static int Count<TSource>(this ICollection<TSource> source)
            => source.Count;

        public static TSource First<TSource>(this IEnumerable<TSource> source)
        {
            using var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current;
            
            throw new InvalidOperationException("Sequence contains no elements");
        }

        public static TSource Single<TSource>(this IEnumerable<TSource> source)
        {
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements");
            
            var first = enumerator.Current;
            if (enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains more than one element");

            return first;
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source is WhereEnumerable<TSource> whereEnumerable)
                return whereEnumerable.Select(selector);

            return new SelectEnumerable<TSource, TResult>(source, selector);
        }

        public static WhereEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            => new WhereEnumerable<TSource>(source, predicate);
    }
}
