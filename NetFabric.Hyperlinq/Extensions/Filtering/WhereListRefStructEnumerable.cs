using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    /// <summary>
    /// Ref struct enumerable for Where operation on List&lt;T&gt;.
    /// Provides maximum performance for foreach-only scenarios using span-based iteration.
    /// Use AsValueEnumerable() before Where() if you need to chain operations.
    /// </summary>
    public readonly ref struct WhereListRefStructEnumerable<TSource>
    {
        readonly List<TSource> source;
        readonly Func<TSource, bool> predicate;

        public WhereListRefStructEnumerable(List<TSource> source, Func<TSource, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }

        public int Count()
        {
            var count = 0;
            var span = CollectionsMarshal.AsSpan(source);
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    count++;
            }
            return count;
        }

        public bool Any()
        {
            var span = CollectionsMarshal.AsSpan(source);
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    return true;
            }
            return false;
        }

        public TSource First()
        {
            var span = CollectionsMarshal.AsSpan(source);
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    return span[i];
            }
            throw new InvalidOperationException("Sequence contains no elements");
        }

        public Option<TSource> FirstOrNone()
        {
            var span = CollectionsMarshal.AsSpan(source);
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    return Option<TSource>.Some(span[i]);
            }
            return Option<TSource>.None();
        }

        public TSource Single()
        {
            var found = false;
            var result = default(TSource);
            var span = CollectionsMarshal.AsSpan(source);
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                {
                    if (found)
                        throw new InvalidOperationException("Sequence contains more than one element");
                    result = span[i];
                    found = true;
                }
            }
            if (!found)
                throw new InvalidOperationException("Sequence contains no elements");
            return result!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> SingleOrNone()
        {
            var span = CollectionsMarshal.AsSpan(source);
            var found = false;
            TSource result = default!;
            foreach (var item in span)
            {
                if (predicate(item))
                {
                    if (found)
                        throw new InvalidOperationException("Sequence contains more than one matching element");
                    found = true;
                    result = item;
                }
            }
            return found ? Option<TSource>.Some(result) : Option<TSource>.None();
        }

        public TSource Sum()
        {
            if (typeof(TSource) == typeof(int))
            {
                var sum = 0;
                var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(source);
                for (var i = 0; i < span.Length; i++)
                {
                    if (predicate(span[i]))
                        sum += (int)(object)span[i]!;
                }
                return (TSource)(object)sum;
            }
            throw new NotSupportedException($"Sum is not supported for type {typeof(TSource)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource Last()
        {
            var span = CollectionsMarshal.AsSpan(source);
            for (var index = span.Length - 1; index >= 0; index--)
            {
                if (predicate(span[index]))
                    return span[index];
            }
            throw new InvalidOperationException("Sequence contains no matching element");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> LastOrNone()
        {
            var span = CollectionsMarshal.AsSpan(source);
            for (var index = span.Length - 1; index >= 0; index--)
            {
                if (predicate(span[index]))
                    return Option<TSource>.Some(span[index]);
            }
            return Option<TSource>.None();
        }

        public TSource[] ToArray()
        {
            var list = new List<TSource>();
            var span = CollectionsMarshal.AsSpan(source);
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    list.Add(span[i]);
            }
            return list.ToArray();
        }

        public List<TSource> ToList()
        {
            var list = new List<TSource>();
            var span = CollectionsMarshal.AsSpan(source);
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    list.Add(span[i]);
            }
            return list;
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
                        return false;
                    if (predicate(span[index]))
                        return true;
                }
            }
        }
    }
}
