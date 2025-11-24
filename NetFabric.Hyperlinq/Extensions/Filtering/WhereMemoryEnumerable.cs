using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// WhereEnumerable for Memory-based sources (arrays, Memory, List via CollectionsMarshal)
    /// </summary>
    public readonly struct WhereMemoryEnumerable<TSource> : IValueEnumerable<TSource, WhereMemoryEnumerable<TSource>.Enumerator>
    {
        readonly ReadOnlyMemory<TSource> source;
        readonly Func<TSource, bool> predicate;

        public WhereMemoryEnumerable(ReadOnlyMemory<TSource> source, Func<TSource, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereSelectMemoryEnumerable<TSource, TResult> Select<TResult>(Func<TSource, TResult> selector)
            => new WhereSelectMemoryEnumerable<TSource, TResult>(source, predicate, selector);

        public Enumerator GetEnumerator() => new Enumerator(source, predicate);
        IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<TSource>
        {
            readonly ReadOnlyMemory<TSource> memory;
            readonly Func<TSource, bool> predicate;
            int index;

            public Enumerator(ReadOnlyMemory<TSource> memory, Func<TSource, bool> predicate)
            {
                this.memory = memory;
                this.predicate = predicate;
                this.index = -1;
            }

            public TSource Current => memory.Span[index];
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
