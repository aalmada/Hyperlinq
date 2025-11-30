using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public readonly ref struct WhereReadOnlySpanEnumerable<TSource>
    {
        readonly ReadOnlySpan<TSource> source;
        readonly Func<TSource, bool> predicate;

        public WhereReadOnlySpanEnumerable(ReadOnlySpan<TSource> source, Func<TSource, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereSelectReadOnlySpanEnumerable<TSource, TResult> Select<TResult>(Func<TSource, TResult> selector)
            => new WhereSelectReadOnlySpanEnumerable<TSource, TResult>(source, predicate, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count() => source.Count(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any() => source.Any(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource First() => source.First(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> FirstOrNone() => source.FirstOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource Single() => source.Single(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> SingleOrNone() => source.SingleOrNone(predicate);

        public TSource[] ToArray()
            => source.ToArray(predicate);

        public List<TSource> ToList()
            => source.ToList(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource Last()
            => source.Last(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> LastOrNone()
            => source.LastOrNone(predicate);

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
