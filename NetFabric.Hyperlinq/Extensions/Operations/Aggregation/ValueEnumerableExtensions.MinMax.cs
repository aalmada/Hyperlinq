using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Computes both minimum and maximum values in a single iteration.
        /// </summary>
        public static (T Min, T Max) MinMax<TEnumerable, TEnumerator, T>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
            => source.MinMaxOrNone<TEnumerable, TEnumerator, T>().Value;

        public static (T Min, T Max) MinMax<TEnumerable, TEnumerator, T, TPredicate>(
            this TEnumerable source, 
            TPredicate predicate)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
            where TPredicate : struct, IFunction<T, bool>
            => source.MinMaxOrNone<TEnumerable, TEnumerator, T, TPredicate>(predicate).Value;

        public static (T Min, T Max) MinMax<TEnumerable, TEnumerator, T>(
            this TEnumerable source, 
            Func<T, bool> predicate)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
            => source.MinMaxOrNone<TEnumerable, TEnumerator, T>(predicate).Value;

        /// <summary>
        /// Computes both minimum and maximum values in a single iteration, or None if empty.
        /// </summary>
        public static Option<(T Min, T Max)> MinMaxOrNone<TEnumerable, TEnumerator, T>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
        {
            using var enumerator = source.GetEnumerator();
            
            if (!enumerator.MoveNext())
                return Option<(T Min, T Max)>.None();
            
            var min = enumerator.Current;
            var max = enumerator.Current;
            
            // Process remaining elements with minimal branching
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                // If item < min, it cannot be > max, so use else if
                if (item < min)
                    min = item;
                else if (item > max)
                    max = item;
            }
            
            return Option<(T Min, T Max)>.Some((min, max));
        }

        public static Option<(T Min, T Max)> MinMaxOrNone<TEnumerable, TEnumerator, T, TPredicate>(
            this TEnumerable source, 
            TPredicate predicate)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
            where TPredicate : struct, IFunction<T, bool>
        {
            using var enumerator = source.GetEnumerator();
            
            // Find first matching element
            while (enumerator.MoveNext())
            {
                if (predicate.Invoke(enumerator.Current))
                {
                    var min = enumerator.Current;
                    var max = enumerator.Current;
                    
                    // Process remaining elements with minimal branching
                    while (enumerator.MoveNext())
                    {
                        var item = enumerator.Current;
                        if (predicate.Invoke(item))
                        {
                            // If item < min, it cannot be > max, so use else if
                            if (item < min)
                                min = item;
                            else if (item > max)
                                max = item;
                        }
                    }
                    
                    return Option<(T Min, T Max)>.Some((min, max));
                }
            }
            
            return Option<(T Min, T Max)>.None();
        }

        public static Option<(T Min, T Max)> MinMaxOrNone<TEnumerable, TEnumerator, T>(
            this TEnumerable source, 
            Func<T, bool> predicate)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
            => MinMaxOrNone<TEnumerable, TEnumerator, T, FunctionWrapper<T, bool>>(source, new FunctionWrapper<T, bool>(predicate));
    }
}
