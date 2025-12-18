using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> source)
    {
        public List<T> ToList<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ToListImpl(source, predicate);

        public List<T> ToList<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => ToListInImpl(source, predicate);

        public List<T> ToList(Func<T, bool> predicate)
            => ToListImpl(source, new FunctionWrapper<T, bool>(predicate));

        /// <summary>
        /// Creates a List from a ReadOnlySpan.
        /// </summary>
        public List<T> ToList()
        {
             var list = new List<T>(source.Length);
             CollectionsMarshal.SetCount(list, source.Length);
             source.CopyTo(CollectionsMarshal.AsSpan(list));
             return list;
        }
    }

    static List<T> ToListImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
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
        return builder.ToList();
    }

    static List<T> ToListInImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
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
        return builder.ToList();
    }
}
