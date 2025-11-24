using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Extension methods for IValueEnumerable providing LINQ-like operations.
    /// </summary>
    public static class ValueEnumerableExtensions
    {
        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// </summary>
        public static bool Any<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            using var enumerator = source.GetEnumerator();
            return enumerator.MoveNext();
        }

        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        public static int Count<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            var count = 0;
            foreach (var _ in source)
                count++;
            return count;
        }

        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        public static TSource First<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            using var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
                return enumerator.Current;
            
            throw new InvalidOperationException("Sequence contains no elements");
        }

        /// <summary>
        /// Returns the only element of a sequence, and throws an exception if there is not exactly one element.
        /// </summary>
        public static TSource Single<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements");
            
            var first = enumerator.Current;
            if (enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains more than one element");

            return first;
        }

        /// <summary>
        /// Computes the sum of a sequence of numeric values.
        /// Uses TensorPrimitives optimization for arrays and lists.
        /// </summary>
        public static T Sum<TEnumerable, TEnumerator, T>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            // Optimize using TensorPrimitives for arrays
            if (source is ArrayValueEnumerable<T> arrayEnum)
            {
                return System.Numerics.Tensors.TensorPrimitives.Sum<T>(arrayEnum.Source);
            }
            
            // Optimize using TensorPrimitives for lists
            if (source is ListValueEnumerable<T> listEnum)
            {
                var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(listEnum.Source);
                return System.Numerics.Tensors.TensorPrimitives.Sum<T>(span);
            }

            // Fallback to standard enumeration
            var sum = T.AdditiveIdentity;
            foreach (var item in source)
                sum += item;
            return sum;
        }
    }
}
