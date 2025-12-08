using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        // ---------------------------------------------------------------------------------
        // Any
        // ---------------------------------------------------------------------------------

        /// <summary>
        /// Determines whether any element satisfies the predicate.
        /// Optimized to ignore the selector since Any doesn't need projected values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TSource, TResult>(this WhereSelectEnumerable<TSource, TResult> source)
        {
            foreach (var item in source.Source)
            {
                if (source.Predicate(item))
                    return true;
            }
            return false;
        }




    }
}
