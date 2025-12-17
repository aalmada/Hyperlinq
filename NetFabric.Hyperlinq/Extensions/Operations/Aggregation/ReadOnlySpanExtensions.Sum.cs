using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlySpanExtensions
{
    // Constrained extension block - only for Sum operations
    extension<T>(ReadOnlySpan<T> source)
        where T : struct, INumberBase<T>
    {
        public T Sum()
            => NetFabric.Numerics.Tensors.TensorOperations.Sum<T>(source);

        // Primary overload - value delegate
        public T Sum<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => SumImpl(source, predicate);

        // Overload for IFunctionIn - passed by reference
        public T Sum<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => SumInImpl(source, predicate);

        // Secondary overload - Func wrapper for backward compatibility
        public T Sum(Func<T, bool> predicate)
            => SumImpl(source, new FunctionWrapper<T, bool>(predicate));
    }

    static T SumImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where T : INumberBase<T>
        where TPredicate : struct, IFunction<T, bool>
    {
        var sum = T.AdditiveIdentity;
        foreach (var item in source)
        {
            var result = predicate.Invoke(item);
            var mask = Unsafe.As<bool, byte>(ref result);
            sum += item * T.CreateChecked(mask);
        }
        return sum;
    }

    static T SumInImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where T : INumberBase<T>
        where TPredicate : struct, IFunctionIn<T, bool>
    {
        var sum = T.AdditiveIdentity;
        foreach (var item in source)
        {
            var result = predicate.Invoke(in item);
            var mask = Unsafe.As<bool, byte>(ref result);
            sum += item * T.CreateChecked(mask);
        }
        return sum;
    }
}
