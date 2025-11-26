using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Returns an option containing the first element of a sequence, or None if the sequence is empty.
        /// </summary>
        public static Option<TSource> FirstOrNone<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            // Optimize for IList<T> - O(1) access via Count and indexer
            if (source is IList<TSource> list)
            {
                if (list.Count == 0)
                    return Option<TSource>.None();
                return Option<TSource>.Some(list[0]);
            }
            
            using var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
                return Option<TSource>.Some(enumerator.Current);
            
            return Option<TSource>.None();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this ArrayValueEnumerable<T> source)
        {
            if (source.Count == 0)
                return Option<T>.None();
            return Option<T>.Some(source[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this ListValueEnumerable<T> source)
        {
            if (source.Count == 0)
                return Option<T>.None();
            return Option<T>.Some(source[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this EnumerableValueEnumerable<T> source)
        {
            using var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
                return Option<T>.Some(enumerator.Current);
            return Option<T>.None();
        }

        public static Option<TSource> FirstOrNone<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => ValueEnumerableExtensions.FirstOrNone<WhereEnumerable<TSource>, WhereEnumerable<TSource>.Enumerator, TSource>(
                ValueEnumerableExtensions.Where<TEnumerable, TEnumerator, TSource>(source, predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this ArrayValueEnumerable<T> source, Func<T, bool> predicate)
            => source.Where(predicate).FirstOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this ListValueEnumerable<T> source, Func<T, bool> predicate)
            => source.Where(predicate).FirstOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this EnumerableValueEnumerable<T> source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.Where<EnumerableValueEnumerable<T>, EnumerableValueEnumerable<T>.Enumerator, T>(source, predicate).FirstOrNone();

        public static Option<TSource> FirstOrNone<TSource>(this WhereEnumerable<TSource> source)
        {
            using var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
                return Option<TSource>.Some(enumerator.Current);
            return Option<TSource>.None();
        }

        public static Option<TSource> FirstOrNone<TSource>(this WhereMemoryEnumerable<TSource> source)
        {
            using var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
                return Option<TSource>.Some(enumerator.Current);
            return Option<TSource>.None();
        }

        public static Option<TSource> FirstOrNone<TSource>(this WhereListEnumerable<TSource> source)
        {
            using var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
                return Option<TSource>.Some(enumerator.Current);
            return Option<TSource>.None();
        }
    }    
}
