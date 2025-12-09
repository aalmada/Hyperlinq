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

        public static Option<TSource> FirstOrNone<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                    return Option<TSource>.Some(enumerator.Current);
            }
            return Option<TSource>.None();
        }
        

    }    
}
