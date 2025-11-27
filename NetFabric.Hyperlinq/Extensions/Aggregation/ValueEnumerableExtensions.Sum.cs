using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
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


        public static TSource Sum<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
        {
            var sum = TSource.AdditiveIdentity;
            foreach (var item in source)
            {
                if (predicate(item))
                    sum += item;
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this ArrayValueEnumerable<T> source, Func<T, bool> predicate)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            var sum = T.AdditiveIdentity;
            foreach (var item in source)
            {
                if (predicate(item))
                    sum += item;
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this ListValueEnumerable<T> source, Func<T, bool> predicate)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            var sum = T.AdditiveIdentity;
            foreach (var item in source)
            {
                if (predicate(item))
                    sum += item;
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this EnumerableValueEnumerable<T> source, Func<T, bool> predicate)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            var sum = T.AdditiveIdentity;
            foreach (var item in source)
            {
                if (predicate(item))
                    sum += item;
            }
            return sum;
        }

        public static TSource Sum<TSource>(this WhereEnumerable<TSource> source)
            where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
        {
            var sum = TSource.AdditiveIdentity;
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                sum += enumerator.Current;
            return sum;
        }
    }
}
