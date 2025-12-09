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
        /// Computes the average of a sequence of numeric values.
        /// Uses TensorPrimitives optimization for arrays and lists.
        /// </summary>
        public static T Average<TEnumerable, TEnumerator, T>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
            => source.AverageOrNone<TEnumerable, TEnumerator, T>().Value;

        /// <summary>
        /// Computes the average of elements that satisfy a condition.
        /// </summary>
        public static T Average<TEnumerable, TEnumerator, T>(
            this TEnumerable source, 
            Func<T, bool> predicate)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
            => source.AverageOrNone<TEnumerable, TEnumerator, T>(predicate).Value;

        /// <summary>
        /// Computes the average of a sequence, returning None if empty.
        /// Uses TensorPrimitives optimization for arrays and lists.
        /// </summary>
        public static Option<T> AverageOrNone<TEnumerable, TEnumerator, T>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
        {
            // Optimize using TensorPrimitives for arrays
            if (source is ArrayValueEnumerable<T> arrayEnum)
            {
                return arrayEnum.Source.AverageOrNone();
            }
            
            // Optimize using TensorPrimitives for lists
            if (source is ListValueEnumerable<T> listEnum)
            {
                var span = CollectionsMarshal.AsSpan(listEnum.Source);
                return span.AverageOrNone();
            }

            // Fallback to standard enumeration
            var sum = T.AdditiveIdentity;
            var count = 0;
            foreach (var item in source)
            {
                sum += item;
                count++;
            }
            
            if (count == 0)
                return Option<T>.None();
            
            return Option<T>.Some(sum / T.CreateChecked(count));
        }

        /// <summary>
        /// Computes the average of elements that satisfy a condition, returning None if no matches.
        /// </summary>
        public static Option<T> AverageOrNone<TEnumerable, TEnumerator, T>(
            this TEnumerable source, 
            Func<T, bool> predicate)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : INumber<T>
        {
            var sum = T.AdditiveIdentity;
            var count = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    sum += item;
                    count++;
                }
            }
            
            if (count == 0)
                return Option<T>.None();
            
            return Option<T>.Some(sum / T.CreateChecked(count));
        }
    }
}
