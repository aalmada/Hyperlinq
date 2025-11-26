using System;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        public static WhereEnumerable<TSource> Where<TEnumerable, TEnumerator, TSource>(
            this TEnumerable source,
            Func<TSource, bool> predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            return new WhereEnumerable<TSource>(source, predicate);
        }

        public static WhereMemoryEnumerable<TSource> Where<TSource>(
            this ArrayValueEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return new WhereMemoryEnumerable<TSource>(source.Source, predicate);
        }

        public static WhereListEnumerable<TSource> Where<TSource>(
            this ListValueEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            return new WhereListEnumerable<TSource>(source.Source, predicate);
        }
    }
}
