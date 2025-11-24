using System;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// Optimized for IValueReadOnlyCollection to use Count property.
        /// </summary>
        public static int Count<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            // Optimize for IValueReadOnlyCollection
            if (source is IValueReadOnlyCollection<TSource, TEnumerator> collection)
                return collection.Count;

            // Fallback to enumeration
            var count = 0;
            foreach (var _ in source)
                count++;
            return count;
        }
    }
}
