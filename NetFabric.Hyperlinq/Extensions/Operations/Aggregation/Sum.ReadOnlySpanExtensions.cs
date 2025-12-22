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
        var localSpan = source;
        var sum = T.AdditiveIdentity;
        var length = localSpan.Length;
        var i = 0;
        
        // Process 4 elements at a time
        for (; i <= length - 4; i += 4)
        {
            var result0 = predicate.Invoke(localSpan[i]);
            var mask0 = Unsafe.As<bool, byte>(ref result0);
            sum += localSpan[i] * T.CreateChecked(mask0);
            
            var result1 = predicate.Invoke(localSpan[i + 1]);
            var mask1 = Unsafe.As<bool, byte>(ref result1);
            sum += localSpan[i + 1] * T.CreateChecked(mask1);
            
            var result2 = predicate.Invoke(localSpan[i + 2]);
            var mask2 = Unsafe.As<bool, byte>(ref result2);
            sum += localSpan[i + 2] * T.CreateChecked(mask2);
            
            var result3 = predicate.Invoke(localSpan[i + 3]);
            var mask3 = Unsafe.As<bool, byte>(ref result3);
            sum += localSpan[i + 3] * T.CreateChecked(mask3);
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 3:
                var r0 = predicate.Invoke(localSpan[i]);
                var m0 = Unsafe.As<bool, byte>(ref r0);
                sum += localSpan[i] * T.CreateChecked(m0);
                var r1 = predicate.Invoke(localSpan[i + 1]);
                var m1 = Unsafe.As<bool, byte>(ref r1);
                sum += localSpan[i + 1] * T.CreateChecked(m1);
                var r2 = predicate.Invoke(localSpan[i + 2]);
                var m2 = Unsafe.As<bool, byte>(ref r2);
                sum += localSpan[i + 2] * T.CreateChecked(m2);
                break;
            case 2:
                var r3 = predicate.Invoke(localSpan[i]);
                var m3 = Unsafe.As<bool, byte>(ref r3);
                sum += localSpan[i] * T.CreateChecked(m3);
                var r4 = predicate.Invoke(localSpan[i + 1]);
                var m4 = Unsafe.As<bool, byte>(ref r4);
                sum += localSpan[i + 1] * T.CreateChecked(m4);
                break;
            case 1:
                var r5 = predicate.Invoke(localSpan[i]);
                var m5 = Unsafe.As<bool, byte>(ref r5);
                sum += localSpan[i] * T.CreateChecked(m5);
                break;
        }
        
        return sum;
    }

    static T SumInImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where T : INumberBase<T>
        where TPredicate : struct, IFunctionIn<T, bool>
    {
        var localSpan = source;
        var sum = T.AdditiveIdentity;
        var length = localSpan.Length;
        var i = 0;
        
        // Process 4 elements at a time
        for (; i <= length - 4; i += 4)
        {
            var result0 = predicate.Invoke(in localSpan[i]);
            var mask0 = Unsafe.As<bool, byte>(ref result0);
            sum += localSpan[i] * T.CreateChecked(mask0);
            
            var result1 = predicate.Invoke(in localSpan[i + 1]);
            var mask1 = Unsafe.As<bool, byte>(ref result1);
            sum += localSpan[i + 1] * T.CreateChecked(mask1);
            
            var result2 = predicate.Invoke(in localSpan[i + 2]);
            var mask2 = Unsafe.As<bool, byte>(ref result2);
            sum += localSpan[i + 2] * T.CreateChecked(mask2);
            
            var result3 = predicate.Invoke(in localSpan[i + 3]);
            var mask3 = Unsafe.As<bool, byte>(ref result3);
            sum += localSpan[i + 3] * T.CreateChecked(mask3);
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 3:
                var r0 = predicate.Invoke(in localSpan[i]);
                var m0 = Unsafe.As<bool, byte>(ref r0);
                sum += localSpan[i] * T.CreateChecked(m0);
                var r1 = predicate.Invoke(in localSpan[i + 1]);
                var m1 = Unsafe.As<bool, byte>(ref r1);
                sum += localSpan[i + 1] * T.CreateChecked(m1);
                var r2 = predicate.Invoke(in localSpan[i + 2]);
                var m2 = Unsafe.As<bool, byte>(ref r2);
                sum += localSpan[i + 2] * T.CreateChecked(m2);
                break;
            case 2:
                var r3 = predicate.Invoke(in localSpan[i]);
                var m3 = Unsafe.As<bool, byte>(ref r3);
                sum += localSpan[i] * T.CreateChecked(m3);
                var r4 = predicate.Invoke(in localSpan[i + 1]);
                var m4 = Unsafe.As<bool, byte>(ref r4);
                sum += localSpan[i + 1] * T.CreateChecked(m4);
                break;
            case 1:
                var r5 = predicate.Invoke(in localSpan[i]);
                var m5 = Unsafe.As<bool, byte>(ref r5);
                sum += localSpan[i] * T.CreateChecked(m5);
                break;
        }
        
        return sum;
    }
}
