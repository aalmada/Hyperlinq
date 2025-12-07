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
        /// Returns the maximum value in a sequence of numeric values.
        /// </summary>
        public static T Max<TEnumerable, TEnumerator, T>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
        {
            var hasValue = false;
            var max = default(T);
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

        /// <summary>
        /// Returns the maximum value in a sequence of values that satisfy a condition.
        /// </summary>
        public static T Max<TEnumerable, TEnumerator, T>(this TEnumerable source, Func<T, bool> predicate)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
        {
            var hasValue = false;
            var max = default(T);
            
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    if (!hasValue || item > max!)
                    {
                        max = item;
                        hasValue = true;
                    }
                }
            }
            
            if (!hasValue)
                throw new InvalidOperationException("Sequence contains no matching element");
            
            return max!;
        }
    }
}
