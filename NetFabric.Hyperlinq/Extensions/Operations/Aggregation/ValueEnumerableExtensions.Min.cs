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
        /// Returns the minimum value in a sequence of numeric values.
        /// </summary>
        public static T Min<TEnumerable, TEnumerator, T>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
            => source.MinOrNone<TEnumerable, TEnumerator, T>().Value;

        /// <summary>
        /// Returns the minimum value in a sequence of values that satisfy a condition.
        /// </summary>
        public static T Min<TEnumerable, TEnumerator, T>(this TEnumerable source, Func<T, bool> predicate)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
            => source.MinOrNone<TEnumerable, TEnumerator, T>(predicate).Value;

        /// <summary>
        /// Returns the minimum value in a sequence, or None if empty.
        /// </summary>
        public static Option<T> MinOrNone<TEnumerable, TEnumerator, T>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
        {
            using var enumerator = source.GetEnumerator();
            
            if (!enumerator.MoveNext())
                return Option<T>.None();
            
            var min = enumerator.Current;
            
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item < min)
                    min = item;
            }
            
            return Option<T>.Some(min);
        }

        public static Option<T> MinOrNone<TEnumerable, TEnumerator, T>(
            this TEnumerable source, 
            Func<T, bool> predicate)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
        {
            using var enumerator = source.GetEnumerator();
            
            // Find first matching element
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    var min = enumerator.Current;
                    
                    // Process remaining elements without branching on hasValue
                    while (enumerator.MoveNext())
                    {
                        var item = enumerator.Current;
                        if (predicate(item) && item < min)
                            min = item;
                    }
                    
                    return Option<T>.Some(min);
                }
            }
            
            return Option<T>.None();
        }
    }
}
