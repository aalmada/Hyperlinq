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



        /// <summary>
        /// Determines whether any element satisfies the predicate.
        /// Optimized to ignore the selector since Any doesn't need projected values.
        /// Uses CollectionsMarshal for zero-copy access.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            ref var spanRef = ref MemoryMarshal.GetReference(span);
            var length = span.Length;

            for (var i = 0; i < length; i++)
            {
                if (source.Predicate(Unsafe.Add(ref spanRef, i)))
                    return true;
            }
            return false;
        }
    }
}
