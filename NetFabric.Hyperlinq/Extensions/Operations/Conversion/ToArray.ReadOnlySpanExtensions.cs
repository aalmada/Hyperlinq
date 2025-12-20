using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> source)
    {
        public T[] ToArray()
        {
            var result = GC.AllocateUninitializedArray<T>(source.Length);
            source.CopyTo(result);
            return result;
        }

        public T[] ToArray<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ToArrayImpl(source, predicate);

        public T[] ToArray<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => ToArrayInImpl(source, predicate);

        public T[] ToArray(Func<T, bool> predicate)
            => ToArrayImpl(source, new FunctionWrapper<T, bool>(predicate));
    }

    static T[] ToArrayImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where TPredicate : struct, IFunction<T, bool>
    {
        Unsafe.SkipInit(out SegmentedArrayBuilder<T>.ScratchBuffer scratch);
        using var builder = new SegmentedArrayBuilder<T>(scratch);
        foreach (var item in source)
        {
            if (predicate.Invoke(item))
            {
                builder.Add(item);
            }
        }
        return builder.ToArray();
    }

    static T[] ToArrayInImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where TPredicate : struct, IFunctionIn<T, bool>
    {
        Unsafe.SkipInit(out SegmentedArrayBuilder<T>.ScratchBuffer scratch);
        using var builder = new SegmentedArrayBuilder<T>(scratch);
        foreach (ref readonly var item in source)
        {
            if (predicate.Invoke(in item))
            {
                builder.Add(item);
            }
        }
        return builder.ToArray();
    }
}
