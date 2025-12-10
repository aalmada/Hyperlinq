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
        /// Uses Tensor optimization for arrays and lists.
        /// </summary>
        public static T Average<TEnumerable, TEnumerator, T>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : struct, INumberBase<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>, IDivisionOperators<T, T, T>
            => source.AverageOrNone<TEnumerable, TEnumerator, T>().Value;

        /// <summary>
        /// Computes the average of elements that satisfy a condition.
        /// </summary>
        public static T Average<TEnumerable, TEnumerator, T, TPredicate>(
            this TEnumerable source, 
            TPredicate predicate)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : struct, INumberBase<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>, IDivisionOperators<T, T, T>
            where TPredicate : struct, IFunction<T, bool>
            => source.AverageOrNone<TEnumerable, TEnumerator, T, TPredicate>(predicate).Value;

        /// <summary>
        /// Computes the average of elements that satisfy a condition.
        /// </summary>
        public static T Average<TEnumerable, TEnumerator, T>(
            this TEnumerable source, 
            Func<T, bool> predicate)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : struct, INumberBase<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>, IDivisionOperators<T, T, T>
            => source.AverageOrNone<TEnumerable, TEnumerator, T>(predicate).Value;

        /// <summary>
        /// Computes the average of a sequence, returning None if empty.
        /// Uses Tensor optimization for arrays and lists.
        /// </summary>
        public static Option<T> AverageOrNone<TEnumerable, TEnumerator, T>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : struct, INumberBase<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>, IDivisionOperators<T, T, T>
        {
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
        public static Option<T> AverageOrNone<TEnumerable, TEnumerator, T, TPredicate>(
            this TEnumerable source, 
            TPredicate predicate)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : struct, INumberBase<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>, IDivisionOperators<T, T, T>
            where TPredicate : struct, IFunction<T, bool>
        {
            var sum = T.AdditiveIdentity;
            var count = 0;
            foreach (var item in source)
            {
                var result = predicate.Invoke(item);
                var mask = System.Runtime.CompilerServices.Unsafe.As<bool, byte>(ref result);
                count += mask;
                sum += item * T.CreateChecked(mask);
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
            where T : struct, INumberBase<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>, IDivisionOperators<T, T, T>
            => AverageOrNone<TEnumerable, TEnumerator, T, FunctionWrapper<T, bool>>(source, new FunctionWrapper<T, bool>(predicate));
    }
}
