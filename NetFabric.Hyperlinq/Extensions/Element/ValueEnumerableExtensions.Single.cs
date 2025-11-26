using System;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Returns the only element of a sequence, and throws an exception if there is not exactly one element.
        /// </summary>
        public static TSource Single<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => source.SingleOrNone<TEnumerable, TEnumerator, TSource>().Value;
    }
}
