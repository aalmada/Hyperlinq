using System;
using System.Collections;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Fused Skip+Take enumerable for optimal pagination performance.
    /// Combines Skip and Take into a single enumerable to avoid double enumeration overhead.
    /// </summary>
    public readonly struct SkipTakeEnumerable<TSource, TEnumerable, TEnumerator> 
        : IValueEnumerable<TSource, SkipTakeEnumerable<TSource, TEnumerable, TEnumerator>.Enumerator>
        where TEnumerable : IValueEnumerable<TSource, TEnumerator>
        where TEnumerator : struct, IEnumerator<TSource>
    {
        readonly TEnumerable source;
        readonly int skipCount;
        readonly int takeCount;

        internal SkipTakeEnumerable(TEnumerable source, int skipCount, int takeCount)
        {
            this.source = source;
            this.skipCount = skipCount;
            this.takeCount = takeCount;
        }

        internal TEnumerable Source => source;
        internal int SkipCount => skipCount;
        internal int TakeCount => takeCount;

        public Enumerator GetEnumerator() => new Enumerator(source.GetEnumerator(), skipCount, takeCount);
        IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<TSource>
        {
            TEnumerator sourceEnumerator;
            readonly int skipCount;
            readonly int takeCount;
            int toSkip;
            int remaining;

            internal Enumerator(TEnumerator sourceEnumerator, int skipCount, int takeCount)
            {
                this.sourceEnumerator = sourceEnumerator;
                this.skipCount = skipCount < 0 ? 0 : skipCount;
                this.takeCount = takeCount < 0 ? 0 : takeCount;
                this.toSkip = this.skipCount;
                this.remaining = this.takeCount;
            }

            public TSource Current => sourceEnumerator.Current;
            object? IEnumerator.Current => Current;

            public bool MoveNext()
            {
                // Skip phase
                while (toSkip > 0)
                {
                    if (!sourceEnumerator.MoveNext())
                        return false;
                    toSkip--;
                }
                
                // Take phase
                if (remaining > 0 && sourceEnumerator.MoveNext())
                {
                    remaining--;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                sourceEnumerator.Reset();
                toSkip = skipCount;
                remaining = takeCount;
            }

            public void Dispose() => sourceEnumerator.Dispose();
        }
    }
}
