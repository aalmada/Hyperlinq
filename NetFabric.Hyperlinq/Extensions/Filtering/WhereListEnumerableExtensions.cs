using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereListEnumerableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereSelectListEnumerable<TSource, TResult> Select<TSource, TResult>(this WhereListEnumerable<TSource> source, Func<TSource, TResult> selector)
            => new WhereSelectListEnumerable<TSource, TResult>(source.Source, source.Predicate, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Sum<TSource>(this WhereListEnumerable<TSource> source)
            where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
        {
            var sum = TSource.AdditiveIdentity;
            foreach (var item in source)
            {
                sum += item;
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource>(this WhereListEnumerable<TSource> source)
        {
            var count = 0;
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    count++;
            }
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TSource>(this WhereListEnumerable<TSource> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource First<TSource>(this WhereListEnumerable<TSource> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    return span[i];
            }
            throw new InvalidOperationException("Sequence contains no elements");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TSource> FirstOrNone<TSource>(this WhereListEnumerable<TSource> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    return Option<TSource>.Some(span[i]);
            }
            return Option<TSource>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Single<TSource>(this WhereListEnumerable<TSource> source)
        {
            var found = false;
            var result = default(TSource);
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
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
        public static Option<TSource> SingleOrNone<TSource>(this WhereListEnumerable<TSource> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var found = false;
            var result = default(TSource);
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
            return found ? Option<TSource>.Some(result!) : Option<TSource>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Last<TSource>(this WhereListEnumerable<TSource> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var index = span.Length - 1; index >= 0; index--)
            {
                if (predicate(span[index]))
                    return span[index];
            }
            throw new InvalidOperationException("Sequence contains no matching element");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TSource> LastOrNone<TSource>(this WhereListEnumerable<TSource> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var index = span.Length - 1; index >= 0; index--)
            {
                if (predicate(span[index]))
                    return Option<TSource>.Some(span[index]);
            }
            return Option<TSource>.None();
        }

        public static TSource[] ToArray<TSource>(this WhereListEnumerable<TSource> source)
        {
            var list = new List<TSource>();
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    list.Add(span[i]);
            }
            return list.ToArray();
        }

        public static List<TSource> ToList<TSource>(this WhereListEnumerable<TSource> source)
        {
            var list = new List<TSource>();
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    list.Add(span[i]);
            }
            return list;
        }
    }
}
