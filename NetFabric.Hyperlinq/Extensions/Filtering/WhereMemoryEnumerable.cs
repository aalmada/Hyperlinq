using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// WhereEnumerable for Memory-based sources (arrays, Memory, List via CollectionsMarshal)
    /// Optimized with 4-way loop unrolling for instruction-level parallelism.
    /// </summary>
    public readonly struct WhereMemoryEnumerable<TSource> : IValueEnumerable<TSource, WhereMemoryEnumerable<TSource>.Enumerator>
    {
        readonly ReadOnlyMemory<TSource> source;
        readonly Func<TSource, bool> predicate;

        public WhereMemoryEnumerable(ReadOnlyMemory<TSource> source, Func<TSource, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        internal ReadOnlyMemory<TSource> Source => source;
        internal Func<TSource, bool> Predicate => predicate;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereSelectMemoryEnumerable<TSource, TResult> Select<TResult>(Func<TSource, TResult> selector)
            => new WhereSelectMemoryEnumerable<TSource, TResult>(source, predicate, selector);

        public Enumerator GetEnumerator() => new Enumerator(source, predicate);
        IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<TSource>
        {
            readonly ReadOnlyMemory<TSource> memory;
            readonly Func<TSource, bool> predicate;
            int index;

            public Enumerator(ReadOnlyMemory<TSource> memory, Func<TSource, bool> predicate)
            {
                this.memory = memory;
                this.predicate = predicate;
                this.index = -1;
            }

            public TSource Current => memory.Span[index];
            object? IEnumerator.Current => Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                var span = memory.Span;
                ref var spanRef = ref MemoryMarshal.GetReference(span);
                var length = span.Length;
                
                // Process 4 items at a time for instruction-level parallelism
                var end = length - 3;
                for (; index < end; index += 4)
                {
                    // Check 4 items in sequence
                    var i0 = index + 1;
                    if (predicate(Unsafe.Add(ref spanRef, i0)))
                    {
                        index = i0;
                        return true;
                    }
                    
                    var i1 = index + 2;
                    if (predicate(Unsafe.Add(ref spanRef, i1)))
                    {
                        index = i1;
                        return true;
                    }
                    
                    var i2 = index + 3;
                    if (predicate(Unsafe.Add(ref spanRef, i2)))
                    {
                        index = i2;
                        return true;
                    }
                    
                    var i3 = index + 4;
                    if (predicate(Unsafe.Add(ref spanRef, i3)))
                    {
                        index = i3;
                        return true;
                    }
                }
                
                // Handle remaining items (0-3) with switch to minimize branching
                switch (length - index - 1)
                {
                    case 3:
                        if (predicate(Unsafe.Add(ref spanRef, ++index)))
                            return true;
                        goto case 2;
                    case 2:
                        if (predicate(Unsafe.Add(ref spanRef, ++index)))
                            return true;
                        goto case 1;
                    case 1:
                        if (predicate(Unsafe.Add(ref spanRef, ++index)))
                            return true;
                        break;
                }
                
                return false;
            }

            public void Reset() => index = -1;
            public void Dispose() { }
        }
    }
}
