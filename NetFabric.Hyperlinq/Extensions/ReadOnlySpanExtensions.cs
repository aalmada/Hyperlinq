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
        }

        static T MinImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where T : INumber<T>
            where TPredicate : struct, IFunction<T, bool>
        {
            var hasValue = false;
            var min = default(T);
            
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                {
                    if (!hasValue || item < min!)
                    {
                        min = item;
                        hasValue = true;
                    }
                }
            }
            
            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no elements");
            return min!;
        }

        static T MaxImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where T : INumber<T>
            where TPredicate : struct, IFunction<T, bool>
        {
            var hasValue = false;
            var max = default(T);
            
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                {
                    if (!hasValue || item > max!)
                    {
                        max = item;
                        hasValue = true;
                    }
                }
            }
            
            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no elements");
            return max!;
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

            public T[] ToArray<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => ToArrayImpl(source, predicate);

            public T[] ToArray(Func<T, bool> predicate)
                => ToArrayImpl(source, new FunctionWrapper<T, bool>(predicate));

            public List<T> ToList<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => ToListImpl(source, predicate);

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

            public PooledBuffer<T> ToArrayPooled(Func<T, bool> predicate, ArrayPool<T>? pool = default)
                => ToArrayPooledImpl(source, new FunctionWrapper<T, bool>(predicate), pool);


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
        static T[] ToArrayImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
        {
            var list = new List<T>();
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                    list.Add(item);
            }
            return list.ToArray();
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
            var poolToUse = pool ?? ArrayPool<T>.Shared;
            var capacity = PooledBuffer<T>.GetDefaultInitialCapacity();
            var buffer = poolToUse.Rent(capacity);
            var count = 0;

            try
            {
                foreach (var item in source)
                {
                    if (predicate.Invoke(item))
                    {
                        // Grow buffer if needed
                        if (count == buffer.Length)
                        {
                            var newCapacity = PooledBuffer<T>.GetNextCapacity(buffer.Length);
                            var newBuffer = poolToUse.Rent(newCapacity);
                            
                            // Copy existing elements
                            Array.Copy(buffer, newBuffer, count);
                            
                            // Return old buffer
                            poolToUse.Return(buffer, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
                            
                            buffer = newBuffer;
                            capacity = newCapacity;
                        }

                        buffer[count++] = item;
                    }
                }

                return new PooledBuffer<T>(buffer, count, pool);
            }
            catch
            {
                // Return buffer on exception
                poolToUse.Return(buffer, RuntimeHelpers.IsReferenceOrContainsReferences<T>());
                throw;
            }
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
