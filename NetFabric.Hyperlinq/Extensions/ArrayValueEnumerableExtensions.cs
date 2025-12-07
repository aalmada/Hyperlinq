using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ArrayValueEnumerableExtensions
    {
        extension<T>(ArrayValueEnumerable<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum()
                => System.Numerics.Tensors.TensorPrimitives.Sum<T>(source.Source);

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

        extension<T>(ArrayValueEnumerable<T> source)
            where T : INumber<T>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Min()
            {
                if (source.Count == 0)
                    throw new InvalidOperationException("Sequence contains no elements");
                return System.Numerics.Tensors.TensorPrimitives.Min<T>(source.Source);
            }

            public T Min(Func<T, bool> predicate)
            {
                var hasValue = false;
                var min = default(T);
                
                foreach (var item in source)
                {
                    if (predicate(item))
                    {
                        if (!hasValue || item < min!)
                        {
                            min = item;
                            hasValue = true;
                        }
                    }
                }
                
                if (!hasValue)
                    throw new InvalidOperationException("Sequence contains no matching element");
                
                return min!;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Max()
            {
                if (source.Count == 0)
                    throw new InvalidOperationException("Sequence contains no elements");
                return System.Numerics.Tensors.TensorPrimitives.Max<T>(source.Source);
            }

            public T Max(Func<T, bool> predicate)
            {
                var hasValue = false;
                var max = default(T);
                
                foreach (var item in source)
                {
                    if (predicate(item))
                    {
                        if (!hasValue || item > max!)
                        {
                            max = item;
                            hasValue = true;
                        }
                    }
                }
                
                if (!hasValue)
                    throw new InvalidOperationException("Sequence contains no matching element");
                
                return max!;
            }
        }

        extension<T>(ArrayValueEnumerable<T> source)
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
            public SelectArrayEnumerable<T, TResult> Select<TResult>(Func<T, TResult> selector)
                => new SelectArrayEnumerable<T, TResult>(source.Source, selector);



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

            /// <summary>
            /// Returns the last element of a sequence.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last()
            {
                if (source.Count == 0)
                    throw new InvalidOperationException("Sequence contains no elements");
                return source.Source[^1];
            }

            /// <summary>
            /// Returns the last element that satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last(Func<T, bool> predicate)
            {
                var array = source.Source;
                for (var index = array.Length - 1; index >= 0; index--)
                {
                    if (predicate(array[index]))
                        return array[index];
                }
                throw new InvalidOperationException("Sequence contains no matching element");
            }
        }

        // Direct array extensions returning ref struct enumerables (maximum performance, foreach-only)

    }
}
