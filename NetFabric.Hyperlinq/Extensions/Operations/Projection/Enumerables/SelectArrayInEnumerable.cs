using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public readonly struct SelectArrayInEnumerable<TSource, TResult, TSelector> : IValueReadOnlyList<TResult, SelectArrayInEnumerable<TSource, TResult, TSelector>.Enumerator>, IList<TResult>
    where TSelector : struct, IFunctionIn<TSource, TResult>
{
    readonly TSource[] source;
    readonly TSelector selector;

    public SelectArrayInEnumerable(TSource[] source, in TSelector selector)
    {
        this.source = source;
        this.selector = selector;
    }

    public int Count => source.Length;

    public TResult this[int index] => selector.Invoke(in source[index]);
    TResult IList<TResult>.this[int index]
    {
        get => this[index];
        set => throw new NotSupportedException();
    }

    public Enumerator GetEnumerator() => new Enumerator(source, in selector);
    IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    bool ICollection<TResult>.IsReadOnly => true;

    public void CopyTo(TResult[] array, int arrayIndex)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (arrayIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        }

        if (array.Length - arrayIndex < Count)
        {
            throw new ArgumentException("Destination array is not long enough.");
        }

        var span = source.AsSpan();
        SpanHelpers.CopyToIn(span, in selector, array.AsSpan(), arrayIndex);
    }

    public PooledBuffer<TResult> ToArrayPooled(ArrayPool<TResult>? pool = null) => SpanHelpers.ToArrayPooledIn(source.AsSpan(), in selector, pool);

    public bool Contains(TResult item) => SpanHelpers.ContainsIn(source.AsSpan(), in selector, item);

    public int IndexOf(TResult item) => SpanHelpers.IndexOfIn(source.AsSpan(), in selector, item);

    void ICollection<TResult>.Add(TResult item) => throw new NotSupportedException();
    void ICollection<TResult>.Clear() => throw new NotSupportedException();
    bool ICollection<TResult>.Remove(TResult item) => throw new NotSupportedException();
    void IList<TResult>.Insert(int index, TResult item) => throw new NotSupportedException();
    void IList<TResult>.RemoveAt(int index) => throw new NotSupportedException();

    public struct Enumerator : IEnumerator<TResult>
    {
        readonly TSource[] source;
        readonly TSelector selector;
        int index;

        public Enumerator(TSource[] source, in TSelector selector)
        {
            this.source = source;
            this.selector = selector;
            this.index = -1;
        }

        public TResult Current => selector.Invoke(in source[index]);
        object? IEnumerator.Current => Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            index++;
            return index < source.Length;
        }

        public void Reset() => index = -1;

        public void Dispose() { }
    }
}
