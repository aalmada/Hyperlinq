using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
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
                throw new InvalidOperationException("Sequence contains no elements");
            return selector.Invoke(source[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TResult> FirstOrNone()
            => source.Length == 0 ? Option<TResult>.None() : Option<TResult>.Some(selector.Invoke(source[0]));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult Single()
        {
            if (source.Length == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            if (source.Length > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return selector.Invoke(source[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TResult> SingleOrNone()
        {
            if (source.Length == 0)
                return Option<TResult>.None();
            if (source.Length > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<TResult>.Some(selector.Invoke(source[0]));
        }

        public TResult[] ToArray()
        {
            var array = new TResult[source.Length];
            var index = 0;
            foreach (ref readonly var item in source)
            {
                array[index++] = selector.Invoke(item);
            }
            return array;
        }

        public PooledBuffer<TResult> ToArrayPooled(ArrayPool<TResult>? pool = null)
        {
            pool ??= ArrayPool<TResult>.Shared;
            var result = pool.Rent(source.Length);
            var index = 0;
            foreach (ref readonly var item in source)
            {
                result[index++] = selector.Invoke(item);
            }
            return new PooledBuffer<TResult>(result, source.Length, pool);
        }

        public List<TResult> ToList()
        {
            var list = new List<TResult>(source.Length);
            foreach (ref readonly var item in source)
            {
                list.Add(selector.Invoke(item));
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
                index++;
                return index < source.Length;
            }
        }
    }
}
