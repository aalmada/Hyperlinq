using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Returns an option containing the only element of a sequence, or None if the sequence is empty.
        /// Throws an exception if there is more than one element in the sequence.
        /// </summary>
        /// <exception cref="InvalidOperationException">The sequence contains more than one element.</exception>
        public static Option<TSource> SingleOrNone<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            // Optimize for IList<T> - O(1) access via Count and indexer
            if (source is IList<TSource> list)
            {
                if (list.Count == 0)
                    return Option<TSource>.None();
                if (list.Count > 1)
                    throw new InvalidOperationException("Sequence contains more than one element");
                return Option<TSource>.Some(list[0]);
            }
            
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                return Option<TSource>.None();
            
            var first = enumerator.Current;
            if (enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains more than one element");

            return Option<TSource>.Some(first);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this ArrayValueEnumerable<T> source)
        {
            if (source.Count == 0)
                return Option<T>.None();
            if (source.Count > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<T>.Some(source[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this ListValueEnumerable<T> source)
        {
            if (source.Count == 0)
                return Option<T>.None();
            if (source.Count > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<T>.Some(source[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this EnumerableValueEnumerable<T> source)
        {
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                return Option<T>.None();
            
            var first = enumerator.Current;
            if (enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains more than one element");

            return Option<T>.Some(first);
        }


        public static Option<TSource> SingleOrNone<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => ValueEnumerableExtensions.SingleOrNone<WhereEnumerable<TSource>, WhereEnumerable<TSource>.Enumerator, TSource>(
                ValueEnumerableExtensions.Where<TEnumerable, TEnumerator, TSource>(source, predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this ArrayValueEnumerable<T> source, Func<T, bool> predicate)
            => source.Where(predicate).SingleOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this ListValueEnumerable<T> source, Func<T, bool> predicate)
            => source.Where(predicate).SingleOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this EnumerableValueEnumerable<T> source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.Where<EnumerableValueEnumerable<T>, EnumerableValueEnumerable<T>.Enumerator, T>(source, predicate).SingleOrNone();

        public static Option<TSource> SingleOrNone<TSource>(this WhereEnumerable<TSource> source)
        {
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                return Option<TSource>.None();
            
            var first = enumerator.Current;
            if (enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains more than one element");

            return Option<TSource>.Some(first);
        }

        public static Option<TSource> SingleOrNone<TSource>(this WhereMemoryEnumerable<TSource> source)
        {
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                return Option<TSource>.None();
            
            var first = enumerator.Current;
            if (enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains more than one element");

            return Option<TSource>.Some(first);
        }

        public static Option<TSource> SingleOrNone<TSource>(this WhereListEnumerable<TSource> source)
        {
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                return Option<TSource>.None();
            
            var first = enumerator.Current;
            if (enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains more than one element");

            return Option<TSource>.Some(first);
        }
    }
}
