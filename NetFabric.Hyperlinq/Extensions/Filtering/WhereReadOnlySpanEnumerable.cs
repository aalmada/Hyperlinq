using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public readonly ref struct WhereReadOnlySpanEnumerable<TSource>
    {
        readonly ReadOnlySpan<TSource> source;
        readonly Func<TSource, bool> predicate;

        internal ReadOnlySpan<TSource> Source => source;
        internal Func<TSource, bool> Predicate => predicate;

        public WhereReadOnlySpanEnumerable(ReadOnlySpan<TSource> source, Func<TSource, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }



        public Enumerator GetEnumerator() => new Enumerator(source, predicate);

        public ref struct Enumerator
        {
            readonly ReadOnlySpan<TSource> source;
            readonly Func<TSource, bool> predicate;
            int index;

            public Enumerator(ReadOnlySpan<TSource> source, Func<TSource, bool> predicate)
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                while (++index < source.Length)
                {
                    if (predicate(source[index]))
                        return true;
                }
                return false;
            }
        }
    }
}
