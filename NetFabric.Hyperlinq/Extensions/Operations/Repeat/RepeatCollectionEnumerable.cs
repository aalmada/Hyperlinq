using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public readonly partial struct RepeatCollectionEnumerable<TEnumerator, TSource>
        : IValueReadOnlyCollection<TSource, RepeatCollectionEnumerable<TEnumerator, TSource>.Enumerator>
        , ICollection<TSource>
        where TEnumerator : struct, IEnumerator<TSource>
    {
        private readonly IValueReadOnlyCollection<TSource, TEnumerator> source;
        private readonly int count;

        internal RepeatCollectionEnumerable(IValueReadOnlyCollection<TSource, TEnumerator> source, int count)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Enumerator GetEnumerator() => new Enumerator(in this);

        IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => new Enumerator(in this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(in this);

        bool ICollection<TSource>.IsReadOnly => true;
        void ICollection<TSource>.Add(TSource item) => throw new NotSupportedException();
        void ICollection<TSource>.Clear() => throw new NotSupportedException();
        
        bool ICollection<TSource>.Contains(TSource item)
        {
            if (count == 0) return false;
            
            // Check if source contains the item by iterating.
            // Since source implements IValueReadOnlyCollection, and TEnumerator is struct,
            // we can iterate without boxing.
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                 if (EqualityComparer<TSource>.Default.Equals(enumerator.Current, item))
                     return true;
            }
            return false;
        }

        void ICollection<TSource>.CopyTo(TSource[] array, int arrayIndex)
        {
            if (array is null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count) throw new ArgumentException("Destination array is not long enough.");

            var currentArrayIndex = arrayIndex;
            for (var i = 0; i < count; i++)
            {
                using var enumerator = source.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    array[currentArrayIndex++] = enumerator.Current;
                }
            }
        }
        
        bool ICollection<TSource>.Remove(TSource item) => throw new NotSupportedException();

        public struct Enumerator
            : IEnumerator<TSource>
        {
            private readonly IValueEnumerable<TSource, TEnumerator> source;
            private TEnumerator enumerator;
            private int remaining;

            internal Enumerator(in RepeatCollectionEnumerable<TEnumerator, TSource> enumerable)
            {
                source = enumerable.source;
                remaining = enumerable.count;
                enumerator = remaining > 0 ? source.GetEnumerator() : default;
            }

            public readonly TSource Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => enumerator.Current;
            }

            readonly object? IEnumerator.Current => enumerator.Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                if (remaining == 0)
                    return false;

                if (enumerator.MoveNext())
                    return true;

                remaining--;
                enumerator.Dispose();

                if (remaining > 0)
                {
                    enumerator = source.GetEnumerator();
                    return enumerator.MoveNext();
                }

                return false;
            }

            public void Reset() => throw new NotSupportedException();

            public void Dispose() => enumerator.Dispose();
        }
    }
}
