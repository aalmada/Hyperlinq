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
            var hasValue = false;
            var min = default(T);
            var max = default(T);
            
            foreach (var item in source)
            {
                if (!hasValue)
                {
                    min = item;
                    max = item;
                    hasValue = true;
                }
                else
                {
                    if (item < min!)
                        min = item;
                    if (item > max!)
                        max = item;
                }
            }
            
            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no elements");
            
            return (min!, max!);
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
            var hasValue = false;
            var min = default(T);
            var max = default(T);
            
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    if (!hasValue)
                    {
                        min = item;
                        max = item;
                        hasValue = true;
                    }
                    else
                    {
                        if (item < min!)
                            min = item;
                        if (item > max!)
                            max = item;
                    }
                }
            }
            
            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no matching element");
            
            return (min!, max!);
        }
    }
}
