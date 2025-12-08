using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public readonly ref struct SelectReadOnlySpanInEnumerable<TSource, TResult, TSelector>
        where TSelector : struct, IFunctionIn<TSource, TResult>
    {
        readonly ReadOnlySpan<TSource> source;
        readonly TSelector selector;

        public SelectReadOnlySpanInEnumerable(ReadOnlySpan<TSource> source, TSelector selector)
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
                throw new InvalidOperationException("Sequence contains no elements");
            return selector.Invoke(in source[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TResult> FirstOrNone()
            => source.Length == 0 ? Option<TResult>.None() : Option<TResult>.Some(selector.Invoke(in source[0]));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult Single()
        {
            if (source.Length == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            if (source.Length > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return selector.Invoke(in source[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TResult> SingleOrNone()
        {
            if (source.Length == 0)
                return Option<TResult>.None();
            if (source.Length > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<TResult>.Some(selector.Invoke(in source[0]));
        }

        public TResult[] ToArray()
        {
            var array = new TResult[source.Length];
            for (var i = 0; i < source.Length; i++)
                array[i] = selector.Invoke(in source[i]);
            return array;
        }

        public List<TResult> ToList()
        {
            var list = new List<TResult>(source.Length);
            for (var i = 0; i < source.Length; i++)
                list.Add(selector.Invoke(in source[i]));
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
                get => selector.Invoke(in source[index]);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                index++;
                return index < source.Length;
            }
        }
    }
}
