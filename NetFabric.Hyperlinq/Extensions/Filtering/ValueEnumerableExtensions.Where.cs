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


    }
}
