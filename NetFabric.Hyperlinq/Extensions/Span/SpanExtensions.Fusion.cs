using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class SpanExtensions
    {
        // ReadOnlySpan<T> Implementation
        
        public static int Count<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
        {
            var count = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                    count++;
            }
            return count;
        }

        public static bool Any<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    return true;
            }
            return false;
        }

        public static T First<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }
            throw new InvalidOperationException("Sequence contains no matching elements");
        }

        public static T FirstOrDefault<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }
            return default!;
        }

        public static T FirstOrDefault<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate, T defaultValue)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    return item;
            }
            return defaultValue;
        }

        public static Option<T> FirstOrNone<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    return Option<T>.Some(item);
            }
            return Option<T>.None();
        }

        public static T Single<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
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
            if (!found)
                throw new InvalidOperationException("Sequence contains no matching elements");
            
            return result!;
        }

        public static T SingleOrDefault<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
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
            return result!;
        }

        public static T SingleOrDefault<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate, T defaultValue)
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
            return found ? result! : defaultValue;
        }

        public static Option<T> SingleOrNone<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
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

        public static T Sum<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            var sum = T.AdditiveIdentity;
            foreach (var item in source)
            {
                if (predicate(item))
                    sum += item;
            }
            return sum;
        }

        // Span<T> Implementation (Delegates to ReadOnlySpan<T>)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this Span<T> source, Func<T, bool> predicate)
            => Count((ReadOnlySpan<T>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this Span<T> source, Func<T, bool> predicate)
            => Any((ReadOnlySpan<T>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this Span<T> source, Func<T, bool> predicate)
            => First((ReadOnlySpan<T>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this Span<T> source, Func<T, bool> predicate)
            => FirstOrDefault((ReadOnlySpan<T>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this Span<T> source, Func<T, bool> predicate, T defaultValue)
            => FirstOrDefault((ReadOnlySpan<T>)source, predicate, defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this Span<T> source, Func<T, bool> predicate)
            => FirstOrNone((ReadOnlySpan<T>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this Span<T> source, Func<T, bool> predicate)
            => Single((ReadOnlySpan<T>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this Span<T> source, Func<T, bool> predicate)
            => SingleOrDefault((ReadOnlySpan<T>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this Span<T> source, Func<T, bool> predicate, T defaultValue)
            => SingleOrDefault((ReadOnlySpan<T>)source, predicate, defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this Span<T> source, Func<T, bool> predicate)
            => SingleOrNone((ReadOnlySpan<T>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this Span<T> source, Func<T, bool> predicate)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            => Sum((ReadOnlySpan<T>)source, predicate);

        // Delegating Overloads (Array, List, Memory) - Delegate to Where

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this T[] source, Func<T, bool> predicate)
            => source.Where(predicate).Count();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this T[] source, Func<T, bool> predicate)
            => source.Where(predicate).Any();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this T[] source, Func<T, bool> predicate)
            => source.Where(predicate).First();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this T[] source, Func<T, bool> predicate)
            => source.Where(predicate).FirstOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this T[] source, Func<T, bool> predicate, T defaultValue)
            => source.Where(predicate).FirstOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this T[] source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.FirstOrNone<WhereMemoryEnumerable<T>, WhereMemoryEnumerable<T>.Enumerator, T>(source.Where(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this T[] source, Func<T, bool> predicate)
            => source.Where(predicate).Single();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this T[] source, Func<T, bool> predicate)
            => source.Where(predicate).SingleOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this T[] source, Func<T, bool> predicate, T defaultValue)
            => source.Where(predicate).SingleOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this T[] source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.SingleOrNone<WhereMemoryEnumerable<T>, WhereMemoryEnumerable<T>.Enumerator, T>(source.Where(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this T[] source, Func<T, bool> predicate)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            => source.Where(predicate).Sum();

        // List<T>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this List<T> source, Func<T, bool> predicate)
            => source.Where(predicate).Count();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this List<T> source, Func<T, bool> predicate)
            => source.Where(predicate).Any();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this List<T> source, Func<T, bool> predicate)
            => source.Where(predicate).First();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this List<T> source, Func<T, bool> predicate)
            => source.Where(predicate).FirstOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this List<T> source, Func<T, bool> predicate, T defaultValue)
            => source.Where(predicate).FirstOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this List<T> source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.FirstOrNone<WhereListEnumerable<T>, WhereListEnumerable<T>.Enumerator, T>(source.Where(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this List<T> source, Func<T, bool> predicate)
            => source.Where(predicate).Single();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this List<T> source, Func<T, bool> predicate)
            => source.Where(predicate).SingleOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this List<T> source, Func<T, bool> predicate, T defaultValue)
            => source.Where(predicate).SingleOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this List<T> source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.SingleOrNone<WhereListEnumerable<T>, WhereListEnumerable<T>.Enumerator, T>(source.Where(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this List<T> source, Func<T, bool> predicate)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            => source.Where(predicate).Sum();

        // Memory<T>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this Memory<T> source, Func<T, bool> predicate)
            => source.Where(predicate).Count();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this Memory<T> source, Func<T, bool> predicate)
            => source.Where(predicate).Any();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this Memory<T> source, Func<T, bool> predicate)
            => source.Where(predicate).First();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this Memory<T> source, Func<T, bool> predicate)
            => source.Where(predicate).FirstOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this Memory<T> source, Func<T, bool> predicate, T defaultValue)
            => source.Where(predicate).FirstOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this Memory<T> source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.FirstOrNone<WhereMemoryEnumerable<T>, WhereMemoryEnumerable<T>.Enumerator, T>(source.Where(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this Memory<T> source, Func<T, bool> predicate)
            => source.Where(predicate).Single();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this Memory<T> source, Func<T, bool> predicate)
            => source.Where(predicate).SingleOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this Memory<T> source, Func<T, bool> predicate, T defaultValue)
            => source.Where(predicate).SingleOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this Memory<T> source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.SingleOrNone<WhereMemoryEnumerable<T>, WhereMemoryEnumerable<T>.Enumerator, T>(source.Where(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this Memory<T> source, Func<T, bool> predicate)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            => source.Where(predicate).Sum();

        // ReadOnlyMemory<T>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            => source.Where(predicate).Count();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            => source.Where(predicate).Any();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            => source.Where(predicate).First();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            => source.Where(predicate).FirstOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate, T defaultValue)
            => source.Where(predicate).FirstOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> FirstOrNone<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.FirstOrNone<WhereMemoryEnumerable<T>, WhereMemoryEnumerable<T>.Enumerator, T>(source.Where(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Single<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            => source.Where(predicate).Single();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            => source.Where(predicate).SingleOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate, T defaultValue)
            => source.Where(predicate).SingleOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            => ValueEnumerableExtensions.SingleOrNone<WhereMemoryEnumerable<T>, WhereMemoryEnumerable<T>.Enumerator, T>(source.Where(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(this ReadOnlyMemory<T> source, Func<T, bool> predicate)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
            => source.Where(predicate).Sum();
    }
}
