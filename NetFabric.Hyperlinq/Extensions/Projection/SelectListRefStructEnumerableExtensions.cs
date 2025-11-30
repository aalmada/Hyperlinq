using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class SelectListRefStructEnumerableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
            => source.Source.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
            => source.Source.Count > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult First<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
        {
            var list = source.Source;
            if (list.Count == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            return source.Selector(list[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> FirstOrNone<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
        {
            var list = source.Source;
            return list.Count == 0 ? Option<TResult>.None() : Option<TResult>.Some(source.Selector(list[0]));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Single<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
        {
            var list = source.Source;
            if (list.Count == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            if (list.Count > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return source.Selector(list[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> SingleOrNone<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
        {
            var list = source.Source;
            if (list.Count == 0)
                return Option<TResult>.None();
            if (list.Count > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<TResult>.Some(source.Selector(list[0]));
        }

        public static TResult[] ToArray<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
        {
            var list = source.Source;
            var selector = source.Selector;
            var array = new TResult[list.Count];
            var span = CollectionsMarshal.AsSpan(list);
            for (var i = 0; i < span.Length; i++)
                array[i] = selector(span[i]);
            return array;
        }

        public static List<TResult> ToList<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
        {
            var list = source.Source;
            var selector = source.Selector;
            var result = new List<TResult>(list.Count);
            var span = CollectionsMarshal.AsSpan(list);
            for (var i = 0; i < span.Length; i++)
                result.Add(selector(span[i]));
            return result;
        }
    }
}
