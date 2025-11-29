using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class EnumerableValueEnumerableExtensions
    {
        extension<T>(EnumerableValueEnumerable<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum()
            {
                var sum = T.AdditiveIdentity;
                foreach (var item in source)
                {
                    sum += item;
                }
                return sum;
            }

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
        }

        extension<T>(EnumerableValueEnumerable<T> source)
        {
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
                using var enumerator = source.GetEnumerator();
                if (enumerator.MoveNext())
                    return Option<T>.Some(enumerator.Current);
                return Option<T>.None();
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
                using var enumerator = source.GetEnumerator();
                if (!enumerator.MoveNext())
                    return Option<T>.None();
                
                var first = enumerator.Current;
                if (enumerator.MoveNext())
                    throw new InvalidOperationException("Sequence contains more than one element");

                return Option<T>.Some(first);
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SelectEnumerable<T, TResult> Select<TResult>(Func<T, TResult> selector)
                => new SelectEnumerable<T, TResult>(source.Source, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public WhereEnumerable<T> Where(Func<T, bool> predicate)
                => new WhereEnumerable<T>(source.Source, predicate);
        }
    }
}
