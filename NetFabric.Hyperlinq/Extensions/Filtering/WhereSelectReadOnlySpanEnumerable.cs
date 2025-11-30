using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public readonly ref struct WhereSelectReadOnlySpanEnumerable<TSource, TResult>
    {
        readonly ReadOnlySpan<TSource> source;
        readonly Func<TSource, bool> predicate;
        readonly Func<TSource, TResult> selector;

        internal ReadOnlySpan<TSource> Source => source;
        internal Func<TSource, bool> Predicate => predicate;
        internal Func<TSource, TResult> Selector => selector;

        public WhereSelectReadOnlySpanEnumerable(ReadOnlySpan<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
        {
            this.source = source;
            this.predicate = predicate;
            this.selector = selector;
        }



        public Enumerator GetEnumerator() => new Enumerator(source, predicate, selector);

        public ref struct Enumerator
        {
            readonly ReadOnlySpan<TSource> source;
            readonly Func<TSource, bool> predicate;
            readonly Func<TSource, TResult> selector;
            int index;

            public Enumerator(ReadOnlySpan<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
            {
                this.source = source;
                this.predicate = predicate;
                this.selector = selector;
                this.index = -1;
            }

            public readonly TResult Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => selector(source[index]);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                while (++index < source.Length)
                {
                    if (predicate(source[index]))
                        return true;
                }
                return false;
            }
        }
    }
}
