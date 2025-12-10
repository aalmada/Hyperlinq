using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public readonly struct SelectArrayInEnumerable<TSource, TResult, TSelector> : IValueReadOnlyList<TResult, SelectArrayInEnumerable<TSource, TResult, TSelector>.Enumerator>, IList<TResult>
        where TSelector : struct, IFunctionIn<TSource, TResult>
    {
        readonly TSource[] source;
        readonly TSelector selector;

        public SelectArrayInEnumerable(TSource[] source, in TSelector selector)
        {
            this.source = source;
            this.selector = selector;
        }

        public int Count => source.Length;

        public TResult this[int index] => selector.Invoke(in source[index]);
        TResult IList<TResult>.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        public Enumerator GetEnumerator() => new Enumerator(source, in selector);
        IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        bool ICollection<TResult>.IsReadOnly => true;

        public void CopyTo(TResult[] array, int arrayIndex)
        {
            if (array is null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count) throw new ArgumentException("Destination array is not long enough.");

            var span = source.AsSpan();
            
            if (arrayIndex == 0)
            {
                // Fast path: copy to start of array using foreach
                var index = 0;
                foreach (ref readonly var item in span)
                {
                    array[index++] = selector.Invoke(in item);
                }
            }
            else
            {
                // Offset copy: use for loop for JIT optimization
                var destinationLength = array.Length - arrayIndex;
                var length = span.Length < destinationLength ? span.Length : destinationLength;
                for (var i = 0; i < length; i++)
                {
                    array[arrayIndex + i] = selector.Invoke(in span[i]);
                }
            }
        }

        public PooledBuffer<TResult> ToArrayPooled(ArrayPool<TResult>? pool = null)
        {
            pool ??= ArrayPool<TResult>.Shared;
            var result = pool.Rent(source.Length);
            var index = 0;
            foreach (ref readonly var item in source.AsSpan())
            {
                result[index++] = selector.Invoke(in item);
            }
            return new PooledBuffer<TResult>(result, source.Length, pool);
        }

        public bool Contains(TResult item)
        {
            foreach (ref readonly var sourceItem in source.AsSpan())
            {
                if (EqualityComparer<TResult>.Default.Equals(selector.Invoke(in sourceItem), item))
                    return true;
            }
            return false;
        }

        public int IndexOf(TResult item)
        {
            var index = 0;
            foreach (ref readonly var sourceItem in source.AsSpan())
            {
                if (EqualityComparer<TResult>.Default.Equals(selector.Invoke(in sourceItem), item))
                    return index;
                index++;
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
            readonly TSource[] source;
            readonly TSelector selector;
            int index;

            public Enumerator(TSource[] source, in TSelector selector)
            {
                this.source = source;
                this.selector = selector;
                this.index = -1;
            }

            public TResult Current => selector.Invoke(in source[index]);
            object? IEnumerator.Current => Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                index++;
                return index < source.Length;
            }

            public void Reset() => index = -1;

            public void Dispose() { }
        }
    }
}
