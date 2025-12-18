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
        var count = 0;
        foreach (var item in source)
        {
            var result = predicate.Invoke(item);
            count += Unsafe.As<bool, byte>(ref result);
        }
        return count;
    }
}
