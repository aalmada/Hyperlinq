using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// SelectEnumerable for Memory-based sources (arrays, Memory, List via CollectionsMarshal)
    /// Implements IValueReadOnlyList to preserve indexer and Count capabilities.
    /// </summary>
    public readonly struct SelectMemoryEnumerable<TSource, TResult> : IValueReadOnlyList<TResult, SelectMemoryEnumerable<TSource, TResult>.Enumerator>, IList<TResult>
    {
        readonly ReadOnlyMemory<TSource> source;
        readonly Func<TSource, TResult> selector;

        public SelectMemoryEnumerable(ReadOnlyMemory<TSource> source, Func<TSource, TResult> selector)
        {
            this.source = source;
            this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        public int Count => source.Length;

        public TResult this[int index] => selector(source.Span[index]);
        
        TResult IList<TResult>.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        public Enumerator GetEnumerator() => new Enumerator(source, selector);
        IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        bool ICollection<TResult>.IsReadOnly => true;

        public void CopyTo(TResult[] array, int arrayIndex)
        {
            if (array is null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count) throw new ArgumentException("Destination array is not long enough.");

            var span = source.Span;
            for (var i = 0; i < span.Length; i++)
            {
                array[arrayIndex + i] = selector(span[i]);
            }
        }

        public bool Contains(TResult item)
        {
            var span = source.Span;
            for (var i = 0; i < span.Length; i++)
            {
                if (EqualityComparer<TResult>.Default.Equals(selector(span[i]), item))
                    return true;
            }
            return false;
        }

        public int IndexOf(TResult item)
        {
            var span = source.Span;
            for (var i = 0; i < span.Length; i++)
            {
                if (EqualityComparer<TResult>.Default.Equals(selector(span[i]), item))
                    return i;
            }
            return -1;
        }

        void ICollection<TResult>.Add(TResult item) => throw new NotSupportedException();
        void ICollection<TResult>.Clear() => throw new NotSupportedException();
        bool ICollection<TResult>.Remove(TResult item) => throw new NotSupportedException();
        void IList<TResult>.Insert(int index, TResult item) => throw new NotSupportedException();
        void IList<TResult>.RemoveAt(int index) => throw new NotSupportedException();

        public struct Enumerator : IEnumerator<TResult>
        {
            readonly ReadOnlyMemory<TSource> memory;
            readonly Func<TSource, TResult> selector;
            int index;

            public Enumerator(ReadOnlyMemory<TSource> memory, Func<TSource, TResult> selector)
            {
                this.memory = memory;
                this.selector = selector;
                this.index = -1;
            }

            public TResult Current => selector(memory.Span[index]);
            object? IEnumerator.Current => Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                index++;
                return index < memory.Length;
            }

            public void Reset() => index = -1;

            public void Dispose() { }
        }
    }
}
