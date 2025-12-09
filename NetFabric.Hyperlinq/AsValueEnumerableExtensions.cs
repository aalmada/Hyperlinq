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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueEnumerableWrapper<TEnumerable, TEnumerator, TGetEnumerator, TSource> AsValueEnumerable<TEnumerable, TEnumerator, TGetEnumerator, TSource>(this TEnumerable source, TGetEnumerator getEnumerator)
            where TEnumerable : IEnumerable<TSource>
            where TEnumerator : struct, IEnumerator<TSource>
            where TGetEnumerator : struct, IFunction<TEnumerable, TEnumerator>
            => new(source, getEnumerator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueEnumerableWrapper<TEnumerable, TEnumerator, FunctionWrapper<TEnumerable, TEnumerator>, TSource> AsValueEnumerable<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TEnumerable, TEnumerator> getEnumerator)
            where TEnumerable : IEnumerable<TSource>
            where TEnumerator : struct, IEnumerator<TSource>
            => new(source, new FunctionWrapper<TEnumerable, TEnumerator>(getEnumerator));
    }
}
