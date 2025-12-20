using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

using System.Numerics;


public readonly partial struct RepeatEnumerable<T>
    : IValueReadOnlyList<T, RepeatEnumerable<T>.Enumerator>, IList<T>
{
    readonly T element;
    readonly int count;

    internal RepeatEnumerable(T element, int count)
    {
        this.element = element;
        this.count = count;
    }

    public int Count => count;

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => element;
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
        readonly T element;
        int remaining;

        internal Enumerator(in RepeatEnumerable<T> enumerable)
        {
            element = enumerable.element;
            remaining = enumerable.count + 1;
        }

        public T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => element;
        }

        object? IEnumerator.Current => element;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            remaining--;
            return remaining > 0;
        }

        public void Reset() => throw new NotSupportedException();

        public void Dispose() { }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Any() => count > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(T value)
        => count > 0 && EqualityComparer<T>.Default.Equals(element, value);

    public int IndexOf(T item)
    {
        if (count > 0 && EqualityComparer<T>.Default.Equals(element, item))
        {
            return 0;
        }

        return -1;
    }

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

        if (array.Length - arrayIndex < count)
        {
            throw new ArgumentException("Destination array is not long enough.");
        }

        Fill(array.AsSpan(arrayIndex, count));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Fill(Span<T> span)
    {
        if (Vector.IsHardwareAccelerated && Vector<T>.IsSupported && span.Length >= Vector<T>.Count)
        {
            var vector = new Vector<T>(element);
            ref var destination = ref MemoryMarshal.GetReference(span);
            var index = 0;
            while (span.Length - index >= Vector<T>.Count)
            {
                vector.StoreUnsafe(ref destination, (nuint)index);
                index += Vector<T>.Count;
            }
            span = span.Slice(index);
        }
        span.Fill(element);
    }

    bool ICollection<T>.IsReadOnly => true;
    void ICollection<T>.Add(T item) => throw new NotSupportedException();
    void ICollection<T>.Clear() => throw new NotSupportedException();
    bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
    void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
    void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] ToArray()
    {
        if (count == 0)
        {
            return [];
        }

        var result = GC.AllocateUninitializedArray<T>(count);
        Fill(result.AsSpan());
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<T> ToList()
    {
        var result = new List<T>(count);
        CollectionsMarshal.SetCount(result, count);
        Fill(CollectionsMarshal.AsSpan(result));
        return result;
    }
}
