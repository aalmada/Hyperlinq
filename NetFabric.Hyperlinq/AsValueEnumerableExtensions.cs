using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

/// <summary>
/// Extension methods to convert sources to IValueEnumerable for optimized enumeration.
/// </summary>
public static class AsValueEnumerableExtensions
{
    /// <summary>
    /// Converts a List&lt;T&gt; to a value-type enumerable.
    /// Uses List&lt;T&gt;.Enumerator which is a value type.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ListValueEnumerable<T> AsValueEnumerable<T>(this List<T> source)
        => new ListValueEnumerable<T>(source);

    /// <summary>
    /// Converts an array to a value-type enumerable.
    /// Uses a custom value-type enumerator.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ArrayValueEnumerable<T> AsValueEnumerable<T>(this T[] source)
        => new ArrayValueEnumerable<T>(source);

    /// <summary>
    /// Converts an IEnumerable&lt;T&gt; to a value-type enumerable wrapper.
    /// This is a fallback for types that don't have specific overloads.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EnumerableValueEnumerable<T> AsValueEnumerable<T>(this IEnumerable<T> source)
        => new EnumerableValueEnumerable<T>(source);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueEnumerableWrapper<IEnumerable<TSource>, TEnumerator, TGetEnumerator, TSource> AsValueEnumerable<TSource, TEnumerator, TGetEnumerator>(this IEnumerable<TSource> source, TGetEnumerator getEnumerator)
        where TEnumerator : struct, IEnumerator<TSource>
        where TGetEnumerator : struct, IFunction<IEnumerable<TSource>, TEnumerator>
        => new(source, getEnumerator);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueEnumerableWrapper<IEnumerable<TSource>, TEnumerator, FunctionWrapper<IEnumerable<TSource>, TEnumerator>, TSource> AsValueEnumerable<TSource, TEnumerator>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, TEnumerator> getEnumerator)
        where TEnumerator : struct, IEnumerator<TSource>
        => new(source, new FunctionWrapper<IEnumerable<TSource>, TEnumerator>(getEnumerator));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyCollectionWrapper<ICollection<TSource>, TEnumerator, TGetEnumerator, TSource> AsValueEnumerable<TSource, TEnumerator, TGetEnumerator>(this ICollection<TSource> source, TGetEnumerator getEnumerator)
        where TEnumerator : struct, IEnumerator<TSource>
        where TGetEnumerator : struct, IFunction<ICollection<TSource>, TEnumerator>
        => new(source, getEnumerator);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyCollectionWrapper<ICollection<TSource>, TEnumerator, FunctionWrapper<ICollection<TSource>, TEnumerator>, TSource> AsValueEnumerable<TSource, TEnumerator>(this ICollection<TSource> source, Func<ICollection<TSource>, TEnumerator> getEnumerator)
        where TEnumerator : struct, IEnumerator<TSource>
        => new(source, new FunctionWrapper<ICollection<TSource>, TEnumerator>(getEnumerator));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyListWrapper<IList<TSource>, TEnumerator, TGetEnumerator, TSource> AsValueEnumerable<TSource, TEnumerator, TGetEnumerator>(this IList<TSource> source, TGetEnumerator getEnumerator)
        where TEnumerator : struct, IEnumerator<TSource>
        where TGetEnumerator : struct, IFunction<IList<TSource>, TEnumerator>
        => new(source, getEnumerator);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyListWrapper<IList<TSource>, TEnumerator, FunctionWrapper<IList<TSource>, TEnumerator>, TSource> AsValueEnumerable<TSource, TEnumerator>(this IList<TSource> source, Func<IList<TSource>, TEnumerator> getEnumerator)
        where TEnumerator : struct, IEnumerator<TSource>
        => new(source, new FunctionWrapper<IList<TSource>, TEnumerator>(getEnumerator));


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyCollectionWrapper<HashSet<TSource>, HashSet<TSource>.Enumerator, HashSetGetEnumerator<TSource>, TSource> AsValueEnumerable<TSource>(this HashSet<TSource> source)
        => new(source, new HashSetGetEnumerator<TSource>());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyCollectionWrapper<LinkedList<TSource>, LinkedList<TSource>.Enumerator, LinkedListGetEnumerator<TSource>, TSource> AsValueEnumerable<TSource>(this LinkedList<TSource> source)
        => new(source, new LinkedListGetEnumerator<TSource>());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueEnumerableWrapper<Queue<TSource>, Queue<TSource>.Enumerator, QueueGetEnumerator<TSource>, TSource> AsValueEnumerable<TSource>(this Queue<TSource> source)
        => new(source, new QueueGetEnumerator<TSource>());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueEnumerableWrapper<Stack<TSource>, Stack<TSource>.Enumerator, StackGetEnumerator<TSource>, TSource> AsValueEnumerable<TSource>(this Stack<TSource> source)
        => new(source, new StackGetEnumerator<TSource>());


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyCollectionWrapper<Dictionary<TKey, TValue>, Dictionary<TKey, TValue>.Enumerator, DictionaryGetEnumerator<TKey, TValue>, KeyValuePair<TKey, TValue>> AsValueEnumerable<TKey, TValue>(this Dictionary<TKey, TValue> source)
        where TKey : notnull
        => new(source, new DictionaryGetEnumerator<TKey, TValue>());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyCollectionWrapper<Dictionary<TKey, TValue>.KeyCollection, Dictionary<TKey, TValue>.KeyCollection.Enumerator, DictionaryKeyCollectionGetEnumerator<TKey, TValue>, TKey> AsValueEnumerable<TKey, TValue>(this Dictionary<TKey, TValue>.KeyCollection source)
        where TKey : notnull
        => new(source, new DictionaryKeyCollectionGetEnumerator<TKey, TValue>());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueReadOnlyCollectionWrapper<Dictionary<TKey, TValue>.ValueCollection, Dictionary<TKey, TValue>.ValueCollection.Enumerator, DictionaryValueCollectionGetEnumerator<TKey, TValue>, TValue> AsValueEnumerable<TKey, TValue>(this Dictionary<TKey, TValue>.ValueCollection source)
        where TKey : notnull
        => new(source, new DictionaryValueCollectionGetEnumerator<TKey, TValue>());

    public readonly struct HashSetGetEnumerator<TSource> : IFunction<HashSet<TSource>, HashSet<TSource>.Enumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HashSet<TSource>.Enumerator Invoke(HashSet<TSource> instance) => instance.GetEnumerator();
    }

    public readonly struct LinkedListGetEnumerator<TSource> : IFunction<LinkedList<TSource>, LinkedList<TSource>.Enumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LinkedList<TSource>.Enumerator Invoke(LinkedList<TSource> instance) => instance.GetEnumerator();
    }

    public readonly struct QueueGetEnumerator<TSource> : IFunction<Queue<TSource>, Queue<TSource>.Enumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Queue<TSource>.Enumerator Invoke(Queue<TSource> instance) => instance.GetEnumerator();
    }

    public readonly struct StackGetEnumerator<TSource> : IFunction<Stack<TSource>, Stack<TSource>.Enumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Stack<TSource>.Enumerator Invoke(Stack<TSource> instance) => instance.GetEnumerator();
    }

    public readonly struct DictionaryGetEnumerator<TKey, TValue> : IFunction<Dictionary<TKey, TValue>, Dictionary<TKey, TValue>.Enumerator>
        where TKey : notnull
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Dictionary<TKey, TValue>.Enumerator Invoke(Dictionary<TKey, TValue> instance) => instance.GetEnumerator();
    }

    public readonly struct DictionaryKeyCollectionGetEnumerator<TKey, TValue> : IFunction<Dictionary<TKey, TValue>.KeyCollection, Dictionary<TKey, TValue>.KeyCollection.Enumerator>
        where TKey : notnull
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Dictionary<TKey, TValue>.KeyCollection.Enumerator Invoke(Dictionary<TKey, TValue>.KeyCollection instance) => instance.GetEnumerator();
    }

    public readonly struct DictionaryValueCollectionGetEnumerator<TKey, TValue> : IFunction<Dictionary<TKey, TValue>.ValueCollection, Dictionary<TKey, TValue>.ValueCollection.Enumerator>
        where TKey : notnull
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Dictionary<TKey, TValue>.ValueCollection.Enumerator Invoke(Dictionary<TKey, TValue>.ValueCollection instance) => instance.GetEnumerator();
    }
}
