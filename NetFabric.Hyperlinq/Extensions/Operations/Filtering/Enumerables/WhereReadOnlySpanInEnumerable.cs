using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public readonly ref struct WhereReadOnlySpanInEnumerable<TSource, TPredicate>
    where TPredicate : struct, IFunctionIn<TSource, bool>
{
    readonly ReadOnlySpan<TSource> source;
    readonly TPredicate predicate;

    internal ReadOnlySpan<TSource> Source => source;
    internal TPredicate Predicate => predicate;

    public WhereReadOnlySpanInEnumerable(ReadOnlySpan<TSource> source, in TPredicate predicate)
    {
        this.source = source;
        this.predicate = predicate;
    }

    public Enumerator GetEnumerator() => new Enumerator(source, predicate);

    public ref struct Enumerator
    {
        readonly ReadOnlySpan<TSource> source;
        readonly TPredicate predicate;
        int index;

        public Enumerator(ReadOnlySpan<TSource> source, TPredicate predicate)
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
            while ((uint)++index < (uint)source.Length)
            {
                if (predicate.Invoke(in source[index]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
