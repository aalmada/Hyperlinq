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

        public static SelectEnumerable<TSource, TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
            => new SelectEnumerable<TSource, TResult>(source, selector);

        public static WhereEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            => new WhereEnumerable<TSource>(source, predicate);

        public static T Sum<T>(this IEnumerable<T> source)
            where T : System.Numerics.INumber<T>
        {
            if (source is T[] array)
                return T.CreateChecked(System.Numerics.Tensors.TensorPrimitives.Sum<T>(array));

            if (source is List<T> list)
                return T.CreateChecked(System.Numerics.Tensors.TensorPrimitives.Sum(System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list)));

            var sum = T.Zero;
            foreach (var item in source)
                sum += item;
            return sum;
        }
    }
}
