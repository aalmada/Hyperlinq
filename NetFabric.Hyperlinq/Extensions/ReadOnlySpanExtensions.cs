using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ReadOnlySpanExtensions
    {
        // Constrained extension block - only for Sum operations
        extension<T>(ReadOnlySpan<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            public T Sum()
                => System.Numerics.Tensors.TensorPrimitives.Sum<T>(source);

            public T Sum(Func<T, bool> predicate)
            {
                var sum = T.AdditiveIdentity;
                foreach (var item in source)
                {
                    if (predicate(item))
                        sum += item;
                }
                return sum;
            }
        }

        // Unconstrained extension block - for all other operations
        extension<T>(ReadOnlySpan<T> source)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any()
                => source.Length > 0;

            public bool Any(Func<T, bool> predicate)
            {
                foreach (var item in source)
                {
                    if (predicate(item))
                        return true;
                }
                return false;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count()
                => source.Length;

            public int Count(Func<T, bool> predicate)
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
            public T First()
                => source.FirstOrNone().Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T First(Func<T, bool> predicate)
                => source.FirstOrNone(predicate).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault()
                => source.FirstOrNone().GetValueOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(T defaultValue)
                => source.FirstOrNone().GetValueOrDefault(defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(Func<T, bool> predicate)
                => source.FirstOrNone(predicate).GetValueOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(Func<T, bool> predicate, T defaultValue)
                => source.FirstOrNone(predicate).GetValueOrDefault(defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone()
                => source.Length == 0 ? Option<T>.None() : Option<T>.Some(source[0]);

            public Option<T> FirstOrNone(Func<T, bool> predicate)
            {
                foreach (var item in source)
                {
                    if (predicate(item))
                        return Option<T>.Some(item);
                }
                return Option<T>.None();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SelectReadOnlySpanEnumerable<T, TResult> Select<TResult>(Func<T, TResult> selector)
                => new SelectReadOnlySpanEnumerable<T, TResult>(source, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public WhereReadOnlySpanEnumerable<T> Where(Func<T, bool> predicate)
                => new WhereReadOnlySpanEnumerable<T>(source, predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Single()
                => source.SingleOrNone().Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Single(Func<T, bool> predicate)
                => source.SingleOrNone(predicate).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault()
                => source.SingleOrNone().GetValueOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault(T defaultValue)
                => source.SingleOrNone().GetValueOrDefault(defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault(Func<T, bool> predicate)
                => source.SingleOrNone(predicate).GetValueOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault(Func<T, bool> predicate, T defaultValue)
                => source.SingleOrNone(predicate).GetValueOrDefault(defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> SingleOrNone()
            {
                if (source.Length == 0)
                    return Option<T>.None();
                if (source.Length > 1)
                    throw new InvalidOperationException("Sequence contains more than one element");
                return Option<T>.Some(source[0]);
            }

            public Option<T> SingleOrNone(Func<T, bool> predicate)
            {
                var found = false;
                var result = default(T);
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
                return found ? Option<T>.Some(result!) : Option<T>.None();
            }

            /// <summary>
            /// Returns the last element of a sequence.
            /// </summary>
            public T Last()
            {
                if (source.Length == 0)
                    throw new InvalidOperationException("Sequence contains no elements");
                return source[^1];
            }

            /// <summary>
            /// Returns the last element that satisfies a condition.
            /// </summary>
            public T Last(Func<T, bool> predicate)
            {
                for (var index = source.Length - 1; index >= 0; index--)
                {
                    if (predicate(source[index]))
                        return source[index];
                }
                throw new InvalidOperationException("Sequence contains no matching element");
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> LastOrNone()
                => source.Length == 0 ? Option<T>.None() : Option<T>.Some(source[^1]);

            public Option<T> LastOrNone(Func<T, bool> predicate)
            {
                for (var index = source.Length - 1; index >= 0; index--)
                {
                    if (predicate(source[index]))
                        return Option<T>.Some(source[index]);
                }
                return Option<T>.None();
            }

            public T[] ToArray(Func<T, bool> predicate)
            {
                var list = new List<T>();
                foreach (var item in source)
                {
                    if (predicate(item))
                        list.Add(item);
                }
                return list.ToArray();
            }

            public List<T> ToList(Func<T, bool> predicate)
            {
                var list = new List<T>();
                foreach (var item in source)
                {
                    if (predicate(item))
                        list.Add(item);
                }
                return list;
            }
        }
    }
}
