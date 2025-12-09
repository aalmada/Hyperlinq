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
        public static ValueEnumerableWrapper<IEnumerable<TSource>, TEnumerator, TGetEnumerator, TSource> AsValueEnumerable<TSource, TEnumerator, TGetEnumerator>(this IEnumerable<TSource> source, TGetEnumerator getEnumerator)
            where TEnumerator : struct, IEnumerator<TSource>
            where TGetEnumerator : struct, IFunction<IEnumerable<TSource>, TEnumerator>
            => new(source, getEnumerator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueEnumerableWrapper<IEnumerable<TSource>, TEnumerator, FunctionWrapper<IEnumerable<TSource>, TEnumerator>, TSource> AsValueEnumerable<TSource, TEnumerator>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, TEnumerator> getEnumerator)
            where TEnumerator : struct, IEnumerator<TSource>
            => new(source, new FunctionWrapper<IEnumerable<TSource>, TEnumerator>(getEnumerator));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueReadOnlyCollectionWrapper<ICollection<TSource>, TEnumerator, TGetEnumerator, TSource> AsValueEnumerable<TSource, TEnumerator, TGetEnumerator>(this ICollection<TSource> source, TGetEnumerator getEnumerator)
            where TEnumerator : struct, IEnumerator<TSource>
            where TGetEnumerator : struct, IFunction<ICollection<TSource>, TEnumerator>
            => new(source, getEnumerator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueReadOnlyCollectionWrapper<ICollection<TSource>, TEnumerator, FunctionWrapper<ICollection<TSource>, TEnumerator>, TSource> AsValueEnumerable<TSource, TEnumerator>(this ICollection<TSource> source, Func<ICollection<TSource>, TEnumerator> getEnumerator)
            where TEnumerator : struct, IEnumerator<TSource>
            => new(source, new FunctionWrapper<ICollection<TSource>, TEnumerator>(getEnumerator));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueReadOnlyListWrapper<IList<TSource>, TEnumerator, TGetEnumerator, TSource> AsValueEnumerable<TSource, TEnumerator, TGetEnumerator>(this IList<TSource> source, TGetEnumerator getEnumerator)
            where TEnumerator : struct, IEnumerator<TSource>
            where TGetEnumerator : struct, IFunction<IList<TSource>, TEnumerator>
            => new(source, getEnumerator);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueReadOnlyListWrapper<IList<TSource>, TEnumerator, FunctionWrapper<IList<TSource>, TEnumerator>, TSource> AsValueEnumerable<TSource, TEnumerator>(this IList<TSource> source, Func<IList<TSource>, TEnumerator> getEnumerator)
            where TEnumerator : struct, IEnumerator<TSource>
            => new(source, new FunctionWrapper<IList<TSource>, TEnumerator>(getEnumerator));
    }
}
