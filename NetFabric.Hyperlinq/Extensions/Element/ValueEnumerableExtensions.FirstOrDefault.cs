using System;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        public static TSource FirstOrDefault<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => source.FirstOrNone<TEnumerable, TEnumerator, TSource>().GetValueOrDefault();

        /// <summary>
        /// Returns the first element of a sequence, or a specified default value if the sequence contains no elements.
        /// </summary>
        public static TSource FirstOrDefault<TEnumerable, TEnumerator, TSource>(this TEnumerable source, TSource defaultValue)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => source.FirstOrNone<TEnumerable, TEnumerator, TSource>().GetValueOrDefault(defaultValue);
    }
}
