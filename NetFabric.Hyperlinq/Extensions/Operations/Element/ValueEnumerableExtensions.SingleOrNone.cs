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

        public static Option<TSource> SingleOrNone<TEnumerable, TEnumerator, TSource, TPredicate>(this TEnumerable source, TPredicate predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TPredicate : struct, IFunction<TSource, bool>
        {
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate.Invoke(enumerator.Current))
                {
                    var first = enumerator.Current;
                    while (enumerator.MoveNext())
                    {
                        if (predicate.Invoke(enumerator.Current))
                            throw new InvalidOperationException("Sequence contains more than one matching element");
                    }
                    return Option<TSource>.Some(first);
                }
            }
            return Option<TSource>.None();
        }

        public static Option<TSource> SingleOrNone<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => SingleOrNone<TEnumerable, TEnumerator, TSource, FunctionWrapper<TSource, bool>>(source, new FunctionWrapper<TSource, bool>(predicate));
        

    }
}
