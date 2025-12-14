using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq;

/// <summary>
/// Value-type enumerable wrapper for List&lt;T&gt;.
/// Uses List&lt;T&gt;.Enumerator which is a value type.
/// Implements IValueReadOnlyList to expose Count and indexer.
/// </summary>
public readonly struct ListValueEnumerable<T> : IValueReadOnlyList<T, List<T>.Enumerator>, IList<T>
{
    readonly List<T> source;

    public ListValueEnumerable(List<T> source) => this.source = source ?? throw new ArgumentNullException(nameof(source));

    internal List<T> Source => source;

    public int Count => source.Count;
    public T this[int index] => source[index];
    T IList<T>.this[int index]
    {
        get => source[index];
        set => throw new NotSupportedException();
    }

    public List<T>.Enumerator GetEnumerator() => source.GetEnumerator();
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => source.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();

    bool ICollection<T>.IsReadOnly => true;

    public void CopyTo(T[] array, int arrayIndex) => source.CopyTo(array, arrayIndex);

    public bool Contains(T item) => source.Contains(item);

    public int IndexOf(T item) => source.IndexOf(item);

    void ICollection<T>.Add(T item) => throw new NotSupportedException();
    void ICollection<T>.Clear() => throw new NotSupportedException();
    bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
    void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
    void IList<T>.RemoveAt(int index) => throw new NotSupportedException();
}
