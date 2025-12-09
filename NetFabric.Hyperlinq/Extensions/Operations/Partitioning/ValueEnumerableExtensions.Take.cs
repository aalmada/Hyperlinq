using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SkipTakeEnumerable<TSource, TEnumerable, TEnumerator> Take<TEnumerable, TEnumerator, TSource>(
            this TEnumerable source,
            int count)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            return new SkipTakeEnumerable<TSource, TEnumerable, TEnumerator>(source, 0, count);
        }
    }
}
