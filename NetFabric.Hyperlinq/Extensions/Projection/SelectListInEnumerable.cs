using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public readonly struct SelectListInEnumerable<TSource, TResult, TSelector> : IValueReadOnlyList<TResult, SelectListInEnumerable<TSource, TResult, TSelector>.Enumerator>, IList<TResult>
        where TSelector : struct, IFunctionIn<TSource, TResult>
    {
        readonly List<TSource> source;
        readonly TSelector selector;

        public SelectListInEnumerable(List<TSource> source, TSelector selector)
        {
            this.source = source;
            this.selector = selector;
        }

        internal List<TSource> Source => source;
        internal TSelector Selector => selector;

        public int Count => source.Count;

        public TResult this[int index]
        {
            get
            {
                var span = CollectionsMarshal.AsSpan(source);
                if ((uint)index >= (uint)span.Length)
                    ThrowIndexOutOfRangeException();
                    
                return selector.Invoke(in span[index]);
            }
        }
        
        static void ThrowIndexOutOfRangeException() => throw new IndexOutOfRangeException();

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

            var span = CollectionsMarshal.AsSpan(source);
            for (var i = 0; i < span.Length; i++)
            {
                array[arrayIndex + i] = selector.Invoke(in span[i]);
            }
        }

        public bool Contains(TResult item)
        {
            var span = CollectionsMarshal.AsSpan(source);
            for (var i = 0; i < span.Length; i++)
            {
                if (EqualityComparer<TResult>.Default.Equals(selector.Invoke(in span[i]), item))
                    return true;
            }
            return false;
        }

        public int IndexOf(TResult item)
        {
            var span = CollectionsMarshal.AsSpan(source);
            for (var i = 0; i < span.Length; i++)
            {
                if (EqualityComparer<TResult>.Default.Equals(selector.Invoke(in span[i]), item))
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
            readonly List<TSource> source;
            readonly TSelector selector;
            int index;

            public Enumerator(List<TSource> source, TSelector selector)
            {
                this.source = source;
                this.selector = selector;
                this.index = -1;
            }

            public TResult Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => selector.Invoke(in CollectionsMarshal.AsSpan(source)[index]);
            }
            
            object? IEnumerator.Current => Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                index++;
                return index < source.Count;
            }

            public void Reset() => index = -1;

            public void Dispose() { }
        }
    }
}
