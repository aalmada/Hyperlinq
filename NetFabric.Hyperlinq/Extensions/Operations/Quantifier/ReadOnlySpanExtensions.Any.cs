using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any()
            => source.Length > 0;

        public bool Any<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => AnyImpl(source, predicate);

        public bool Any(Func<T, bool> predicate)
            => AnyImpl(source, new FunctionWrapper<T, bool>(predicate));
    }

    static bool AnyImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where TPredicate : struct, IFunction<T, bool>
    {
        foreach (var item in source)
        {
            if (predicate.Invoke(item))
            {
                return true;
            }
        }
        return false;
    }
}
