using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Value-type enumerable wrapper for List&lt;T&gt;.
    /// Uses List&lt;T&gt;.Enumerator which is a value type.
    /// </summary>
    public readonly struct ListValueEnumerable<T> : IValueEnumerable<T, List<T>.Enumerator>
    {
        private readonly List<T> source;

        public ListValueEnumerable(List<T> source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public List<T>.Enumerator GetEnumerator() => source.GetEnumerator();
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();
    }
}
