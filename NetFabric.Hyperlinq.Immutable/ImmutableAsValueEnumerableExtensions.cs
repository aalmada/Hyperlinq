using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

/// <summary>
/// Extension methods for System.Collections.Immutable types.
/// </summary>
public static class ImmutableAsValueEnumerableExtensions
{
    // ImmutableArray<T> -> ValueReadOnlyListWrapper
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyListWrapper<ImmutableArray<TSource>, ImmutableArrayEnumeratorWrapper<TSource>, ImmutableArrayGetEnumerator<TSource>, TSource> AsValueEnumerable<TSource>(this ImmutableArray<TSource> source)
        => new(source, new ImmutableArrayGetEnumerator<TSource>());

    // ImmutableList<T> -> ValueReadOnlyListWrapper
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyListWrapper<ImmutableList<TSource>, ImmutableListEnumeratorWrapper<TSource>, ImmutableListGetEnumerator<TSource>, TSource> AsValueEnumerable<TSource>(this ImmutableList<TSource> source)
        => new(source, new ImmutableListGetEnumerator<TSource>());

    // ImmutableQueue<T> -> ValueEnumerableWrapper
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueEnumerableWrapper<ImmutableQueue<TSource>, ImmutableQueueEnumeratorWrapper<TSource>, ImmutableQueueGetEnumerator<TSource>, TSource> AsValueEnumerable<TSource>(this ImmutableQueue<TSource> source)
        => new(source, new ImmutableQueueGetEnumerator<TSource>());

    // ImmutableStack<T> -> ValueEnumerableWrapper
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueEnumerableWrapper<ImmutableStack<TSource>, ImmutableStackEnumeratorWrapper<TSource>, ImmutableStackGetEnumerator<TSource>, TSource> AsValueEnumerable<TSource>(this ImmutableStack<TSource> source)
        => new(source, new ImmutableStackGetEnumerator<TSource>());

    // ImmutableHashSet<T> -> ValueReadOnlyCollectionWrapper
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyCollectionWrapper<ImmutableHashSet<TSource>, ImmutableHashSetEnumeratorWrapper<TSource>, ImmutableHashSetGetEnumerator<TSource>, TSource> AsValueEnumerable<TSource>(this ImmutableHashSet<TSource> source)
        => new(source, new ImmutableHashSetGetEnumerator<TSource>());

    // ImmutableSortedSet<T> -> ValueReadOnlyCollectionWrapper
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyCollectionWrapper<ImmutableSortedSet<TSource>, ImmutableSortedSetEnumeratorWrapper<TSource>, ImmutableSortedSetGetEnumerator<TSource>, TSource> AsValueEnumerable<TSource>(this ImmutableSortedSet<TSource> source)
        => new(source, new ImmutableSortedSetGetEnumerator<TSource>());

    // ImmutableDictionary<TKey, TValue> -> ValueReadOnlyCollectionWrapper
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyCollectionWrapper<ImmutableDictionary<TKey, TValue>, ImmutableDictionaryEnumeratorWrapper<TKey, TValue>, ImmutableDictionaryGetEnumerator<TKey, TValue>, KeyValuePair<TKey, TValue>> AsValueEnumerable<TKey, TValue>(this ImmutableDictionary<TKey, TValue> source)
        where TKey : notnull
        => new(source, new ImmutableDictionaryGetEnumerator<TKey, TValue>());

    // ImmutableSortedDictionary<TKey, TValue> -> ValueReadOnlyCollectionWrapper
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyCollectionWrapper<ImmutableSortedDictionary<TKey, TValue>, ImmutableSortedDictionaryEnumeratorWrapper<TKey, TValue>, ImmutableSortedDictionaryGetEnumerator<TKey, TValue>, KeyValuePair<TKey, TValue>> AsValueEnumerable<TKey, TValue>(this ImmutableSortedDictionary<TKey, TValue> source)
        where TKey : notnull
        => new(source, new ImmutableSortedDictionaryGetEnumerator<TKey, TValue>());

    public readonly struct ImmutableArrayGetEnumerator<TSource> : IFunction<ImmutableArray<TSource>, ImmutableArrayEnumeratorWrapper<TSource>>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImmutableArrayEnumeratorWrapper<TSource> Invoke(ImmutableArray<TSource> instance) => new(instance.GetEnumerator());
    }

    public readonly struct ImmutableListGetEnumerator<TSource> : IFunction<ImmutableList<TSource>, ImmutableListEnumeratorWrapper<TSource>>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImmutableListEnumeratorWrapper<TSource> Invoke(ImmutableList<TSource> instance) => new(instance.GetEnumerator());
    }

    public readonly struct ImmutableQueueGetEnumerator<TSource> : IFunction<ImmutableQueue<TSource>, ImmutableQueueEnumeratorWrapper<TSource>>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImmutableQueueEnumeratorWrapper<TSource> Invoke(ImmutableQueue<TSource> instance) => new(instance.GetEnumerator());
    }

    public readonly struct ImmutableStackGetEnumerator<TSource> : IFunction<ImmutableStack<TSource>, ImmutableStackEnumeratorWrapper<TSource>>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImmutableStackEnumeratorWrapper<TSource> Invoke(ImmutableStack<TSource> instance) => new(instance.GetEnumerator());
    }

    public readonly struct ImmutableHashSetGetEnumerator<TSource> : IFunction<ImmutableHashSet<TSource>, ImmutableHashSetEnumeratorWrapper<TSource>>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImmutableHashSetEnumeratorWrapper<TSource> Invoke(ImmutableHashSet<TSource> instance) => new(instance.GetEnumerator());
    }

    public readonly struct ImmutableSortedSetGetEnumerator<TSource> : IFunction<ImmutableSortedSet<TSource>, ImmutableSortedSetEnumeratorWrapper<TSource>>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImmutableSortedSetEnumeratorWrapper<TSource> Invoke(ImmutableSortedSet<TSource> instance) => new(instance.GetEnumerator());
    }

    public readonly struct ImmutableDictionaryGetEnumerator<TKey, TValue> : IFunction<ImmutableDictionary<TKey, TValue>, ImmutableDictionaryEnumeratorWrapper<TKey, TValue>>
        where TKey : notnull
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImmutableDictionaryEnumeratorWrapper<TKey, TValue> Invoke(ImmutableDictionary<TKey, TValue> instance) => new(instance.GetEnumerator());
    }

    public readonly struct ImmutableSortedDictionaryGetEnumerator<TKey, TValue> : IFunction<ImmutableSortedDictionary<TKey, TValue>, ImmutableSortedDictionaryEnumeratorWrapper<TKey, TValue>>
        where TKey : notnull
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImmutableSortedDictionaryEnumeratorWrapper<TKey, TValue> Invoke(ImmutableSortedDictionary<TKey, TValue> instance) => new(instance.GetEnumerator());
    }
}
