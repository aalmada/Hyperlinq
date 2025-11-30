using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class SelectArrayRefStructEnumerableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource, TResult>(this SelectArrayRefStructEnumerable<TSource, TResult> source)
            => source.Source.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TSource, TResult>(this SelectArrayRefStructEnumerable<TSource, TResult> source)
            => source.Source.Length > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult First<TSource, TResult>(this SelectArrayRefStructEnumerable<TSource, TResult> source)
        {
            var array = source.Source;
            if (array.Length == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            return source.Selector(array[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> FirstOrNone<TSource, TResult>(this SelectArrayRefStructEnumerable<TSource, TResult> source)
        {
            var array = source.Source;
            return array.Length == 0 ? Option<TResult>.None() : Option<TResult>.Some(source.Selector(array[0]));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Single<TSource, TResult>(this SelectArrayRefStructEnumerable<TSource, TResult> source)
        {
            var array = source.Source;
            if (array.Length == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            if (array.Length > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return source.Selector(array[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> SingleOrNone<TSource, TResult>(this SelectArrayRefStructEnumerable<TSource, TResult> source)
        {
            var array = source.Source;
            if (array.Length == 0)
                return Option<TResult>.None();
            if (array.Length > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<TResult>.Some(source.Selector(array[0]));
        }

        public static TResult[] ToArray<TSource, TResult>(this SelectArrayRefStructEnumerable<TSource, TResult> source)
        {
            var array = source.Source;
            var selector = source.Selector;
            var result = new TResult[array.Length];
            for (var i = 0; i < array.Length; i++)
                result[i] = selector(array[i]);
            return result;
        }

        public static List<TResult> ToList<TSource, TResult>(this SelectArrayRefStructEnumerable<TSource, TResult> source)
        {
            var array = source.Source;
            var selector = source.Selector;
            var list = new List<TResult>(array.Length);
            for (var i = 0; i < array.Length; i++)
                list.Add(selector(array[i]));
            return list;
        }
    }
}
