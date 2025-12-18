using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ValueEnumerableExtensions
{
    /// <summary>
    /// Returns the number of elements in a sequence.
    /// Optimized for ICollection to use Count property.
    /// </summary>
    public static int Count<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
        where TEnumerable : IValueEnumerable<TSource, TEnumerator>
        where TEnumerator : struct, IEnumerator<TSource>
    {
        // Optimize for ICollection (includes IValueReadOnlyCollection implementations)
        if (source is ICollection<TSource> collection)
        {
            return collection.Count;
        }

        // Fallback to enumeration
        var count = 0;
        foreach (var _ in source)
        {
            count++;
        }

        return count;
    }

    /// <summary>
    /// Returns the number of elements in a collection.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count<T>(this ICollection<T> source)
            => source.Count;

    public static int Count<TEnumerable, TEnumerator, TSource, TPredicate>(this TEnumerable source, TPredicate predicate)
        where TEnumerable : IValueEnumerable<TSource, TEnumerator>
        where TEnumerator : struct, IEnumerator<TSource>
        where TPredicate : struct, IFunction<TSource, bool>
    {
        var count = 0;
        foreach (var item in source)
        {
            var result = predicate.Invoke(item);
            count += System.Runtime.CompilerServices.Unsafe.As<bool, byte>(ref result);
        }
        return count;
    }

    public static int Count<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
        where TEnumerable : IValueEnumerable<TSource, TEnumerator>
        where TEnumerator : struct, IEnumerator<TSource>
        => Count<TEnumerable, TEnumerator, TSource, FunctionWrapper<TSource, bool>>(source, new FunctionWrapper<TSource, bool>(predicate));


}
