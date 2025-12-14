using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public readonly partial struct EmptyEnumerable<T>
    : IValueReadOnlyList<T, EmptyEnumerable<T>.Enumerator>, IList<T>
{
    public int Count => 0;

    public T this[int index] => throw new ArgumentOutOfRangeException(nameof(index));

    T IList<T>.this[int index]
    {
        get => this[index];
        set => throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator() => new Enumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator();
    IEnumerator IEnumerable.GetEnumerator() => new Enumerator();

    public struct Enumerator
        : IEnumerator<T>
    {
        public T Current => default!;

        object? IEnumerator.Current => default;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() => false;

        public void Reset() { }

        public void Dispose() { }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Any() => false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(T value) => false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(T item) => -1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(T[] array, int arrayIndex) { }

    bool ICollection<T>.IsReadOnly => true;
    void ICollection<T>.Add(T item) => throw new NotSupportedException();
    void ICollection<T>.Clear() => throw new NotSupportedException();
    bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
    void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
    void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] ToArray() => Array.Empty<T>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<T> ToList() => new List<T>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PooledBuffer<T> ToArrayPooled(ArrayPool<T>? pool = null)
    {
        pool ??= ArrayPool<T>.Shared;
        return new PooledBuffer<T>(pool.Rent(0), 0, pool);
    }
}
