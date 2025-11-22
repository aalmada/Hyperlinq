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

        public static bool Any<TSource>(ICollection<TSource> source)
            => source.Count != 0;

        public static int Count<TSource>(IEnumerable<TSource> source)
        {
            if (source is ICollection<TSource> collection)
                return collection.Count;

            var count = 0;
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                count++;
            return count;
        }

        public static int Count<TSource>(ICollection<TSource> source)
            => source.Count;

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

        public static IEnumerable<TResult> Select<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source is WhereEnumerable<TSource> whereEnumerable)
                return whereEnumerable.Select(selector);

            return new SelectEnumerable<TSource, TResult>(source, selector);
        }

        public static WhereEnumerable<TSource> Where<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
            => new WhereEnumerable<TSource>(source, predicate);
    }
}
