using System;
using System.Collections.Generic;

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
    }
}
