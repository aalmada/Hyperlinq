using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public readonly ref struct RepeatReadOnlySpanEnumerable<TSource>
{
    readonly ReadOnlySpan<TSource> source;
    readonly int count;

    internal RepeatReadOnlySpanEnumerable(ReadOnlySpan<TSource> source, int count)
    {
        this.source = source;
        this.count = count;
    }

    public readonly int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (count == 0)
            {
                return 0;
            }

            var sourceCount = source.Length;
            if (sourceCount == 0)
            {
                return 0;
            }

            try
            {
                return checked(sourceCount * count);
            }
            catch (OverflowException)
            {
                throw new OverflowException("The number of elements in the repeated sequence exceeds Int32.MaxValue.");
            }
        }
    }

    public readonly TSource this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return source[index % source.Length];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Enumerator GetEnumerator() => new Enumerator(source, count);

    public ref struct Enumerator
    {
        readonly ReadOnlySpan<TSource> source;
        int remaining;
        int index;

        internal Enumerator(ReadOnlySpan<TSource> source, int count)
        {
            this.source = source;
            remaining = checked(source.Length * count);
            index = -1;
        }

        public readonly TSource Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => source[index % source.Length];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            if (remaining == 0)
            {
                return false;
            }

            remaining--;
            index++;
            return true;
        }
    }

    // Optimize ToArray, ToList, etc.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TSource[] ToArray()
    {
        var length = Count;
        if (length == 0)
        {
            return Array.Empty<TSource>();
        }

        var result = GC.AllocateUninitializedArray<TSource>(length);
        CopyTo(result);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PooledBuffer<TSource> ToArrayPooled(ArrayPool<TSource>? pool = null)
    {
        var length = Count;
        if (length == 0)
        {
            return PooledBuffer.Empty<TSource>();
        }

        pool ??= ArrayPool<TSource>.Shared;
        var buffer = pool.Rent(length);
        CopyTo(buffer.AsSpan(0, length));
        return new PooledBuffer<TSource>(buffer, length, pool);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<TSource> ToList()
    {
        var length = Count;
        if (length == 0)
        {
            return new List<TSource>();
        }

        var result = new List<TSource>(length);
        CollectionsMarshal.SetCount(result, length);
        CopyTo(CollectionsMarshal.AsSpan(result));
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(Span<TSource> destination)
    {
        if (destination.Length < Count)
        {
            throw new ArgumentException("Destination span is not long enough.");
        }

        var destSpan = destination.Slice(0, Count);
        for (int i = 0; i < count; i++)
        {
            source.CopyTo(destSpan.Slice(i * source.Length, source.Length));
        }
    }
}
