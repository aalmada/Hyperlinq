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
        var localSpan = source;
        Unsafe.SkipInit(out SegmentedArrayBuilder<T>.ScratchBuffer scratch);
        using var builder = new SegmentedArrayBuilder<T>(scratch);
        var length = localSpan.Length;
        var i = 0;
        
        // Process 4 elements at a time
        for (; i <= length - 4; i += 4)
        {
            if (predicate.Invoke(localSpan[i]))
                builder.Add(localSpan[i]);
            if (predicate.Invoke(localSpan[i + 1]))
                builder.Add(localSpan[i + 1]);
            if (predicate.Invoke(localSpan[i + 2]))
                builder.Add(localSpan[i + 2]);
            if (predicate.Invoke(localSpan[i + 3]))
                builder.Add(localSpan[i + 3]);
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 3:
                if (predicate.Invoke(localSpan[i]))
                    builder.Add(localSpan[i]);
                if (predicate.Invoke(localSpan[i + 1]))
                    builder.Add(localSpan[i + 1]);
                if (predicate.Invoke(localSpan[i + 2]))
                    builder.Add(localSpan[i + 2]);
                break;
            case 2:
                if (predicate.Invoke(localSpan[i]))
                    builder.Add(localSpan[i]);
                if (predicate.Invoke(localSpan[i + 1]))
                    builder.Add(localSpan[i + 1]);
                break;
            case 1:
                if (predicate.Invoke(localSpan[i]))
                    builder.Add(localSpan[i]);
                break;
        }
        
        return builder.ToArray();
    }

    static T[] ToArrayInImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where TPredicate : struct, IFunctionIn<T, bool>
    {
        var localSpan = source;
        Unsafe.SkipInit(out SegmentedArrayBuilder<T>.ScratchBuffer scratch);
        using var builder = new SegmentedArrayBuilder<T>(scratch);
        var length = localSpan.Length;
        var i = 0;
        
        // Process 4 elements at a time
        for (; i <= length - 4; i += 4)
        {
            if (predicate.Invoke(in localSpan[i]))
                builder.Add(localSpan[i]);
            if (predicate.Invoke(in localSpan[i + 1]))
                builder.Add(localSpan[i + 1]);
            if (predicate.Invoke(in localSpan[i + 2]))
                builder.Add(localSpan[i + 2]);
            if (predicate.Invoke(in localSpan[i + 3]))
                builder.Add(localSpan[i + 3]);
        }
        
        // Process remaining elements with switch
        switch (length - i)
        {
            case 3:
                if (predicate.Invoke(in localSpan[i]))
                    builder.Add(localSpan[i]);
                if (predicate.Invoke(in localSpan[i + 1]))
                    builder.Add(localSpan[i + 1]);
                if (predicate.Invoke(in localSpan[i + 2]))
                    builder.Add(localSpan[i + 2]);
                break;
            case 2:
                if (predicate.Invoke(in localSpan[i]))
                    builder.Add(localSpan[i]);
                if (predicate.Invoke(in localSpan[i + 1]))
                    builder.Add(localSpan[i + 1]);
                break;
            case 1:
                if (predicate.Invoke(in localSpan[i]))
                    builder.Add(localSpan[i]);
                break;
        }
        
        return builder.ToArray();
    }
}
