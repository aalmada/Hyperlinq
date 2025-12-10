using System;
using System.Buffers;
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

            // Primary overload - value delegate
            public T Sum<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => SumImpl(source, predicate);

            // Overload for IFunctionIn - passed by reference
            public T Sum<TPredicate>(in TPredicate predicate)
                where TPredicate : struct, IFunctionIn<T, bool>
                => SumInImpl(source, predicate);

            // Secondary overload - Func wrapper for backward compatibility
            public T Sum(Func<T, bool> predicate)
                => SumImpl(source, new FunctionWrapper<T, bool>(predicate));
        }

        static T SumImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            where TPredicate : struct, IFunction<T, bool>
        {
            var sum = T.AdditiveIdentity;
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                    sum += item;
            }
            return sum;
        }

        static T SumInImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            where TPredicate : struct, IFunctionIn<T, bool>
        {
            var sum = T.AdditiveIdentity;
            foreach (var item in source)
            {
                if (predicate.Invoke(in item))
                    sum += item;
            }
            return sum;
        }

        extension<T>(ReadOnlySpan<T> source)
            where T : INumber<T>
        {
            public T Min()
            {
                if (source.Length == 0)
                    throw new InvalidOperationException("Sequence contains no elements");
                return System.Numerics.Tensors.TensorPrimitives.Min(source);
            }

            // Primary overload - value delegate
            public T Min<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => MinImpl(source, predicate);

            // Secondary overload - Func wrapper for backward compatibility
            public T Min(Func<T, bool> predicate)
                => MinImpl(source, new FunctionWrapper<T, bool>(predicate));

            public T Max()
            {
                if (source.Length == 0)
                    throw new InvalidOperationException("Sequence contains no elements");
                return System.Numerics.Tensors.TensorPrimitives.Max(source);
            }

            // Primary overload - value delegate
            public T Max<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => MaxImpl(source, predicate);

            // Secondary overload - Func wrapper for backward compatibility
            public T Max(Func<T, bool> predicate)
                => MaxImpl(source, new FunctionWrapper<T, bool>(predicate));

            /// <summary>
            /// Computes both minimum and maximum values in a single iteration.
            /// </summary>
            public (T Min, T Max) MinMax()
            {
                if (source.Length == 0)
                    throw new InvalidOperationException("Sequence contains no elements");
                
                var min = System.Numerics.Tensors.TensorPrimitives.Min(source);
                var max = System.Numerics.Tensors.TensorPrimitives.Max(source);
                return (min, max);
            }

            /// <summary>
            /// Computes both minimum and maximum values for elements that satisfy a condition.
            /// </summary>
            public (T Min, T Max) MinMax<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => MinMaxImpl(source, predicate);

            public (T Min, T Max) MinMax(Func<T, bool> predicate)
                => MinMaxImpl(source, new FunctionWrapper<T, bool>(predicate));
        }

        static (T Min, T Max) MinMaxImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where T : INumber<T>
            where TPredicate : struct, IFunction<T, bool>
        {
            // Find first matching element
            var index = 0;
            while (index < source.Length && !predicate.Invoke(source[index]))
                index++;
            
            if (index >= source.Length)
                throw new InvalidOperationException("Sequence contains no matching element");
            
            var min = source[index];
            var max = source[index];
            index++;
            
            // Process remaining elements with minimal branching
            while (index < source.Length)
            {
                var item = source[index];
                if (predicate.Invoke(item))
                {
                    // If item < min, it cannot be > max, so use else if
                    if (item < min)
                        min = item;
                    else if (item > max)
                        max = item;
                }
                index++;
            }
            
            return (min, max);
        }


        static T MinImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where T : INumber<T>
            where TPredicate : struct, IFunction<T, bool>
        {
            // Find first matching element
            var index = 0;
            while (index < source.Length && !predicate.Invoke(source[index]))
                index++;
            
            if (index >= source.Length)
                throw new InvalidOperationException("Sequence contains no matching element");
            
            var min = source[index];
            index++;
            
            // Process remaining elements without branching on hasValue
            while (index < source.Length)
            {
                var item = source[index];
                if (predicate.Invoke(item) && item < min)
                    min = item;
                index++;
            }
            
            return min;
        }

        static T MaxImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where T : INumber<T>
            where TPredicate : struct, IFunction<T, bool>
        {
            // Find first matching element
            var index = 0;
            while (index < source.Length && !predicate.Invoke(source[index]))
                index++;
            
            if (index >= source.Length)
                throw new InvalidOperationException("Sequence contains no matching element");
            
            var max = source[index];
            index++;
            
            // Process remaining elements without branching on hasValue
            while (index < source.Length)
            {
                var item = source[index];
                if (predicate.Invoke(item) && item > max)
                    max = item;
                index++;
            }
            
            return max;
        }

        extension<T>(ReadOnlySpan<T> source)
            where T : INumber<T>
        {
            /// <summary>
            /// Computes the average of a sequence using SIMD acceleration.
            /// </summary>
            public T Average()
                => source.AverageOrNone().Value;

            /// <summary>
            /// Computes the average of elements that satisfy a condition.
            /// </summary>
            public T Average<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.AverageOrNone(predicate).Value;

            public T Average(Func<T, bool> predicate)
                => source.AverageOrNone(predicate).Value;

            /// <summary>
            /// Computes the average of a sequence, returning None if empty.
            /// </summary>
            public Option<T> AverageOrNone()
            {
                if (source.Length == 0)
                    return Option<T>.None();
                
                var sum = System.Numerics.Tensors.TensorPrimitives.Sum<T>(source);
                return Option<T>.Some(sum / T.CreateChecked(source.Length));
            }

            /// <summary>
            /// Computes the average of elements that satisfy a condition, returning None if no matches.
            /// </summary>
            public Option<T> AverageOrNone<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => AverageOrNoneImpl(source, predicate);

            public Option<T> AverageOrNone(Func<T, bool> predicate)
                => AverageOrNoneImpl(source, new FunctionWrapper<T, bool>(predicate));
        }

        static Option<T> AverageOrNoneImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where T : INumber<T>
            where TPredicate : struct, IFunction<T, bool>
        {
            var sum = T.AdditiveIdentity;
            var count = 0;
            
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                {
                    sum += item;
                    count++;
                }
            }
            
            if (count == 0)
                return Option<T>.None();
            
            return Option<T>.Some(sum / T.CreateChecked(count));
        }



        // Unconstrained extension block - for all other operations
        extension<T>(ReadOnlySpan<T> source)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any()
                => source.Length > 0;

            public bool Any<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => AnyImpl(source, predicate);

            public bool Any(Func<T, bool> predicate)
                => AnyImpl(source, new FunctionWrapper<T, bool>(predicate));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count()
                => source.Length;

            public int Count<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CountImpl(source, predicate);

            public int Count(Func<T, bool> predicate)
                => CountImpl(source, new FunctionWrapper<T, bool>(predicate));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T First()
                => source.FirstOrNone().Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T First<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.FirstOrNone(predicate).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T First(Func<T, bool> predicate)
                => source.FirstOrNone(predicate).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault()
                => source.Length == 0 ? default! : source[0];

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.FirstOrNone(predicate).GetValueOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(Func<T, bool> predicate)
                => source.FirstOrNone(predicate).GetValueOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
                where TPredicate : struct, IFunction<T, bool>
                => source.FirstOrNone(predicate).GetValueOrDefault(defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(Func<T, bool> predicate, T defaultValue)
                => source.FirstOrNone(predicate).GetValueOrDefault(defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone()
                => source.Length == 0 ? Option<T>.None() : Option<T>.Some(source[0]);

            public Option<T> FirstOrNone<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => FirstOrNoneImpl(source, predicate);

            public Option<T> FirstOrNone(Func<T, bool> predicate)
                => FirstOrNoneImpl(source, new FunctionWrapper<T, bool>(predicate));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SelectReadOnlySpanEnumerable<T, TResult, TSelector> Select<TResult, TSelector>(TSelector selector)
                where TSelector : struct, IFunction<T, TResult>
                => new SelectReadOnlySpanEnumerable<T, TResult, TSelector>(source, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SelectReadOnlySpanEnumerable<T, TResult, FunctionWrapper<T, TResult>> Select<TResult>(Func<T, TResult> selector)
                => new SelectReadOnlySpanEnumerable<T, TResult, FunctionWrapper<T, TResult>>(source, new FunctionWrapper<T, TResult>(selector));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SelectReadOnlySpanInEnumerable<T, TResult, TSelector> Select<TResult, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<T, TResult>
                => new SelectReadOnlySpanInEnumerable<T, TResult, TSelector>(source, selector);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public WhereReadOnlySpanEnumerable<T, TPredicate> Where<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => new WhereReadOnlySpanEnumerable<T, TPredicate>(source, predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>> Where(Func<T, bool> predicate)
                => new WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>>(source, new FunctionWrapper<T, bool>(predicate));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public WhereReadOnlySpanInEnumerable<T, TPredicate> Where<TPredicate>(in TPredicate predicate)
                where TPredicate : struct, IFunctionIn<T, bool>
                => new WhereReadOnlySpanInEnumerable<T, TPredicate>(source, predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Single()
                => source.SingleOrNone().Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault()
            {
                if (source.Length == 0)
                    return default!;
                if (source.Length > 1)
                    throw new InvalidOperationException("Sequence contains more than one element");
                return source[0];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Single<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.SingleOrNone(predicate).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Single(Func<T, bool> predicate)
                => source.SingleOrNone(predicate).Value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.SingleOrNone(predicate).GetValueOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault(Func<T, bool> predicate)
                => source.SingleOrNone(predicate).GetValueOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
                where TPredicate : struct, IFunction<T, bool>
                => source.SingleOrNone(predicate).GetValueOrDefault(defaultValue);

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

            public Option<T> SingleOrNone<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => SingleOrNoneImpl(source, predicate);

            public Option<T> SingleOrNone(Func<T, bool> predicate)
                => SingleOrNoneImpl(source, new FunctionWrapper<T, bool>(predicate));

            /// <summary>
            /// Returns the last element of a sequence.
            /// </summary>
            public T Last()
            {
                if (source.Length == 0)
                    throw new InvalidOperationException("Sequence contains no elements");
                return source[^1];
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => LastImpl(source, predicate);

            public T Last(Func<T, bool> predicate)
                => LastImpl(source, new FunctionWrapper<T, bool>(predicate));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> LastOrNone()
                => source.Length == 0 ? Option<T>.None() : Option<T>.Some(source[^1]);

            public Option<T> LastOrNone<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => LastOrNoneImpl(source, predicate);

            public Option<T> LastOrNone(Func<T, bool> predicate)
                => LastOrNoneImpl(source, new FunctionWrapper<T, bool>(predicate));

            public T[] ToArray<TPredicate>(TPredicate predicate, ArrayPool<T>? pool = default)
                where TPredicate : struct, IFunction<T, bool>
                => ToArrayImpl(source, predicate, pool);

            public T[] ToArray<TPredicate>(in TPredicate predicate, ArrayPool<T>? pool = default)
                where TPredicate : struct, IFunctionIn<T, bool>
                => ToArrayInImpl(source, predicate, pool);

            public T[] ToArray(Func<T, bool> predicate, ArrayPool<T>? pool = default)
                => ToArrayImpl(source, new FunctionWrapper<T, bool>(predicate), pool);

            public List<T> ToList<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => ToListImpl(source, predicate);

            public List<T> ToList<TPredicate>(in TPredicate predicate)
                where TPredicate : struct, IFunctionIn<T, bool>
                => ToListInImpl(source, predicate);

            public List<T> ToList(Func<T, bool> predicate)
                => ToListImpl(source, new FunctionWrapper<T, bool>(predicate));

            /// <summary>
            /// Materializes the span into a pooled buffer. The buffer must be disposed to return memory to the pool.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToArrayPooled()
                => source.ToArrayPooled((ArrayPool<T>?)null);

            /// <summary>
            /// Materializes the span into a pooled buffer using the specified pool. The buffer must be disposed to return memory to the pool.
            /// </summary>
            /// <param name="pool">The ArrayPool to use, or null to use ArrayPool.Shared.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToArrayPooled(ArrayPool<T>? pool)
            {
                var poolToUse = pool ?? ArrayPool<T>.Shared;
                var buffer = poolToUse.Rent(source.Length);
                source.CopyTo(buffer);
                return new PooledBuffer<T>(buffer, source.Length, pool);
            }

            /// <summary>
            /// Filters and materializes the span into a pooled buffer. The buffer must be disposed to return memory to the pool.
            /// Uses dynamic growth strategy since result size is unknown.
            /// </summary>
            public PooledBuffer<T> ToArrayPooled<TPredicate>(TPredicate predicate, ArrayPool<T>? pool = default)
                where TPredicate : struct, IFunction<T, bool>
                => ToArrayPooledImpl(source, predicate, pool);

            public PooledBuffer<T> ToArrayPooled<TPredicate>(in TPredicate predicate, ArrayPool<T>? pool = default)
                where TPredicate : struct, IFunctionIn<T, bool>
                => ToArrayPooledInImpl(source, predicate, pool);

            public PooledBuffer<T> ToArrayPooled(Func<T, bool> predicate, ArrayPool<T>? pool = default)
                => ToArrayPooledImpl(source, new FunctionWrapper<T, bool>(predicate), pool);


            /// <summary>
            /// Bypasses a specified number of elements and returns the remaining elements.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlySpan<T> Skip(int count)
                => count <= 0 ? source : source[(count < source.Length ? count : source.Length)..];

            /// <summary>
            /// Returns a specified number of contiguous elements from the start.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlySpan<T> Take(int count)
                => count <= 0 ? ReadOnlySpan<T>.Empty : source[..(count < source.Length ? count : source.Length)];

        }

        static bool AnyImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
        {
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                    return true;
            }
            return false;
        }

        static int CountImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
        {
            var count = 0;
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                    count++;
            }
            return count;
        }
        static T[] ToArrayImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate, ArrayPool<T>? pool)
            where TPredicate : struct, IFunction<T, bool>
        {
            using var builder = new ArrayBuilder<T>(pool ?? ArrayPool<T>.Shared);
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                    builder.Add(item);
            }
            return builder.ToArray();
        }

        static List<T> ToListImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
        {
            var list = new List<T>();
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                    list.Add(item);
            }
            return list;
        }

        static PooledBuffer<T> ToArrayPooledImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate, ArrayPool<T>? pool)
            where TPredicate : struct, IFunction<T, bool>
        {
            using var builder = new ArrayBuilder<T>(pool ?? ArrayPool<T>.Shared);
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                    builder.Add(item);
            }
            return builder.ToPooledBuffer();
        }

        static T[] ToArrayInImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate, ArrayPool<T>? pool)
            where TPredicate : struct, IFunctionIn<T, bool>
        {
            using var builder = new ArrayBuilder<T>(pool ?? ArrayPool<T>.Shared);
            foreach (var item in source)
            {
                if (predicate.Invoke(in item))
                    builder.Add(item);
            }
            return builder.ToArray();
        }

        static List<T> ToListInImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
        {
            var list = new List<T>();
            foreach (var item in source)
            {
                if (predicate.Invoke(in item))
                    list.Add(item);
            }
            return list;
        }

        static PooledBuffer<T> ToArrayPooledInImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate, ArrayPool<T>? pool)
            where TPredicate : struct, IFunctionIn<T, bool>
        {
            using var builder = new ArrayBuilder<T>(pool ?? ArrayPool<T>.Shared);
            foreach (var item in source)
            {
                if (predicate.Invoke(in item))
                    builder.Add(item);
            }
            return builder.ToPooledBuffer();
        }

        static Option<T> LastOrNoneImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
        {
            for (var index = source.Length - 1; index >= 0; index--)
            {
                if (predicate.Invoke(source[index]))
                    return Option<T>.Some(source[index]);
            }
            return Option<T>.None();
        }

        static Option<T> FirstOrNoneImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
        {
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                    return Option<T>.Some(item);
            }
            return Option<T>.None();
        }

        static Option<T> SingleOrNoneImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
        {
            var found = false;
            var result = default(T);
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                {
                    if (found)
                        throw new InvalidOperationException("Sequence contains more than one matching element");
                    
                    found = true;
                    result = item;
                }
            }
            return found ? Option<T>.Some(result!) : Option<T>.None();
        }
        static T LastImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
        {
            for (var index = source.Length - 1; index >= 0; index--)
            {
                if (predicate.Invoke(source[index]))
                    return source[index];
            }
            throw new InvalidOperationException("Sequence contains no matching element");
        }
    }
}
