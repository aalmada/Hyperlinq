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

        public WhereArrayRefStructEnumerable(TSource[] source, Func<TSource, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }

        public int Count()
        {
            var count = 0;
            for (var i = 0; i < source.Length; i++)
            {
                if (predicate(source[i]))
                    count++;
            }
            return count;
        }

        public bool Any()
        {
            for (var i = 0; i < source.Length; i++)
            {
                if (predicate(source[i]))
                    return true;
            }
            return false;
        }

        public TSource First()
        {
            for (var i = 0; i < source.Length; i++)
            {
                if (predicate(source[i]))
                    return source[i];
            }
            throw new InvalidOperationException("Sequence contains no elements");
        }

        public Option<TSource> FirstOrNone()
        {
            for (var i = 0; i < source.Length; i++)
            {
                if (predicate(source[i]))
                    return Option<TSource>.Some(source[i]);
            }
            return Option<TSource>.None();
        }

        public TSource Single()
        {
            var found = false;
            var result = default(TSource);
            for (var i = 0; i < source.Length; i++)
            {
                if (predicate(source[i]))
                {
                    if (found)
                        throw new InvalidOperationException("Sequence contains more than one element");
                    result = source[i];
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
            var found = false;
            TSource result = default!;
            foreach (var item in source)
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
                for (var i = 0; i < source.Length; i++)
                {
                    if (predicate(source[i]))
                        sum += (int)(object)source[i]!;
                }
                return (TSource)(object)sum;
            }
            throw new NotSupportedException($"Sum is not supported for type {typeof(TSource)}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSource Last()
        {
            for (var index = source.Length - 1; index >= 0; index--)
            {
                if (predicate(source[index]))
                    return source[index];
            }
            throw new InvalidOperationException("Sequence contains no matching element");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<TSource> LastOrNone()
        {
            for (var index = source.Length - 1; index >= 0; index--)
            {
                if (predicate(source[index]))
                    return Option<TSource>.Some(source[index]);
            }
            return Option<TSource>.None();
        }

        public TSource[] ToArray()
        {
            var list = new List<TSource>();
            for (var i = 0; i < source.Length; i++)
            {
                if (predicate(source[i]))
                    list.Add(source[i]);
            }
            return list.ToArray();
        }

        public List<TSource> ToList()
        {
            var list = new List<TSource>();
            for (var i = 0; i < source.Length; i++)
            {
                if (predicate(source[i]))
                    list.Add(source[i]);
            }
            return list;
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
