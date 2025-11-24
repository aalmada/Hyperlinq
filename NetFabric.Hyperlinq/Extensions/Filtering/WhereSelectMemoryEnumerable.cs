using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// WhereSelectEnumerable for Memory-based sources (fused Where+Select)
    /// </summary>
    public readonly struct WhereSelectMemoryEnumerable<TSource, TResult> : IValueEnumerable<TResult, WhereSelectMemoryEnumerable<TSource, TResult>.Enumerator>
    {
        readonly ReadOnlyMemory<TSource> source;
        readonly Func<TSource, bool> predicate;
        readonly Func<TSource, TResult> selector;

        public WhereSelectMemoryEnumerable(ReadOnlyMemory<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
        {
            this.source = source;
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        public Enumerator GetEnumerator() => new Enumerator(source, predicate, selector);
        IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<TResult>
        {
            readonly ReadOnlyMemory<TSource> memory;
            readonly Func<TSource, bool> predicate;
            readonly Func<TSource, TResult> selector;
            int index;

            public Enumerator(ReadOnlyMemory<TSource> memory, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
            {
                this.memory = memory;
                this.predicate = predicate;
                this.selector = selector;
                this.index = -1;
            }

            public TResult Current => selector(memory.Span[index]);
            object? IEnumerator.Current => Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                var span = memory.Span;
                while (++index < span.Length)
                {
                    if (predicate(span[index]))
                        return true;
                }
                return false;
            }

            public void Reset() => index = -1;
            public void Dispose() { }
        }
    }
}
