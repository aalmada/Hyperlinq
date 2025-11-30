using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public readonly ref struct WhereSelectReadOnlySpanEnumerable<TSource, TResult>
    {
        readonly ReadOnlySpan<TSource> source;
        readonly Func<TSource, bool> predicate;
        readonly Func<TSource, TResult> selector;

        public WhereSelectReadOnlySpanEnumerable(ReadOnlySpan<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
        {
            this.source = source;
            this.predicate = predicate;
            this.selector = selector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count()
        {
            var count = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                    count++;
            }
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any()
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult First()
            => FirstOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult FirstOrDefault()
            => FirstOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult FirstOrDefault(TResult defaultValue)
            => FirstOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TResult> FirstOrNone()
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    return Option<TResult>.Some(selector(item));
            }
            return Option<TResult>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult Single()
            => SingleOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult SingleOrDefault()
            => SingleOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult SingleOrDefault(TResult defaultValue)
            => SingleOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TResult> SingleOrNone()
        {
            var found = false;
            var result = default(TResult);
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    if (found)
                        throw new InvalidOperationException("Sequence contains more than one matching element");
                    
                    found = true;
                    result = selector(item);
                }
            }
            return found ? Option<TResult>.Some(result!) : Option<TResult>.None();
        }

        public TResult Sum()
        {
            if (typeof(TResult) == typeof(int))
            {
                var sum = 0;
                foreach (var item in source)
                {
                    if (predicate(item))
                        sum += (int)(object)selector(item)!;
                }
                return (TResult)(object)sum;
            }
            throw new NotSupportedException($"Sum is not supported for type {typeof(TResult)}");
        }

        public TResult[] ToArray()
        {
            var list = new List<TResult>();
            foreach (var item in source)
            {
                if (predicate(item))
                    list.Add(selector(item));
            }
            return list.ToArray();
        }

        public List<TResult> ToList()
        {
            var list = new List<TResult>();
            foreach (var item in source)
            {
                if (predicate(item))
                    list.Add(selector(item));
            }
            return list;
        }

        public Enumerator GetEnumerator() => new Enumerator(source, predicate, selector);

        public ref struct Enumerator
        {
            readonly ReadOnlySpan<TSource> source;
            readonly Func<TSource, bool> predicate;
            readonly Func<TSource, TResult> selector;
            int index;

            public Enumerator(ReadOnlySpan<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
            {
                this.source = source;
                this.predicate = predicate;
                this.selector = selector;
                this.index = -1;
            }

            public readonly TResult Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => selector(source[index]);
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
