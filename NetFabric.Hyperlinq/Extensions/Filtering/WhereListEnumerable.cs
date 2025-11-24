using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// WhereEnumerable for List sources (uses CollectionsMarshal for zero-copy)
    /// </summary>
    public readonly struct WhereListEnumerable<TSource> : IValueEnumerable<TSource, WhereListEnumerable<TSource>.Enumerator>
    {
        readonly List<TSource> source;
        readonly Func<TSource, bool> predicate;

        public WhereListEnumerable(List<TSource> source, Func<TSource, bool> predicate)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereSelectListEnumerable<TSource, TResult> Select<TResult>(Func<TSource, TResult> selector)
            => new WhereSelectListEnumerable<TSource, TResult>(source, predicate, selector);

        public Enumerator GetEnumerator() => new Enumerator(source, predicate);
        IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<TSource>
        {
            readonly List<TSource> list;
            readonly Func<TSource, bool> predicate;
            readonly int length;
            int index;

            public Enumerator(List<TSource> list, Func<TSource, bool> predicate)
            {
                this.list = list;
                this.predicate = predicate;
                this.length = CollectionsMarshal.AsSpan(list).Length;
                this.index = -1;
            }

            public TSource Current => CollectionsMarshal.AsSpan(list)[index];
            object? IEnumerator.Current => Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                var span = CollectionsMarshal.AsSpan(list);
                while (++index < length)
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
