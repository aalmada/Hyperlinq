using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        // ---------------------------------------------------------------------------------
        // Count
        // ---------------------------------------------------------------------------------

        /// <summary>
        /// Returns the number of elements that satisfy the predicate.
        /// Optimized to ignore the selector since Count doesn't need projected values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource, TResult>(this WhereSelectEnumerable<TSource, TResult> source)
        {
            var count = 0;
            foreach (var item in source.Source)
            {
                if (source.Predicate(item))
                    count++;
            }
            return count;
        }
        /// Uses CollectionsMarshal for zero-copy access.
        /// </summary>

    }
}
