using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

/// <summary>
/// Ref struct enumerable for Select operation on List&lt;T&gt;.
/// Provides maximum performance for foreach-only scenarios using span-based iteration.
/// Use AsValueEnumerable() before Select() if you need to chain operations.
/// </summary>
public readonly ref struct SelectListRefStructEnumerable<TSource, TResult>
{
    readonly List<TSource> source;
    readonly Func<TSource, TResult> selector;

    internal List<TSource> Source => source;
    internal Func<TSource, TResult> Selector => selector;

    public SelectListRefStructEnumerable(List<TSource> source, Func<TSource, TResult> selector)
    {
        this.source = source;
        this.selector = selector;
    }



    public Enumerator GetEnumerator() => new Enumerator(source, selector);

    public ref struct Enumerator
    {
        readonly Span<TSource> span;
        readonly Func<TSource, TResult> selector;
        int index;

        public Enumerator(List<TSource> source, Func<TSource, TResult> selector)
        {
            this.span = CollectionsMarshal.AsSpan(source);
            this.selector = selector;
            this.index = -1;
        }

        public readonly TResult Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => selector(span[index]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            index++;
            return index < span.Length;
        }
    }
}
