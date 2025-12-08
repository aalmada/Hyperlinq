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
        public static WhereListEnumerable<TSource, PredicateAnd<TSource, TPredicate, FunctionWrapper<TSource, bool>>> Where<TSource, TPredicate>(
            this WhereListEnumerable<TSource, TPredicate> source, 
            Func<TSource, bool> predicate)
            where TPredicate : struct, IFunction<TSource, bool>
        {
            return new WhereListEnumerable<TSource, PredicateAnd<TSource, TPredicate, FunctionWrapper<TSource, bool>>>(
                source.Source, 
                new PredicateAnd<TSource, TPredicate, FunctionWrapper<TSource, bool>>(
                    source.Predicate, 
                    new FunctionWrapper<TSource, bool>(predicate)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> Select<TSource, TResult, TPredicate, TSelector>(this WhereListEnumerable<TSource, TPredicate> source, TSelector selector)
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
            => new WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector>(source.Source, source.Predicate, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereSelectListEnumerable<TSource, TResult, TPredicate, FunctionWrapper<TSource, TResult>> Select<TSource, TResult, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source, Func<TSource, TResult> selector)
            where TPredicate : struct, IFunction<TSource, bool>
            => new WhereSelectListEnumerable<TSource, TResult, TPredicate, FunctionWrapper<TSource, TResult>>(source.Source, source.Predicate, new FunctionWrapper<TSource, TResult>(selector));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Sum<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
            where TPredicate : struct, IFunction<TSource, bool>
        {
            var sum = TSource.AdditiveIdentity;
            foreach (var item in source)
            {
                sum += item;
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Min<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TSource : INumber<TSource>
            where TPredicate : struct, IFunction<TSource, bool>
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
        public static TSource Max<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TSource : INumber<TSource>
            where TPredicate : struct, IFunction<TSource, bool>
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
        public static int Count<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunction<TSource, bool>
        {
            var count = 0;
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                    count++;
            }
            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunction<TSource, bool>
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                    return true;
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource First<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunction<TSource, bool>
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                    return span[i];
            }
            throw new InvalidOperationException("Sequence contains no elements");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TSource> FirstOrNone<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunction<TSource, bool>
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                    return Option<TSource>.Some(span[i]);
            }
            return Option<TSource>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Single<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunction<TSource, bool>
        {
            var found = false;
            var result = default(TSource);
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
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
        public static Option<TSource> SingleOrNone<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunction<TSource, bool>
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var found = false;
            var result = default(TSource);
            foreach (var item in span)
            {
                if (predicate.Invoke(item))
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
        public static TSource Last<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunction<TSource, bool>
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var index = span.Length - 1; index >= 0; index--)
            {
                if (predicate.Invoke(span[index]))
                    return span[index];
            }
            throw new InvalidOperationException("Sequence contains no matching element");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TSource> LastOrNone<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunction<TSource, bool>
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var index = span.Length - 1; index >= 0; index--)
            {
                if (predicate.Invoke(span[index]))
                    return Option<TSource>.Some(span[index]);
            }
            return Option<TSource>.None();
        }

        public static TSource[] ToArray<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunction<TSource, bool>
        {
            var list = new List<TSource>();
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                    list.Add(span[i]);
            }
            return list.ToArray();
        }

        public static List<TSource> ToList<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunction<TSource, bool>
        {
            var list = new List<TSource>();
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                    list.Add(span[i]);
            }
            return list;
        }

        public static PooledBuffer<TSource> ToArrayPooled<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source)
            where TPredicate : struct, IFunction<TSource, bool>
            => source.ToArrayPooled((ArrayPool<TSource>?)null);

        public static PooledBuffer<TSource> ToArrayPooled<TSource, TPredicate>(this WhereListEnumerable<TSource, TPredicate> source, ArrayPool<TSource>? pool)
            where TPredicate : struct, IFunction<TSource, bool>
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
                    if (predicate.Invoke(span[i]))
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
