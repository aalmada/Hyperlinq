using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq;

/// <summary>
/// Generic value-type collection wrapper for any ICollection&lt;T&gt; providing a value-type enumerator and Count property.
/// Implements IValueReadOnlyCollection and ICollection (read-only).
/// </summary>
public readonly struct ValueReadOnlyCollectionWrapper<TCollection, TEnumerator, TGetEnumerator, TSource>
    : IValueReadOnlyCollection<TSource, TEnumerator>, ICollection<TSource>
    where TCollection : ICollection<TSource>
    where TEnumerator : struct, IEnumerator<TSource>
    where TGetEnumerator : struct, IFunction<TCollection, TEnumerator>
{
    readonly TCollection source;
    readonly TGetEnumerator getEnumerator;

    public ValueReadOnlyCollectionWrapper(TCollection source, TGetEnumerator getEnumerator)
    {
        this.source = source ?? throw new ArgumentNullException(nameof(source));
        this.getEnumerator = getEnumerator;
    }

    internal TCollection Source => source;

    public int Count => source.Count;

    public TEnumerator GetEnumerator() => getEnumerator.Invoke(source);
    IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => getEnumerator.Invoke(source);
    IEnumerator IEnumerable.GetEnumerator() => getEnumerator.Invoke(source);

    bool ICollection<TSource>.IsReadOnly => true;

    public void CopyTo(TSource[] array, int arrayIndex) => source.CopyTo(array, arrayIndex);

    public bool Contains(TSource item) => source.Contains(item);

    void ICollection<TSource>.Add(TSource item) => throw new NotSupportedException();
    void ICollection<TSource>.Clear() => throw new NotSupportedException();
    bool ICollection<TSource>.Remove(TSource item) => throw new NotSupportedException();
}
