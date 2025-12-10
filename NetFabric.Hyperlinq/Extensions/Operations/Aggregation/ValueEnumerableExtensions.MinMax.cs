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
        {
            // Optimize for arrays
            if (source is ArrayValueEnumerable<T> arrayEnum)
            {
                return arrayEnum.Source.MinMax();
            }
            
            // Optimize for lists
            if (source is ListValueEnumerable<T> listEnum)
            {
                var span = CollectionsMarshal.AsSpan(listEnum.Source);
                return span.MinMax();
            }

            // Fallback to standard enumeration
            using var enumerator = source.GetEnumerator();
            
            if (!enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements");
            
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
            
            return (min, max);
        }

        /// <summary>
        /// Computes both minimum and maximum values for elements that satisfy a condition.
        /// </summary>
        public static (T Min, T Max) MinMax<TEnumerable, TEnumerator, T>(
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
                    var max = enumerator.Current;
                    
                    // Process remaining elements with minimal branching
                    while (enumerator.MoveNext())
                    {
                        var item = enumerator.Current;
                        if (predicate(item))
                        {
                            // If item < min, it cannot be > max, so use else if
                            if (item < min)
                                min = item;
                            else if (item > max)
                                max = item;
                        }
                    }
                    
                    return (min, max);
                }
            }
            
            throw new InvalidOperationException("Sequence contains no matching element");
        }
    }
}
