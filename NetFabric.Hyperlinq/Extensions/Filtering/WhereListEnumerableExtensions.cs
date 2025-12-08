using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereListEnumerableExtensions
    {
        /// <summary>
        /// Fuses consecutive Where operations by combining predicates with AND logic.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereListEnumerable<TSource> Where<TSource>(this WhereListEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var firstPredicate = source.Predicate;
            return new WhereListEnumerable<TSource>(source.Source, item => firstPredicate(item) && predicate(item));
        }

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
        public static TSource Min<TSource>(this WhereListEnumerable<TSource> source)
            where TSource : INumber<TSource>
        {
            var hasValue = false;
            var min = default(TSource);
            foreach (var item in source)
            {
                if (!hasValue || item < min!)
                {
                    min = item;
                    hasValue = true;
                }
            }
            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no elements");
            return min!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Max<TSource>(this WhereListEnumerable<TSource> source)
            where TSource : INumber<TSource>
        {
            var hasValue = false;
            var max = default(TSource);
            foreach (var item in source)
            {
                if (!hasValue || item > max!)
                {
                    max = item;
                    hasValue = true;
                }
            }
            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no elements");
            return max!;
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

        public static PooledBuffer<TSource> ToArrayPooled<TSource>(this WhereListEnumerable<TSource> source)
            => source.ToArrayPooled((ArrayPool<TSource>?)null);

        public static PooledBuffer<TSource> ToArrayPooled<TSource>(this WhereListEnumerable<TSource> source, ArrayPool<TSource>? pool)
        {
            var poolToUse = pool ?? ArrayPool<TSource>.Shared;
            var capacity = PooledBuffer<TSource>.GetDefaultInitialCapacity();
            var buffer = poolToUse.Rent(capacity);
            var count = 0;

            try
            {
                var span = CollectionsMarshal.AsSpan(source.Source);
                var predicate = source.Predicate;
                
                for (var i = 0; i < span.Length; i++)
                {
                    if (predicate(span[i]))
                    {
                        // Grow buffer if needed
                        if (count == buffer.Length)
                        {
                            var newCapacity = PooledBuffer<TSource>.GetNextCapacity(capacity);
                            var newBuffer = poolToUse.Rent(newCapacity);
                            
                            // Copy existing elements
                            Array.Copy(buffer, newBuffer, count);
                            
                            // Return old buffer
                            poolToUse.Return(buffer, RuntimeHelpers.IsReferenceOrContainsReferences<TSource>());
                            
                            buffer = newBuffer;
                            capacity = newCapacity;
                        }

                        buffer[count++] = span[i];
                    }
                }

                return new PooledBuffer<TSource>(buffer, count, pool);
            }
            catch
            {
                // Return buffer on exception
                poolToUse.Return(buffer, RuntimeHelpers.IsReferenceOrContainsReferences<TSource>());
                throw;
            }
        }


    }
}
