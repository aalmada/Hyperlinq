using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class WhereListRefStructEnumerableExtensions
{
    /// <summary>
    /// Computes the sum of a WhereListRefStructEnumerable for numeric types.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Sum<TSource>(this WhereListRefStructEnumerable<TSource> source)
        where TSource : IAdditionOperators<TSource, TSource, TSource>, IAdditiveIdentity<TSource, TSource>
    {
        var sum = TSource.AdditiveIdentity;
        foreach (var item in source)
        {
            sum += item;
        }
        return sum;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count<TSource>(this WhereListRefStructEnumerable<TSource> source)
    {
        var count = 0;
        var span = CollectionsMarshal.AsSpan(source.Source);
        var predicate = source.Predicate;
        for (var i = 0; (uint)i < (uint)span.Length; i++)
        {
            var result = predicate(span[i]);
            count += Unsafe.As<bool, byte>(ref result);
        }
        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any<TSource>(this WhereListRefStructEnumerable<TSource> source)
    {
        var span = CollectionsMarshal.AsSpan(source.Source);
        var predicate = source.Predicate;
        for (var i = 0; (uint)i < (uint)span.Length; i++)
        {
            if (predicate(span[i]))
            {
                return true;
            }
        }
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource First<TSource>(this WhereListRefStructEnumerable<TSource> source)
        => source.FirstOrNone().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> FirstOrNone<TSource>(this WhereListRefStructEnumerable<TSource> source)
    {
        var span = CollectionsMarshal.AsSpan(source.Source);
        var predicate = source.Predicate;
        for (var i = 0; (uint)i < (uint)span.Length; i++)
        {
            if (predicate(span[i]))
            {
                return Option<TSource>.Some(span[i]);
            }
        }
        return Option<TSource>.None();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Single<TSource>(this WhereListRefStructEnumerable<TSource> source)
        => source.SingleOrNone().Value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> SingleOrNone<TSource>(this WhereListRefStructEnumerable<TSource> source)
    {
        var span = CollectionsMarshal.AsSpan(source.Source);
        var predicate = source.Predicate;
        var found = false;
        var result = default(TSource);
        foreach (var item in span)
        {
            if (predicate(item))
            {
                if (found)
                {
                    throw new InvalidOperationException("Sequence contains more than one matching element");
                }

                found = true;
                result = item;
            }
        }
        return found ? Option<TSource>.Some(result!) : Option<TSource>.None();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Last<TSource>(this WhereListRefStructEnumerable<TSource> source)
    {
        var span = CollectionsMarshal.AsSpan(source.Source);
        var predicate = source.Predicate;
        for (var index = span.Length - 1; index >= 0; index--)
        {
            if (predicate(span[index]))
            {
                return span[index];
            }
        }
        throw new InvalidOperationException("Sequence contains no matching element");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TSource> LastOrNone<TSource>(this WhereListRefStructEnumerable<TSource> source)
    {
        var span = CollectionsMarshal.AsSpan(source.Source);
        var predicate = source.Predicate;
        for (var index = span.Length - 1; index >= 0; index--)
        {
            if (predicate(span[index]))
            {
                return Option<TSource>.Some(span[index]);
            }
        }
        return Option<TSource>.None();
    }

    public static TSource[] ToArray<TSource>(this WhereListRefStructEnumerable<TSource> source, ArrayPool<TSource>? pool = default)
    {
        Unsafe.SkipInit(out SegmentedArrayBuilder<TSource>.ScratchBuffer scratch);
        using var builder = new SegmentedArrayBuilder<TSource>(scratch);
        var span = CollectionsMarshal.AsSpan(source.Source);
        var predicate = source.Predicate;
        foreach (var item in span)
        {
            if (predicate(item))
            {
                builder.Add(item);
            }
        }
        return builder.ToArray();
    }

    public static List<TSource> ToList<TSource>(this WhereListRefStructEnumerable<TSource> source)
    {
        Unsafe.SkipInit(out SegmentedArrayBuilder<TSource>.ScratchBuffer scratch);
        using var builder = new SegmentedArrayBuilder<TSource>(scratch);
        var span = CollectionsMarshal.AsSpan(source.Source);
        var predicate = source.Predicate;
        foreach (var item in span)
        {
            if (predicate(item))
            {
                builder.Add(item);
            }
        }
        return builder.ToList();
    }
}
