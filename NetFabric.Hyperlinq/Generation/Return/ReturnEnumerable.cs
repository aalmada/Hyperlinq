using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public readonly partial struct ReturnEnumerable<T>
    : IValueReadOnlyList<T, ReturnEnumerable<T>.Enumerator>, IList<T>
{
    readonly T value;

    internal ReturnEnumerable(T value) => this.value = value;

    public int Count => 1;

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return value;
        }
    }

    T IList<T>.this[int index]
    {
        get => this[index];
        set => throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator() => new Enumerator(in this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(in this);
    IEnumerator IEnumerable.GetEnumerator() => new Enumerator(in this);

    public struct Enumerator
        : IEnumerator<T>
    {
        T? current;
        bool enumerated;

        internal Enumerator(in ReturnEnumerable<T> enumerable)
        {
            current = enumerable.value;
            enumerated = false;
        }

        public readonly T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => current!;
        }

        readonly object? IEnumerator.Current => current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            if (!enumerated)
            {
                enumerated = true;
                return true;
            }
            return false;
        }

        public void Reset() => enumerated = false;

        public void Dispose() { }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Any() => true;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(T item) => EqualityComparer<T>.Default.Equals(value, item);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(T item) => EqualityComparer<T>.Default.Equals(value, item) ? 0 : -1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (arrayIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        }

        if (array.Length - arrayIndex < 1)
        {
            throw new ArgumentException("Destination array is not long enough.");
        }

        array[arrayIndex] = value;
    }

    bool ICollection<T>.IsReadOnly => true;
    void ICollection<T>.Add(T item) => throw new NotSupportedException();
    void ICollection<T>.Clear() => throw new NotSupportedException();
    bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
    void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
    void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] ToArray() => [value];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<T> ToList() => [value];
}
