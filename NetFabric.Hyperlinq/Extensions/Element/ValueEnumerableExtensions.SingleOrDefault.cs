using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
        /// </summary>
        public static TSource SingleOrDefault<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => source.SingleOrNone<TEnumerable, TEnumerator, TSource>().GetValueOrDefault();

        /// <summary>
        /// Returns the only element of a sequence, or a specified default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
        /// </summary>
        public static TSource SingleOrDefault<TEnumerable, TEnumerator, TSource>(this TEnumerable source, TSource defaultValue)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => source.SingleOrNone<TEnumerable, TEnumerator, TSource>().GetValueOrDefault(defaultValue);
        /// <summary>
        /// Returns the only element of a sequence that satisfies a specified condition, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
        /// </summary>
        public static TSource SingleOrDefault<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => ValueEnumerableExtensions.SingleOrNone<TEnumerable, TEnumerator, TSource>(source, predicate).GetValueOrDefault();

        /// <summary>
        /// Returns the only element of a sequence that satisfies a specified condition, or a specified default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
        /// </summary>
        public static TSource SingleOrDefault<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate, TSource defaultValue)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => ValueEnumerableExtensions.SingleOrNone<TEnumerable, TEnumerator, TSource>(source, predicate).GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ArrayValueEnumerable<T> source, Func<T, bool> predicate)
            => source.SingleOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ArrayValueEnumerable<T> source, Func<T, bool> predicate, T defaultValue)
            => source.SingleOrNone(predicate).GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ListValueEnumerable<T> source, Func<T, bool> predicate)
            => source.SingleOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ListValueEnumerable<T> source, Func<T, bool> predicate, T defaultValue)
            => source.SingleOrNone(predicate).GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this EnumerableValueEnumerable<T> source, Func<T, bool> predicate)
            => source.SingleOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this EnumerableValueEnumerable<T> source, Func<T, bool> predicate, T defaultValue)
            => source.SingleOrNone(predicate).GetValueOrDefault(defaultValue);

        public static TSource SingleOrDefault<TSource>(this WhereEnumerable<TSource> source)
            => source.SingleOrNone().GetValueOrDefault();

        public static TSource SingleOrDefault<TSource>(this WhereEnumerable<TSource> source, TSource defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);

        public static TSource SingleOrDefault<TSource>(this WhereMemoryEnumerable<TSource> source)
            => source.SingleOrNone().GetValueOrDefault();

        public static TSource SingleOrDefault<TSource>(this WhereMemoryEnumerable<TSource> source, TSource defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);

        public static TSource SingleOrDefault<TSource>(this WhereListEnumerable<TSource> source)
            => source.SingleOrNone().GetValueOrDefault();

        public static TSource SingleOrDefault<TSource>(this WhereListEnumerable<TSource> source, TSource defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);
    }
}
