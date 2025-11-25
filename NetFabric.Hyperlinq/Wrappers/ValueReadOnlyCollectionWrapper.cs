using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Generic value-type collection wrapper for any ICollection&lt;T&gt; providing a value-type enumerator and Count property.
    /// Implements IValueReadOnlyCollection and ICollection (read-only).
    /// </summary>
    public readonly struct ValueReadOnlyCollectionWrapper<TCollection, TEnumerator, TSource> 
        : IValueReadOnlyCollection<TSource, TEnumerator>, ICollection<TSource>
        where TCollection : ICollection<TSource>
        where TEnumerator : struct, IEnumerator<TSource>
    {
        private readonly TCollection source;

        public ValueReadOnlyCollectionWrapper(TCollection source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        internal TCollection Source => source;

        public int Count => source.Count;

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

        void ICollection<TSource>.Add(TSource item) => throw new NotSupportedException();
        void ICollection<TSource>.Clear() => throw new NotSupportedException();
        bool ICollection<TSource>.Remove(TSource item) => throw new NotSupportedException();
    }
}
