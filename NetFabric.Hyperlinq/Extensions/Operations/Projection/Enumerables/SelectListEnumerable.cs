using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public readonly struct SelectListEnumerable<TSource, TResult, TSelector> : IValueReadOnlyList<TResult, SelectListEnumerable<TSource, TResult, TSelector>.Enumerator>, IList<TResult>
    where TSelector : struct, IFunction<TSource, TResult>
{
    readonly List<TSource> source;
    readonly TSelector selector;

    public SelectListEnumerable(List<TSource> source, TSelector selector)
    {
        this.source = source;
        this.selector = selector;
    }

    internal List<TSource> Source => source;
    internal TSelector Selector => selector;

    public int Count => source.Count;

    public TResult this[int index] => selector.Invoke(source[index]);
    TResult IList<TResult>.this[int index]
    {
        get => this[index];
        set => throw new NotSupportedException();
    }

    public Enumerator GetEnumerator() => new Enumerator(source, selector);
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

        var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source);
        SpanHelpers.CopyTo(span, selector, array.AsSpan(), arrayIndex);
    }

    public bool Contains(TResult item)
    {
        var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source);
        return SpanHelpers.Contains(span, selector, item);
    }

    public int IndexOf(TResult item)
    {
        var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source);
        return SpanHelpers.IndexOf(span, selector, item);
    }

    void ICollection<TResult>.Add(TResult item) => throw new NotSupportedException();
    void ICollection<TResult>.Clear() => throw new NotSupportedException();
    bool ICollection<TResult>.Remove(TResult item) => throw new NotSupportedException();
    void IList<TResult>.Insert(int index, TResult item) => throw new NotSupportedException();
    void IList<TResult>.RemoveAt(int index) => throw new NotSupportedException();

    public struct Enumerator : IEnumerator<TResult>
    {
        readonly List<TSource> source;
        readonly TSelector selector;
        int index;

        public Enumerator(List<TSource> source, TSelector selector)
        {
            this.source = source;
            this.selector = selector;
            this.index = -1;
        }

        public TResult Current => selector.Invoke(source[index]);
        object? IEnumerator.Current => Current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            index++;
            return index < source.Count;
        }

        public void Reset() => index = -1;

        public void Dispose() { }
    }
}
