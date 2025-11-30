using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ListValueEnumerableExtensions
    {
        extension<T>(ListValueEnumerable<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum()
                => System.Numerics.Tensors.TensorPrimitives.Sum<T>(CollectionsMarshal.AsSpan(source.Source));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            public SelectListEnumerable<T, TResult> Select<TResult>(Func<T, TResult> selector)
                => new SelectListEnumerable<T, TResult>(source.Source, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public WhereListEnumerable<T> Where(Func<T, bool> predicate)
                => new WhereListEnumerable<T>(source.Source, predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T First(Func<T, bool> predicate)
                => source.FirstOrNone(predicate).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(Func<T, bool> predicate)
                => source.FirstOrNone(predicate).GetValueOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(Func<T, bool> predicate, T defaultValue)
                => source.FirstOrNone(predicate).GetValueOrDefault(defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone()
            {
                if (source.Count == 0)
                    return Option<T>.None();
                return Option<T>.Some(source[0]);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            public T Single(Func<T, bool> predicate)
                => source.SingleOrNone(predicate).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault(Func<T, bool> predicate)
                => source.SingleOrNone(predicate).GetValueOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault(Func<T, bool> predicate, T defaultValue)
                => source.SingleOrNone(predicate).GetValueOrDefault(defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> SingleOrNone()
            {
                if (source.Count == 0)
                    return Option<T>.None();
                if (source.Count > 1)
                    throw new InvalidOperationException("Sequence contains more than one element");
                return Option<T>.Some(source[0]);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        }

        // Direct List extensions returning ref struct enumerables (maximum performance, foreach-only)
        extension<TSource>(List<TSource> source)
        {
            /// <summary>
            /// Projects each element of a List into a new form using ref struct enumerable.
            /// For maximum performance in foreach-only scenarios.
            /// Use AsValueEnumerable().Select() if you need to chain operations.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SelectListRefStructEnumerable<TSource, TResult> Select<TResult>(Func<TSource, TResult> selector)
                => new SelectListRefStructEnumerable<TSource, TResult>(source, selector);

            /// <summary>
            /// Filters a List based on a predicate using ref struct enumerable.
            /// For maximum performance in foreach-only scenarios.
            /// Use AsValueEnumerable().Where() if you need to chain operations.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public WhereListRefStructEnumerable<TSource> Where(Func<TSource, bool> predicate)
                => new WhereListRefStructEnumerable<TSource>(source, predicate);
        }
    }
}
