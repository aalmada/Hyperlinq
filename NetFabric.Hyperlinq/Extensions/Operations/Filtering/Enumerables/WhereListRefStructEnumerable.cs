using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

/// <summary>
/// Ref struct enumerable for Where operation on List&lt;T&gt;.
/// Provides maximum performance for foreach-only scenarios using span-based iteration.
/// Use AsValueEnumerable() before Where() if you need to chain operations.
/// </summary>
public readonly ref struct WhereListRefStructEnumerable<TSource>
{
    readonly List<TSource> source;
    readonly Func<TSource, bool> predicate;

    internal List<TSource> Source => source;
    internal Func<TSource, bool> Predicate => predicate;

    public WhereListRefStructEnumerable(List<TSource> source, Func<TSource, bool> predicate)
    {
        this.source = source;
        this.predicate = predicate;
    }



    public Enumerator GetEnumerator() => new Enumerator(source, predicate);

    public ref struct Enumerator
    {
        readonly Span<TSource> span;
        readonly Func<TSource, bool> predicate;
        int index;

        public Enumerator(List<TSource> source, Func<TSource, bool> predicate)
        {
            this.span = CollectionsMarshal.AsSpan(source);
            this.predicate = predicate;
            this.index = -1;
        }

        public readonly TSource Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => span[index];
        }

        public bool MoveNext()
        {
            while (true)
            {
                index++;
                if (index >= span.Length)
                {
                    return false;
                }

                if (predicate(span[index]))
                {
                    return true;
                }
            }
        }
    }
}
