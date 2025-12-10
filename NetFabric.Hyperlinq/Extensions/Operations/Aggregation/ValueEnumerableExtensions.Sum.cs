using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using NetFabric.Numerics.Tensors;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        /// <summary>
        /// Computes the sum of a sequence of numeric values.
        /// Uses Tensor optimization for arrays and lists.
        /// </summary>
        public static T Sum<TEnumerable, TEnumerator, T>(this TEnumerable source)
            where TEnumerable : IValueEnumerable<T, TEnumerator>
            where TEnumerator : struct, IEnumerator<T>
            where T : struct, INumberBase<T>
        {
            // Optimize using Tensor for arrays
            if (source is ArrayValueEnumerable<T> arrayEnum)
            {
                return NetFabric.Numerics.Tensors.TensorOperations.Sum<T>(arrayEnum.Source.AsSpan());
            }
            
            // Optimize using Tensor for lists
            if (source is ListValueEnumerable<T> listEnum)
            {
                var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(listEnum.Source);
                return NetFabric.Numerics.Tensors.TensorOperations.Sum<T>(span);
            }

            // Fallback to standard enumeration
            var sum = T.AdditiveIdentity;
            foreach (var item in source)
                sum += item;
            return sum;
        }


        public static TSource Sum<TEnumerable, TEnumerator, TSource, TPredicate>(this TEnumerable source, TPredicate predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TSource : struct, INumberBase<TSource>
            where TPredicate : struct, IFunction<TSource, bool>
        {
            var sum = TSource.AdditiveIdentity;
            foreach (var item in source)
            {
                var result = predicate.Invoke(item);
                var mask = System.Runtime.CompilerServices.Unsafe.As<bool, byte>(ref result);
                sum += item * TSource.CreateChecked(mask);
            }
            return sum;
        }

        public static TSource Sum<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TSource : struct, INumberBase<TSource>
            => Sum<TEnumerable, TEnumerator, TSource, FunctionWrapper<TSource, bool>>(source, new FunctionWrapper<TSource, bool>(predicate));


    }
}
