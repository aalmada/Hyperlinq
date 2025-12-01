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

        extension<T>(ReadOnlySpan<T> source)
            where T : INumber<T>
        {
            public T Min()
            {
                if (source.Length == 0)
                    throw new InvalidOperationException("Sequence contains no elements");
                return System.Numerics.Tensors.TensorPrimitives.Min(source);
            }

            public T Max()
            {
                if (source.Length == 0)
                    throw new InvalidOperationException("Sequence contains no elements");
                return System.Numerics.Tensors.TensorPrimitives.Max(source);
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
            public PooledBuffer<T> ToArrayPooled(Func<T, bool> predicate)
                => source.ToArrayPooled(predicate, (ArrayPool<T>?)null);

            /// <summary>
            /// Filters and materializes the span into a pooled buffer using the specified pool. The buffer must be disposed to return memory to the pool.
            /// Uses dynamic growth strategy since result size is unknown.
            /// </summary>
            /// <param name="predicate">The filter predicate.</param>
            /// <param name="pool">The ArrayPool to use, or null to use ArrayPool.Shared.</param>
            public PooledBuffer<T> ToArrayPooled(Func<T, bool> predicate, ArrayPool<T>? pool)
            {
                var poolToUse = pool ?? ArrayPool<T>.Shared;
                var capacity = PooledBuffer<T>.GetDefaultInitialCapacity();
                var buffer = poolToUse.Rent(capacity);
                var count = 0;

                try
                {
                    foreach (var item in source)
                    {
                        if (predicate(item))
                        {
                            // Grow buffer if needed
                            if (count == buffer.Length)
                            {
                                var newCapacity = PooledBuffer<T>.GetNextCapacity(capacity);
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

            /// <summary>
            /// Alias for ToArrayPooled(). Both return PooledBuffer&lt;T&gt;.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToListPooled()
                => source.ToArrayPooled();

            /// <summary>
            /// Alias for ToArrayPooled(pool). Both return PooledBuffer&lt;T&gt;.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToListPooled(ArrayPool<T>? pool)
                => source.ToArrayPooled(pool);

            /// <summary>
            /// Alias for ToArrayPooled(predicate). Both return PooledBuffer&lt;T&gt;.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToListPooled(Func<T, bool> predicate)
                => source.ToArrayPooled(predicate);

            /// <summary>
            /// Alias for ToArrayPooled(predicate, pool). Both return PooledBuffer&lt;T&gt;.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToListPooled(Func<T, bool> predicate, ArrayPool<T>? pool)
                => source.ToArrayPooled(predicate, pool);
        }
    }
}
