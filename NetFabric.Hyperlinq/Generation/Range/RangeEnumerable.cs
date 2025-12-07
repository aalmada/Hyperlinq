using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public readonly partial struct RangeEnumerable
        : IValueReadOnlyList<int, RangeEnumerable.Enumerator>, IList<int>
    {
        private readonly int start;
        private readonly int count;

        internal RangeEnumerable(int start, int count)
        {
            this.start = start;
            this.count = count;
        }

        public int Count => count;

        public int this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => start + index;
        }

        int IList<int>.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(in this);

        IEnumerator<int> IEnumerable<int>.GetEnumerator() => new Enumerator(in this);
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(in this);

        public struct Enumerator
            : IEnumerator<int>
        {
            private int current;
            private readonly int end;

            internal Enumerator(in RangeEnumerable enumerable)
            {
                current = enumerable.start - 1;
                end = enumerable.start + enumerable.count;
            }

            public int Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => current;
            }

            object IEnumerator.Current => current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                current++;
                return current < end;
            }

            public void Reset() => throw new NotSupportedException();

            public void Dispose() { }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any() => count > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(int value)
            => value >= start && value < start + count;

        public int IndexOf(int item)
        {
            if (item >= start && item < start + count)
                return item - start;
            return -1;
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < count)
                throw new ArgumentException("Destination array is not long enough.");

            var current = start;
            for (var i = 0; i < count; i++)
            {
                array[arrayIndex + i] = current++;
            }
        }

        bool ICollection<int>.IsReadOnly => true;
        void ICollection<int>.Add(int item) => throw new NotSupportedException();
        void ICollection<int>.Clear() => throw new NotSupportedException();
        bool ICollection<int>.Remove(int item) => throw new NotSupportedException();
        void IList<int>.Insert(int index, int item) => throw new NotSupportedException();
        void IList<int>.RemoveAt(int index) => throw new NotSupportedException();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int[] ToArray()
        {
            if (count == 0)
                return Array.Empty<int>();

            var result = new int[count];
            if (count > 0)
            {
                // Optimized fill
                var span = result.AsSpan();
                var current = start;
                for (var i = 0; i < span.Length; i++)
                {
                    span[i] = current++;
                }
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<int> ToList()
        {
            var result = new List<int>(count);
            if (count > 0)
            {
                var current = start;
                for (var i = 0; i < count; i++)
                {
                    result.Add(current++);
                }
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PooledBuffer<int> ToArrayPooled(ArrayPool<int>? pool = null)
        {
            pool ??= ArrayPool<int>.Shared;
            var result = pool.Rent(count);
            if (count > 0)
            {
                var span = result.AsSpan(0, count);
                var current = start;
                for (var i = 0; i < span.Length; i++)
                {
                    span[i] = current++;
                }
            }
            return new PooledBuffer<int>(result, count, pool);
        }


    }
}
