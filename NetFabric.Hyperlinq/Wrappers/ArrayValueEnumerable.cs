using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq;

/// <summary>
/// Value-type enumerable wrapper for arrays.
/// Uses ArrayEnumerator&lt;T&gt; which is a value type.
/// Implements IValueReadOnlyList to expose Count and indexer.
/// </summary>
public readonly struct ArrayValueEnumerable<T> : IValueReadOnlyList<T, ArrayValueEnumerable<T>.Enumerator>, IList<T>
{
    private readonly T[] source;

    public ArrayValueEnumerable(T[] source) => this.source = source ?? throw new ArgumentNullException(nameof(source));

    internal T[] Source => source;

    public int Count => source.Length;
    public T this[int index] => source[index];
    T IList<T>.this[int index]
    {
        get => source[index];
        set => throw new NotSupportedException();
    }

    public Enumerator GetEnumerator() => new Enumerator(source);
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    bool ICollection<T>.IsReadOnly => true;

    public void CopyTo(T[] array, int arrayIndex) => source.CopyTo(array, arrayIndex);

    public bool Contains(T item) => ((IList<T>)source).Contains(item);

    public int IndexOf(T item) => Array.IndexOf(source, item);

    void ICollection<T>.Add(T item) => throw new NotSupportedException();
    void ICollection<T>.Clear() => throw new NotSupportedException();
    bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
    void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
    void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

    public struct Enumerator : IEnumerator<T>
    {
        private readonly T[] array;
        private int index;

        public Enumerator(T[] array)
        {
            this.array = array;
            this.index = -1;
        }

        public T Current => array[index];
        object? IEnumerator.Current => Current;

        public bool MoveNext() => ++index < array.Length;

        public void Reset() => index = -1;

        public void Dispose() { }
    }
}
