using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq;

/// <summary>
/// Generic value-type enumerable wrapper for any IEnumerable&lt;T&gt; providing a value-type enumerator.
/// Implements IValueEnumerable for basic enumeration support.
/// </summary>
public readonly struct ValueEnumerableWrapper<TEnumerable, TEnumerator, TGetEnumerator, TSource>
    : IValueEnumerable<TSource, TEnumerator>
    where TEnumerable : IEnumerable<TSource>
    where TEnumerator : struct, IEnumerator<TSource>
    where TGetEnumerator : struct, IFunction<TEnumerable, TEnumerator>
{
    readonly TEnumerable source;
    readonly TGetEnumerator getEnumerator;

    public ValueEnumerableWrapper(TEnumerable source, TGetEnumerator getEnumerator)
    {
        this.source = source ?? throw new ArgumentNullException(nameof(source));
        this.getEnumerator = getEnumerator;
    }

    internal TEnumerable Source => source;

    public TEnumerator GetEnumerator() => getEnumerator.Invoke(source);
    IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => getEnumerator.Invoke(source);
    IEnumerator IEnumerable.GetEnumerator() => getEnumerator.Invoke(source);
}
