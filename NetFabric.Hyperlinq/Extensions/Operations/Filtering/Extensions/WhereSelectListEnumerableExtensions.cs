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
        public static WhereSelectListEnumerable<TSource, TResult, PredicateAnd<TSource, TPredicate, SelectorCompose<TSource, TResult, bool, TSelector, FunctionWrapper<TResult, bool>>>, TSelector> Where<TSource, TResult, TPredicate, TSelector>(
            this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source, 
            Func<TResult, bool> predicate)
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            return new WhereSelectListEnumerable<TSource, TResult, PredicateAnd<TSource, TPredicate, SelectorCompose<TSource, TResult, bool, TSelector, FunctionWrapper<TResult, bool>>>, TSelector>(
                source.Source,
                new PredicateAnd<TSource, TPredicate, SelectorCompose<TSource, TResult, bool, TSelector, FunctionWrapper<TResult, bool>>>(
                    source.Predicate,
                    new SelectorCompose<TSource, TResult, bool, TSelector, FunctionWrapper<TResult, bool>>(
                        source.Selector,
                        new FunctionWrapper<TResult, bool>(predicate))),
                source.Selector
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source)
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
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
        public static bool Any<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source)
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
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
        public static TResult First<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source)
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                    return selector.Invoke(span[i]);
            }
            throw new InvalidOperationException("Sequence contains no elements");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> FirstOrNone<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source)
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                    return Option<TResult>.Some(selector.Invoke(span[i]));
            }
            return Option<TResult>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Single<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source)
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            var found = false;
            var result = default(TResult);
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                {
                    if (found)
                        throw new InvalidOperationException("Sequence contains more than one element");
                    result = selector.Invoke(span[i]);
                    found = true;
                }
            }
            if (!found)
                throw new InvalidOperationException("Sequence contains no elements");
            return result!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> SingleOrNone<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source)
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            var found = false;
            var result = default(TResult);
            foreach (var item in span)
            {
                if (predicate.Invoke(item))
                {
                    if (found)
                        throw new InvalidOperationException("Sequence contains more than one matching element");
                    found = true;
                    result = selector.Invoke(item);
                }
            }
            return found ? Option<TResult>.Some(result!) : Option<TResult>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Min<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source)
            where TResult : INumber<TResult>
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
            => source.MinOrNone<TSource, TResult, TPredicate, TSelector>().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> MinOrNone<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source)
            where TResult : INumber<TResult>
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            
            // Find first matching element
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                {
                    var min = selector.Invoke(span[i]);
                    
                    // Process remaining elements
                    for (i++; i < span.Length; i++)
                    {
                        if (predicate.Invoke(span[i]))
                        {
                            var value = selector.Invoke(span[i]);
                            if (value < min)
                                min = value;
                        }
                    }
                    
                    return Option<TResult>.Some(min);
                }
            }
            
            return Option<TResult>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Max<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source)
            where TResult : INumber<TResult>
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
            => source.MaxOrNone<TSource, TResult, TPredicate, TSelector>().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<TResult> MaxOrNone<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source)
            where TResult : INumber<TResult>
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            
            // Find first matching element
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                {
                    var max = selector.Invoke(span[i]);
                    
                    // Process remaining elements
                    for (i++; i < span.Length; i++)
                    {
                        if (predicate.Invoke(span[i]))
                        {
                            var value = selector.Invoke(span[i]);
                            if (value > max)
                                max = value;
                        }
                    }
                    
                    return Option<TResult>.Some(max);
                }
            }
            
            return Option<TResult>.None();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Sum<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source)
            where TResult : IAdditionOperators<TResult, TResult, TResult>, IAdditiveIdentity<TResult, TResult>
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            var sum = TResult.AdditiveIdentity;
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                {
                    sum += selector.Invoke(span[i]);
                }
            }
            
            return sum;
        }

        public static TResult[] ToArray<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source, ArrayPool<TResult>? pool = default)
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            using var builder = new ArrayBuilder<TResult>(pool ?? ArrayPool<TResult>.Shared);
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                    builder.Add(selector.Invoke(span[i]));
            }
            return builder.ToArray();
        }

        public static List<TResult> ToList<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source)
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            var list = new List<TResult>();
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                    list.Add(selector.Invoke(span[i]));
            }
            return list;
        }

        public static PooledBuffer<TResult> ToArrayPooled<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source)
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
            => source.ToArrayPooled((ArrayPool<TResult>?)null);

        public static PooledBuffer<TResult> ToArrayPooled<TSource, TResult, TPredicate, TSelector>(this WhereSelectListEnumerable<TSource, TResult, TPredicate, TSelector> source, ArrayPool<TResult>? pool)
            where TPredicate : struct, IFunction<TSource, bool>
            where TSelector : struct, IFunction<TSource, TResult>
        {
            using var builder = new ArrayBuilder<TResult>(pool ?? ArrayPool<TResult>.Shared);
            var span = CollectionsMarshal.AsSpan(source.Source);
            var predicate = source.Predicate;
            var selector = source.Selector;
            for (var i = 0; i < span.Length; i++)
            {
                if (predicate.Invoke(span[i]))
                    builder.Add(selector.Invoke(span[i]));
            }
            return builder.ToPooledBuffer();
        }


    }
}
