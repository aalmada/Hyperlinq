using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereSelectListEnumerableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var count = 0;
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    count++;
            }
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult First<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    return selector(span[i]);
            }
            throw new InvalidOperationException("Sequence contains no elements");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> FirstOrNone<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    return Option<TResult>.Some(selector(span[i]));
            }
            return Option<TResult>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Single<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var found = false;
            var result = default(TResult);
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                {
                    if (found)
                        throw new InvalidOperationException("Sequence contains more than one element");
                    result = selector(span[i]);
                    found = true;
                }
            }
            if (!found)
                throw new InvalidOperationException("Sequence contains no elements");
            return result!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> SingleOrNone<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
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

        public static TResult[] ToArray<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var list = new List<TResult>();
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    list.Add(selector(span[i]));
            }
            return list.ToArray();
        }

        public static List<TResult> ToList<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var list = new List<TResult>();
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    list.Add(selector(span[i]));
            }
            return list;
        }
    }
}
