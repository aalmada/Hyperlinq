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
        var localSpan = source;
        var length = localSpan.Length;
        var i = 0;
        
        // Process 4 elements at a time with early exit
        for (; i <= length - 4; i += 4)
        {
            if (predicate.Invoke(localSpan[i]))
                return true;
            if (predicate.Invoke(localSpan[i + 1]))
                return true;
            if (predicate.Invoke(localSpan[i + 2]))
                return true;
            if (predicate.Invoke(localSpan[i + 3]))
                return true;
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 3:
                if (predicate.Invoke(localSpan[i]))
                    return true;
                if (predicate.Invoke(localSpan[i + 1]))
                    return true;
                if (predicate.Invoke(localSpan[i + 2]))
                    return true;
                break;
            case 2:
                if (predicate.Invoke(localSpan[i]))
                    return true;
                if (predicate.Invoke(localSpan[i + 1]))
                    return true;
                break;
            case 1:
                if (predicate.Invoke(localSpan[i]))
                    return true;
                break;
        }
        
        return false;
    }
}
