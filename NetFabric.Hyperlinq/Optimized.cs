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

        public static int Sum(this IEnumerable<int> source)
        {
            if (source is int[] array)
                return (int)System.Numerics.Tensors.TensorPrimitives.Sum<int>(array);

            if (source is List<int> list)
                return (int)System.Numerics.Tensors.TensorPrimitives.Sum(System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list));

            var sum = 0;
            foreach (var item in source)
                sum += item;
            return sum;
        }

        public static long Sum(this IEnumerable<long> source)
        {
            if (source is long[] array)
                return (long)System.Numerics.Tensors.TensorPrimitives.Sum<long>(array);

            if (source is List<long> list)
                return (long)System.Numerics.Tensors.TensorPrimitives.Sum(System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list));

            var sum = 0L;
            foreach (var item in source)
                sum += item;
            return sum;
        }

        public static float Sum(this IEnumerable<float> source)
        {
            if (source is float[] array)
                return System.Numerics.Tensors.TensorPrimitives.Sum(array);

            if (source is List<float> list)
                return System.Numerics.Tensors.TensorPrimitives.Sum(System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list));

            var sum = 0f;
            foreach (var item in source)
                sum += item;
            return sum;
        }

        public static double Sum(this IEnumerable<double> source)
        {
            if (source is double[] array)
                return System.Numerics.Tensors.TensorPrimitives.Sum(array);

            if (source is List<double> list)
                return System.Numerics.Tensors.TensorPrimitives.Sum(System.Runtime.InteropServices.CollectionsMarshal.AsSpan(list));

            var sum = 0.0;
            foreach (var item in source)
                sum += item;
            return sum;
        }
    }
}
