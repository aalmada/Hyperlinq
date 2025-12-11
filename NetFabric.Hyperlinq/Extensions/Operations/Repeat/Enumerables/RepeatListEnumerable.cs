using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public readonly partial struct RepeatListEnumerable<TSource>
        : IValueReadOnlyCollection<TSource, RepeatListEnumerable<TSource>.Enumerator>
        , ICollection<TSource>
        , IList<TSource>
    {
        private readonly List<TSource> source;
        private readonly int count;

        internal RepeatListEnumerable(List<TSource> source, int count)
        {
            this.source = source;
            this.count = count;
        }

        public readonly int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (count == 0) return 0;
                var sourceCount = source.Count;
                if (sourceCount == 0) return 0;
                try
                {
                    return checked(sourceCount * count);
                }
                catch (OverflowException)
                {
                    throw new OverflowException("The number of elements in the repeated sequence exceeds Int32.MaxValue.");
                }
            }
        }

        public readonly TSource this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return source[index % source.Count];
            }
        }

        TSource IList<TSource>.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Enumerator GetEnumerator() => new Enumerator(in this);

        IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => new Enumerator(in this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(in this);

        bool ICollection<TSource>.IsReadOnly => true;
        void ICollection<TSource>.Add(TSource item) => throw new NotSupportedException();
        void ICollection<TSource>.Clear() => throw new NotSupportedException();
        bool ICollection<TSource>.Contains(TSource item)
        {
            if (Count == 0) return false;
            return source.Contains(item);
        }
        void ICollection<TSource>.CopyTo(TSource[] array, int arrayIndex)
        {
             if (array is null) throw new ArgumentNullException(nameof(array));
             if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
             if (array.Length - arrayIndex < Count) throw new ArgumentException("Destination array is not long enough.");

             var sourceSpan = CollectionsMarshal.AsSpan(source);
             var destSpan = array.AsSpan(arrayIndex, Count);
             for(int i = 0; i < count; i++)
             {
                 sourceSpan.CopyTo(destSpan.Slice(i * source.Count, source.Count));
             }
        }
        bool ICollection<TSource>.Remove(TSource item) => throw new NotSupportedException();
        int IList<TSource>.IndexOf(TSource item)
        {
            if (count == 0) return -1;
            var index = source.IndexOf(item);
            if (index >= 0) return index;
            return -1;
        }
        void IList<TSource>.Insert(int index, TSource item) => throw new NotSupportedException();
        void IList<TSource>.RemoveAt(int index) => throw new NotSupportedException();

        public struct Enumerator
            : IEnumerator<TSource>
        {
            private readonly List<TSource> source;
            private int remaining;
            private int index;

            internal Enumerator(in RepeatListEnumerable<TSource> enumerable)
            {
                source = enumerable.source;
                remaining = enumerable.Count;
                index = -1;
            }

            public readonly TSource Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => source[index % source.Count];
            }

            readonly object? IEnumerator.Current => Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                if (remaining == 0)
                    return false;

                remaining--;
                index++;
                return true;
            }

            public void Reset() => throw new NotSupportedException();

            public void Dispose() {}
        }
    }
}
