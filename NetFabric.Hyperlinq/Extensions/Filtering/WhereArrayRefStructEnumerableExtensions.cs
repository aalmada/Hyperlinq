using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereArrayRefStructEnumerableExtensions
    {
        /// <summary>
        /// Computes the sum of a WhereArrayRefStructEnumerable for numeric types.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Sum<TSource>(this WhereArrayRefStructEnumerable<TSource> source)
            where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
        {
            var sum = TSource.AdditiveIdentity;
            foreach (var item in source)
            {
                sum += item;
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource>(this WhereArrayRefStructEnumerable<TSource> source)
        {
            var count = 0;
            var array = source.Source;
            var predicate = source.Predicate;
            for (var i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                    count++;
            }
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TSource>(this WhereArrayRefStructEnumerable<TSource> source)
        {
            var array = source.Source;
            var predicate = source.Predicate;
            for (var i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                    return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource First<TSource>(this WhereArrayRefStructEnumerable<TSource> source)
        {
            var array = source.Source;
            var predicate = source.Predicate;
            for (var i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                    return array[i];
            }
            throw new InvalidOperationException("Sequence contains no elements");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TSource> FirstOrNone<TSource>(this WhereArrayRefStructEnumerable<TSource> source)
        {
            var array = source.Source;
            var predicate = source.Predicate;
            for (var i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                    return Option<TSource>.Some(array[i]);
            }
            return Option<TSource>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Single<TSource>(this WhereArrayRefStructEnumerable<TSource> source)
        {
            var array = source.Source;
            var predicate = source.Predicate;
            var found = false;
            var result = default(TSource);
            for (var i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                {
                    if (found)
                        throw new InvalidOperationException("Sequence contains more than one element");
                    result = array[i];
                    found = true;
                }
            }
            if (!found)
                throw new InvalidOperationException("Sequence contains no elements");
            return result!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TSource> SingleOrNone<TSource>(this WhereArrayRefStructEnumerable<TSource> source)
        {
            var array = source.Source;
            var predicate = source.Predicate;
            var found = false;
            var result = default(TSource);
            for (var i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                {
                    if (found)
                        throw new InvalidOperationException("Sequence contains more than one matching element");
                    result = array[i];
                    found = true;
                }
            }
            return found ? Option<TSource>.Some(result!) : Option<TSource>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Last<TSource>(this WhereArrayRefStructEnumerable<TSource> source)
        {
            var array = source.Source;
            var predicate = source.Predicate;
            for (var index = array.Length - 1; index >= 0; index--)
            {
                if (predicate(array[index]))
                    return array[index];
            }
            throw new InvalidOperationException("Sequence contains no matching element");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TSource> LastOrNone<TSource>(this WhereArrayRefStructEnumerable<TSource> source)
        {
            var array = source.Source;
            var predicate = source.Predicate;
            for (var index = array.Length - 1; index >= 0; index--)
            {
                if (predicate(array[index]))
                    return Option<TSource>.Some(array[index]);
            }
            return Option<TSource>.None();
        }

        public static TSource[] ToArray<TSource>(this WhereArrayRefStructEnumerable<TSource> source)
        {
            var array = source.Source;
            var predicate = source.Predicate;
            var list = new List<TSource>();
            for (var i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                    list.Add(array[i]);
            }
            return list.ToArray();
        }

        public static List<TSource> ToList<TSource>(this WhereArrayRefStructEnumerable<TSource> source)
        {
            var array = source.Source;
            var predicate = source.Predicate;
            var list = new List<TSource>();
            for (var i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                    list.Add(array[i]);
            }
            return list;
        }
    }
}
