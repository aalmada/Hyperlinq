using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WhereSelectListEnumerableExtensions
    {
        /// <summary>
        /// Fuses consecutive Where operations by combining predicates with AND logic.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereSelectListEnumerable<TSource, TResult> Where<TSource, TResult>(
            this WhereSelectListEnumerable<TSource, TResult> source, 
            Func<TResult, bool> predicate)
        {
            var sourcePredicate = source.Predicate;
            var selector = source.Selector;
            // Merge: first apply source predicate, then selector, then new predicate
            return new WhereSelectListEnumerable<TSource, TResult>(
                source.Source,
                item => sourcePredicate(item) && predicate(selector(item)),
                selector
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
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
        public static bool Any<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
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
        public static TResult First<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    return selector(span[i]);
            }
            throw new InvalidOperationException("Sequence contains no elements");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> FirstOrNone<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    return Option<TResult>.Some(selector(span[i]));
            }
            return Option<TResult>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Single<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var found = false;
            var result = default(TResult);
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                {
                    if (found)
                        throw new InvalidOperationException("Sequence contains more than one element");
                    result = selector(span[i]);
                    found = true;
                }
            }
            if (!found)
                throw new InvalidOperationException("Sequence contains no elements");
            return result!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> SingleOrNone<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            var found = false;
            var result = default(TResult);
            foreach (var item in span)
            {
                if (predicate(item))
                {
                    if (found)
                        throw new InvalidOperationException("Sequence contains more than one matching element");
                    found = true;
                    result = selector(item);
                }
            }
            return found ? Option<TResult>.Some(result!) : Option<TResult>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Min<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
            where TResult : INumber<TResult>
        {
            var hasValue = false;
            var min = default(TResult);
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                {
                    var value = selector(span[i]);
                    if (!hasValue || value < min!)
                    {
                        min = value;
                        hasValue = true;
                    }
                }
            }
            
            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no elements");
            return min!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Max<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
            where TResult : INumber<TResult>
        {
            var hasValue = false;
            var max = default(TResult);
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                {
                    var value = selector(span[i]);
                    if (!hasValue || value > max!)
                    {
                        max = value;
                        hasValue = true;
                    }
                }
            }
            
            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no elements");
            return max!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Sum<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
            where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
        {
            var sum = TResult.AdditiveIdentity;
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                {
                    sum += selector(span[i]);
                }
            }
            
            return sum;
        }

        public static TResult[] ToArray<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var list = new List<TResult>();
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    list.Add(selector(span[i]));
            }
            return list.ToArray();
        }

        public static List<TResult> ToList<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
        {
            var list = new List<TResult>();
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                    list.Add(selector(span[i]));
            }
            return list;
        }

        public static PooledBuffer<TResult> ToArrayPooled<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source)
            => source.ToArrayPooled((ArrayPool<TResult>?)null);

        public static PooledBuffer<TResult> ToArrayPooled<TSource, TResult>(this WhereSelectListEnumerable<TSource, TResult> source, ArrayPool<TResult>? pool)
        {
            var poolToUse = pool ?? ArrayPool<TResult>.Shared;
            var capacity = PooledBuffer<TResult>.GetDefaultInitialCapacity();
            var buffer = poolToUse.Rent(capacity);
            var count = 0;

            try
            {
                var span = CollectionsMarshal.AsSpan(source.Source);
                var predicate = source.Predicate;
                var selector = source.Selector;
                
                for (var i = 0; i < span.Length; i++)
                {
                    if (predicate(span[i]))
                    {
                        // Grow buffer if needed
                        if (count == buffer.Length)
                        {
                            var newCapacity = PooledBuffer<TResult>.GetNextCapacity(capacity);
                            var newBuffer = poolToUse.Rent(newCapacity);
                            
                            // Copy existing elements
                            Array.Copy(buffer, newBuffer, count);
                            
                            // Return old buffer
                            poolToUse.Return(buffer, RuntimeHelpers.IsReferenceOrContainsReferences<TResult>());
                            
                            buffer = newBuffer;
                            capacity = newCapacity;
                        }

                        buffer[count++] = selector(span[i]);
                    }
                }

                return new PooledBuffer<TResult>(buffer, count, pool);
            }
            catch
            {
                // Return buffer on exception
                poolToUse.Return(buffer, RuntimeHelpers.IsReferenceOrContainsReferences<TResult>());
                throw;
            }
        }


    }
}
