using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count()
            => source.Length;

        public int Count<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => CountImpl(source, predicate);

        public int Count(Func<T, bool> predicate)
            => CountImpl(source, new FunctionWrapper<T, bool>(predicate));
    }

    static int CountImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where TPredicate : struct, IFunction<T, bool>
    {
        var localSpan = source;
        var count = 0;
        var length = localSpan.Length;
        var i = 0;
        
        // Process 4 elements at a time
        for (; i <= length - 4; i += 4)
        {
            var result0 = predicate.Invoke(localSpan[i]);
            count += Unsafe.As<bool, byte>(ref result0);
            var result1 = predicate.Invoke(localSpan[i + 1]);
            count += Unsafe.As<bool, byte>(ref result1);
            var result2 = predicate.Invoke(localSpan[i + 2]);
            count += Unsafe.As<bool, byte>(ref result2);
            var result3 = predicate.Invoke(localSpan[i + 3]);
            count += Unsafe.As<bool, byte>(ref result3);
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 3:
                var r0 = predicate.Invoke(localSpan[i]);
                count += Unsafe.As<bool, byte>(ref r0);
                var r1 = predicate.Invoke(localSpan[i + 1]);
                count += Unsafe.As<bool, byte>(ref r1);
                var r2 = predicate.Invoke(localSpan[i + 2]);
                count += Unsafe.As<bool, byte>(ref r2);
                break;
            case 2:
                var r3 = predicate.Invoke(localSpan[i]);
                count += Unsafe.As<bool, byte>(ref r3);
                var r4 = predicate.Invoke(localSpan[i + 1]);
                count += Unsafe.As<bool, byte>(ref r4);
                break;
            case 1:
                var r5 = predicate.Invoke(localSpan[i]);
                count += Unsafe.As<bool, byte>(ref r5);
                break;
        }
        
        return count;
    }
}
