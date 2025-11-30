using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereSelectReadOnlySpanEnumerableExtensions
    {
        /// <summary>
        /// Computes the sum of a WhereSelectReadOnlySpanEnumerable for numeric result types.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Sum<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source)
            where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
        {
            var sum = TResult.AdditiveIdentity;
            foreach (var item in source)
            {
                sum += item;
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source)
        {
            var count = 0;
            var span = source.Source;
            var predicate = source.Predicate;
            foreach (var item in span)
            {
                if (predicate(item))
                    count++;
            }
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source)
        {
            var span = source.Source;
            var predicate = source.Predicate;
            foreach (var item in span)
            {
                if (predicate(item))
                    return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult First<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source)
            => FirstOrNone(source).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult FirstOrDefault<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source)
            => FirstOrNone(source).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult FirstOrDefault<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source, TResult defaultValue)
            => FirstOrNone(source).GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> FirstOrNone<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source)
        {
            var span = source.Source;
            var predicate = source.Predicate;
            var selector = source.Selector;
            foreach (var item in span)
            {
                if (predicate(item))
                    return Option<TResult>.Some(selector(item));
            }
            return Option<TResult>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Single<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source)
            => SingleOrNone(source).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult SingleOrDefault<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source)
            => SingleOrNone(source).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult SingleOrDefault<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source, TResult defaultValue)
            => SingleOrNone(source).GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> SingleOrNone<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source)
        {
            var span = source.Source;
            var predicate = source.Predicate;
            var selector = source.Selector;
            var found = false;
            var result = default(TResult);
            foreach (var item in span)
            {
                if (predicate(item))
                {
                    if (found)
                        throw new InvalidOperationException("Sequence contains more than one matching element");
                    
                    found = true;
                    result = selector(item);
                }
            }
            return found ? Option<TResult>.Some(result!) : Option<TResult>.None();
        }

        public static TResult[] ToArray<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source)
        {
            var list = new List<TResult>();
            var span = source.Source;
            var predicate = source.Predicate;
            var selector = source.Selector;
            foreach (var item in span)
            {
                if (predicate(item))
                    list.Add(selector(item));
            }
            return list.ToArray();
        }

        public static List<TResult> ToList<TSource, TResult>(this WhereSelectReadOnlySpanEnumerable<TSource, TResult> source)
        {
            var list = new List<TResult>();
            var span = source.Source;
            var predicate = source.Predicate;
            var selector = source.Selector;
            foreach (var item in span)
            {
                if (predicate(item))
                    list.Add(selector(item));
            }
            return list;
        }
    }
}
