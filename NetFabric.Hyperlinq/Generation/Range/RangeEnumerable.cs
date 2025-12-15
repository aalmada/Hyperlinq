using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
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

        var span = array.AsSpan(arrayIndex, count);
        for (var i = 0; i < count; i++)
        {
            span[i] = start + i;
        }
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
            return Array.Empty<int>();
        }

        var result = GC.AllocateUninitializedArray<int>(count);
        var span = result.AsSpan();
        for (var i = 0; i < count; i++)
        {
            span[i] = start + i;
        }
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<int> ToList()
    {
        var result = new List<int>(count);
        CollectionsMarshal.SetCount(result, count);
        var span = CollectionsMarshal.AsSpan(result);
        for (var i = 0; i < count; i++)
        {
            span[i] = start + i;
        }
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PooledBuffer<int> ToArrayPooled(ArrayPool<int>? pool = null)
    {
        pool ??= ArrayPool<int>.Shared;
        var result = pool.Rent(count);
        if (count > 0)
        {
            var span = result.AsSpan(0, count);
            for (var i = 0; i < count; i++)
            {
                span[i] = start + i;
            }
        }
        return new PooledBuffer<int>(result, count, pool);
    }


}
