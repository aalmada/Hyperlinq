using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// WhereSelectEnumerable for List sources (fused Where+Select)
    /// Optimized with 4-way loop unrolling for instruction-level parallelism.
    /// </summary>
    public readonly struct WhereSelectListEnumerable<TSource, TResult> : IValueEnumerable<TResult, WhereSelectListEnumerable<TSource, TResult>.Enumerator>
    {
        readonly List<TSource> source;
        readonly Func<TSource, bool> predicate;
        readonly Func<TSource, TResult> selector;

        public WhereSelectListEnumerable(List<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        public Enumerator GetEnumerator() => new Enumerator(source, predicate, selector);
        IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<TResult>
        {
            readonly List<TSource> list;
            readonly Func<TSource, bool> predicate;
            readonly Func<TSource, TResult> selector;
            readonly int length;
            int index;

            public Enumerator(List<TSource> list, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
            {
                this.list = list;
                this.predicate = predicate;
                this.selector = selector;
                this.length = CollectionsMarshal.AsSpan(list).Length;
                this.index = -1;
            }

            public TResult Current => selector(CollectionsMarshal.AsSpan(list)[index]);
            object? IEnumerator.Current => Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                var span = CollectionsMarshal.AsSpan(list);
                ref var spanRef = ref MemoryMarshal.GetReference(span);
                
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
