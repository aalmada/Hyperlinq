using System;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq;

public static partial class ValueEnumerableExtensions
{
    /// <summary>
    /// Projects each element of a sequence into a new form.
    /// </summary>
    public static SelectEnumerable<TSource, TResult> Select<TEnumerable, TEnumerator, TSource, TResult>(
        this TEnumerable source,
        Func<TSource, TResult> selector)
        where TEnumerable : IValueEnumerable<TSource, TEnumerator>
        where TEnumerator : struct, IEnumerator<TSource> => new SelectEnumerable<TSource, TResult>(source, selector);

    /// <summary>
    /// Projects each element of a collection into a new form.
    /// Preserves Count property for O(1) access.
    /// </summary>
    public static SelectCollectionEnumerable<TEnumerator, TSource, TResult> Select<TEnumerator, TSource, TResult>(
        this IValueReadOnlyCollection<TSource, TEnumerator> source,
        Func<TSource, TResult> selector)
        where TEnumerator : struct, IEnumerator<TSource> => new SelectCollectionEnumerable<TEnumerator, TSource, TResult>(source, selector);


}
