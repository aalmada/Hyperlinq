using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class SelectListRefStructEnumerableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
        => source.Source.Count;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
        => source.Source.Count > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult First<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
    {
        var list = source.Source;
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }

        return source.Selector(list[0]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> FirstOrNone<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
    {
        var list = source.Source;
        return list.Count == 0 ? Option<TResult>.None() : Option<TResult>.Some(source.Selector(list[0]));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Single<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
    {
        var list = source.Source;
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }

        if (list.Count > 1)
        {
            throw new InvalidOperationException("Sequence contains more than one element");
        }

        return source.Selector(list[0]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> SingleOrNone<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
    {
        var list = source.Source;
        if (list.Count == 0)
        {
            return Option<TResult>.None();
        }

        if (list.Count > 1)
        {
            throw new InvalidOperationException("Sequence contains more than one element");
        }

        return Option<TResult>.Some(source.Selector(list[0]));
    }

    public static TResult[] ToArray<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
    {
        var list = source.Source;
        var selector = source.Selector;
        var array = GC.AllocateUninitializedArray<TResult>(list.Count);
        var span = CollectionsMarshal.AsSpan(list);
        for (var i = 0; i < span.Length; i++)
        {
            array[i] = selector(span[i]);
        }

        return array;
    }

    public static PooledBuffer<TResult> ToArrayPooled<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source, ArrayPool<TResult>? pool = null)
    {
        var list = source.Source;
        var selector = source.Selector;
        pool ??= ArrayPool<TResult>.Shared;
        var result = pool.Rent(list.Count);
        var span = CollectionsMarshal.AsSpan(list);
        for (var i = 0; i < span.Length; i++)
        {
            result[i] = selector(span[i]);
        }

        return new PooledBuffer<TResult>(result, list.Count, pool);
    }

    public static List<TResult> ToList<TSource, TResult>(this SelectListRefStructEnumerable<TSource, TResult> source)
    {
        var list = source.Source;
        var selector = source.Selector;
        var count = list.Count;
        var result = new List<TResult>(count);
        CollectionsMarshal.SetCount(result, count);
        var destination = CollectionsMarshal.AsSpan(result);
        var sourceSpan = CollectionsMarshal.AsSpan(list);
        for (var i = 0; i < count; i++)
        {
            destination[i] = selector(sourceSpan[i]);
        }

        return result;
    }
}
