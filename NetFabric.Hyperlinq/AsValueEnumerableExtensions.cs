using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Extension methods to convert sources to IValueEnumerable for optimized enumeration.
    /// </summary>
    public static class AsValueEnumerableExtensions
    {
        /// <summary>
        /// Converts a List&lt;T&gt; to a value-type enumerable.
        /// Uses List&lt;T&gt;.Enumerator which is a value type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ListValueEnumerable<T> AsValueEnumerable<T>(this List<T> source)
            => new ListValueEnumerable<T>(source);

        /// <summary>
        /// Converts an array to a value-type enumerable.
        /// Uses a custom value-type enumerator.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArrayValueEnumerable<T> AsValueEnumerable<T>(this T[] source)
            => new ArrayValueEnumerable<T>(source);

        /// <summary>
        /// Converts an IEnumerable&lt;T&gt; to a value-type enumerable wrapper.
        /// This is a fallback for types that don't have specific overloads.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EnumerableValueEnumerable<T> AsValueEnumerable<T>(this IEnumerable<T> source)
            => new EnumerableValueEnumerable<T>(source);
    }
}
