using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public readonly partial struct RangeEnumerable
    : IValueReadOnlyList<int, RangeEnumerable.Enumerator>, IList<int>
{
    readonly int start;
    readonly int count;

    internal RangeEnumerable(int start, int count)
    {
        this.start = start;
        this.count = count;
    }

    public int Count => count;

    public int this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => start + index;
    }

    int IList<int>.this[int index]
    {
        get => this[index];
        set => throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator() => new Enumerator(in this);

    IEnumerator<int> IEnumerable<int>.GetEnumerator() => new Enumerator(in this);
    IEnumerator IEnumerable.GetEnumerator() => new Enumerator(in this);

    public struct Enumerator
        : IEnumerator<int>
    {
        int current;
        readonly int end;

        internal Enumerator(in RangeEnumerable enumerable)
        {
            current = enumerable.start - 1;
            end = enumerable.start + enumerable.count;
        }

        public int Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => current;
        }

        object IEnumerator.Current => current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            current++;
            return current < end;
        }

        public void Reset() => throw new NotSupportedException();

        public void Dispose() { }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Fill(Span<int> span)
    {
        var index = 0;

        if (Vector.IsHardwareAccelerated && span.Length >= Vector<int>.Count)
        {
            // Initialize vector with sequence [start, start+1, ..., start+N-1]
            Span<int> init = stackalloc int[Vector<int>.Count];
            for (var i = 0; (uint)i < (uint)init.Length; i++)
            {
                init[i] = start + i;
            }
            var vector = new Vector<int>(init);
            var increment = new Vector<int>(Vector<int>.Count);

            ref var destination = ref MemoryMarshal.GetReference(span);

            while (span.Length - index >= Vector<int>.Count)
            {
                vector.StoreUnsafe(ref destination, (nuint)index);
                vector += increment;
                index += Vector<int>.Count;
            }
        }

        for (; (uint)index < (uint)span.Length; index++)
        {
            span[index] = start + index;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Any() => count > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(int value)
        => value >= start && value < start + count;

    public int IndexOf(int item)
    {
        if (item >= start && item < start + count)
        {
            return item - start;
        }

        return -1;
    }

    public void CopyTo(int[] array, int arrayIndex)
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

    bool ICollection<int>.IsReadOnly => true;
    void ICollection<int>.Add(int item) => throw new NotSupportedException();
    void ICollection<int>.Clear() => throw new NotSupportedException();
    bool ICollection<int>.Remove(int item) => throw new NotSupportedException();
    void IList<int>.Insert(int index, int item) => throw new NotSupportedException();
    void IList<int>.RemoveAt(int index) => throw new NotSupportedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int[] ToArray()
    {
        if (count == 0)
        {
            return [];
        }

        var result = GC.AllocateUninitializedArray<int>(count);
        Fill(result.AsSpan());
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<int> ToList()
    {
        var result = new List<int>(count);
        CollectionsMarshal.SetCount(result, count);
        Fill(CollectionsMarshal.AsSpan(result));
        return result;
    }
}
