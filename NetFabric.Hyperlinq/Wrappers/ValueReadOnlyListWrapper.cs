using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Generic value-type list wrapper for any IList&lt;T&gt; providing a value-type enumerator, Count property, and indexer.
    /// Implements IValueReadOnlyList and IList (read-only).
    /// </summary>
    public readonly struct ValueReadOnlyListWrapper<TList, TEnumerator, TSource> 
        : IValueReadOnlyList<TSource, TEnumerator>, IList<TSource>
        where TList : IList<TSource>
        where TEnumerator : struct, IEnumerator<TSource>
    {
        private readonly TList source;

        public ValueReadOnlyListWrapper(TList source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        internal TList Source => source;

        public int Count => source.Count;
        public TSource this[int index] => source[index];
        
        TSource IList<TSource>.this[int index]
        {
            get => source[index];
            set => throw new NotSupportedException();
        }

        public TEnumerator GetEnumerator() => (TEnumerator)source.GetEnumerator();
        IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();

        bool ICollection<TSource>.IsReadOnly => true;

        public void CopyTo(TSource[] array, int arrayIndex)
        {
            source.CopyTo(array, arrayIndex);
        }

        public bool Contains(TSource item)
        {
            return source.Contains(item);
        }

        public int IndexOf(TSource item)
        {
            return source.IndexOf(item);
        }

        void ICollection<TSource>.Add(TSource item) => throw new NotSupportedException();
        void ICollection<TSource>.Clear() => throw new NotSupportedException();
        bool ICollection<TSource>.Remove(TSource item) => throw new NotSupportedException();
        void IList<TSource>.Insert(int index, TSource item) => throw new NotSupportedException();
        void IList<TSource>.RemoveAt(int index) => throw new NotSupportedException();
    }
}
