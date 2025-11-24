using System;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Determines whether a sequence contains any elements.
        /// Optimized for IValueReadOnlyCollection to use Count property.
        /// </summary>
        public static bool Any<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            // Optimize for IValueReadOnlyCollection
            if (source is IValueReadOnlyCollection<TSource, TEnumerator> collection)
                return collection.Count != 0;

            // Fallback to enumeration
            using var enumerator = source.GetEnumerator();
            return enumerator.MoveNext();
        }
    }
}
