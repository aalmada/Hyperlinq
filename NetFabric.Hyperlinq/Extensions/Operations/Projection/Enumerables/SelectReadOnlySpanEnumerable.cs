using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public readonly ref struct SelectReadOnlySpanEnumerable<TSource, TResult, TSelector>
    where TSelector : struct, IFunction<TSource, TResult>
{
    readonly ReadOnlySpan<TSource> source;
    readonly TSelector selector;

    public SelectReadOnlySpanEnumerable(ReadOnlySpan<TSource> source, TSelector selector)
    {
        this.source = source;
        this.selector = selector;
    }

    internal ReadOnlySpan<TSource> Source => source;
    internal TSelector Selector => selector;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Count() => source.Length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Any() => source.Length > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult First()
    {
        if (source.Length == 0)
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }

        return selector.Invoke(source[0]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<TResult> FirstOrNone()
        => source.Length == 0 ? Option<TResult>.None() : Option<TResult>.Some(selector.Invoke(source[0]));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Single()
    {
        if (source.Length == 0)
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }

        if (source.Length > 1)
        {
            throw new InvalidOperationException("Sequence contains more than one element");
        }

        return selector.Invoke(source[0]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<TResult> SingleOrNone()
    {
        if (source.Length == 0)
        {
            return Option<TResult>.None();
        }

        if (source.Length > 1)
        {
            throw new InvalidOperationException("Sequence contains more than one element");
        }

        return Option<TResult>.Some(selector.Invoke(source[0]));
    }

    public TResult[] ToArray() => SpanHelpers.ToArray<TSource, TResult, TSelector>(source, selector);
    public List<TResult> ToList()
    {
        var count = source.Length;
        var list = new List<TResult>(count);
        CollectionsMarshal.SetCount(list, count);
        var destination = CollectionsMarshal.AsSpan(list);
        for (var i = 0; (uint)i < (uint)source.Length; i++)
        {
            destination[i] = selector.Invoke(source[i]);
        }
        return list;
    }

    public Enumerator GetEnumerator() => new Enumerator(source, selector);

    public ref struct Enumerator
    {
        readonly ReadOnlySpan<TSource> source;
        readonly TSelector selector;
        int index;

        public Enumerator(ReadOnlySpan<TSource> source, TSelector selector)
        {
            this.source = source;
            this.selector = selector;
            this.index = -1;
        }

        public readonly TResult Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => selector.Invoke(source[index]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return (uint)++index < (uint)source.Length;
        }
    }
}
