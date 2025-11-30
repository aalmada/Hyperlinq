using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Ref struct enumerable for Select operation on arrays.
    /// Provides maximum performance for foreach-only scenarios.
    /// Use AsValueEnumerable() before Select() if you need to chain operations.
    /// </summary>
    public readonly ref struct SelectArrayRefStructEnumerable<TSource, TResult>
    {
        readonly TSource[] source;
        readonly Func<TSource, TResult> selector;

        internal TSource[] Source => source;
        internal Func<TSource, TResult> Selector => selector;

        public SelectArrayRefStructEnumerable(TSource[] source, Func<TSource, TResult> selector)
        {
            this.source = source;
            this.selector = selector;
        }



        public Enumerator GetEnumerator() => new Enumerator(source, selector);

        public ref struct Enumerator
        {
            readonly TSource[] source;
            readonly Func<TSource, TResult> selector;
            int index;

            public Enumerator(TSource[] source, Func<TSource, TResult> selector)
            {
                this.source = source;
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
                index++;
                return index < source.Length;
            }
        }
    }
}
