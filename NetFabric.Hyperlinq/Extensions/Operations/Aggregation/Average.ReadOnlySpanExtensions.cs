using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> source)
        where T : struct, INumberBase<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>, IDivisionOperators<T, T, T>
    {
        /// <summary>
        /// Computes the average of a sequence using SIMD acceleration.
        /// </summary>
        public T Average()
            => source.AverageOrNone().Value;

        /// <summary>
        /// Computes the average of elements that satisfy a condition.
        /// </summary>
        public T Average<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.AverageOrNone(predicate).Value;

        public T Average(Func<T, bool> predicate)
            => source.AverageOrNone(predicate).Value;

        /// <summary>
        /// Computes the average of a sequence, returning None if empty.
        /// </summary>
        public Option<T> AverageOrNone()
        {
            if (source.Length == 0)
            {
                return Option<T>.None();
            }

            var sum = NetFabric.Numerics.Tensors.TensorOperations.Sum<T>(source);
            return Option<T>.Some(sum / T.CreateChecked(source.Length));
        }

        /// <summary>
        /// Computes the average of elements that satisfy a condition, returning None if no matches.
        /// </summary>
        public Option<T> AverageOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => AverageOrNoneImpl(source, predicate);

        public Option<T> AverageOrNone(Func<T, bool> predicate)
            => AverageOrNoneImpl(source, new FunctionWrapper<T, bool>(predicate));
    }

    static Option<T> AverageOrNoneImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where T : struct, INumberBase<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>, IDivisionOperators<T, T, T>
        where TPredicate : struct, IFunction<T, bool>
    {
        var sum = T.AdditiveIdentity;
        var count = 0;

        foreach (var item in source)
        {
            var result = predicate.Invoke(item);
            var mask = Unsafe.As<bool, byte>(ref result);
            count += mask;
            sum += item * T.CreateChecked(mask);
        }

        if (count == 0)
        {
            return Option<T>.None();
        }

        return Option<T>.Some(sum / T.CreateChecked(count));
    }
}
