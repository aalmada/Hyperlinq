using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Ref struct enumerable for Select operation on List&lt;T&gt;.
    /// Provides maximum performance for foreach-only scenarios using span-based iteration.
    /// Use AsValueEnumerable() before Select() if you need to chain operations.
    /// </summary>
    public readonly ref struct SelectListRefStructEnumerable<TSource, TResult>
    {
        readonly List<TSource> source;
        readonly Func<TSource, TResult> selector;

        public SelectListRefStructEnumerable(List<TSource> source, Func<TSource, TResult> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count() => source.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any() => source.Count > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult First()
        {
            if (source.Count == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            return selector(source[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TResult> FirstOrNone()
            => source.Count == 0 ? Option<TResult>.None() : Option<TResult>.Some(selector(source[0]));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult Single()
        {
            if (source.Count == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            if (source.Count > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return selector(source[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TResult> SingleOrNone()
        {
            if (source.Count == 0)
                return Option<TResult>.None();
            if (source.Count > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<TResult>.Some(selector(source[0]));
        }

        public TResult[] ToArray()
        {
            var array = new TResult[source.Count];
            var span = CollectionsMarshal.AsSpan(source);
            for (var i = 0; i < span.Length; i++)
                array[i] = selector(span[i]);
            return array;
        }

        public List<TResult> ToList()
        {
            var list = new List<TResult>(source.Count);
            var span = CollectionsMarshal.AsSpan(source);
            for (var i = 0; i < span.Length; i++)
                list.Add(selector(span[i]));
            return list;
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
}
