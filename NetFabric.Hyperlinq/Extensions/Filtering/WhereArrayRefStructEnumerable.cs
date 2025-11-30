using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Ref struct enumerable for Where operation on arrays.
    /// Provides maximum performance for foreach-only scenarios.
    /// Use AsValueEnumerable() before Where() if you need to chain operations.
    /// </summary>
    public readonly ref struct WhereArrayRefStructEnumerable<TSource>
    {
        readonly TSource[] source;
        readonly Func<TSource, bool> predicate;

        internal TSource[] Source => source;
        internal Func<TSource, bool> Predicate => predicate;

        public WhereArrayRefStructEnumerable(TSource[] source, Func<TSource, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }



        public Enumerator GetEnumerator() => new Enumerator(source, predicate);

        public ref struct Enumerator
        {
            readonly TSource[] source;
            readonly Func<TSource, bool> predicate;
            int index;

            public Enumerator(TSource[] source, Func<TSource, bool> predicate)
            {
                this.source = source;
                this.predicate = predicate;
                this.index = -1;
            }

            public readonly TSource Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => source[index];
            }

            public bool MoveNext()
            {
                while (true)
                {
                    index++;
                    if (index >= source.Length)
                        return false;
                    if (predicate(source[index]))
                        return true;
                }
            }
        }
    }
}
