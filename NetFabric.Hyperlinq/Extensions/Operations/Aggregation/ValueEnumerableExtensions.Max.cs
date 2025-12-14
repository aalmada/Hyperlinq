using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ValueEnumerableExtensions
{
    /// <summary>
    /// Returns the maximum value in a sequence of numeric values.
    /// </summary>
    public static T Max<TEnumerable, TEnumerator, T>(this TEnumerable source)
        where TEnumerable : IValueEnumerable<T, TEnumerator>
        where TEnumerator : struct, IEnumerator<T>
        where T : INumber<T>
        => source.MaxOrNone<TEnumerable, TEnumerator, T>().Value;

    public static T Max<TEnumerable, TEnumerator, T, TPredicate>(this TEnumerable source, TPredicate predicate)
        where TEnumerable : IValueEnumerable<T, TEnumerator>
        where TEnumerator : struct, IEnumerator<T>
        where T : INumber<T>
        where TPredicate : struct, IFunction<T, bool>
        => source.MaxOrNone<TEnumerable, TEnumerator, T, TPredicate>(predicate).Value;

    public static T Max<TEnumerable, TEnumerator, T>(this TEnumerable source, Func<T, bool> predicate)
        where TEnumerable : IValueEnumerable<T, TEnumerator>
        where TEnumerator : struct, IEnumerator<T>
        where T : INumber<T>
        => source.MaxOrNone<TEnumerable, TEnumerator, T>(predicate).Value;

    /// <summary>
    /// Returns the maximum value in a sequence, or None if empty.
    /// </summary>
    public static Option<T> MaxOrNone<TEnumerable, TEnumerator, T>(this TEnumerable source)
        where TEnumerable : IValueEnumerable<T, TEnumerator>
        where TEnumerator : struct, IEnumerator<T>
        where T : INumber<T>
    {
        using var enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            return Option<T>.None();
        }

        var max = enumerator.Current;

        while (enumerator.MoveNext())
        {
            var item = enumerator.Current;
            if (item > max)
            {
                max = item;
            }
        }

        return Option<T>.Some(max);
    }

    public static Option<T> MaxOrNone<TEnumerable, TEnumerator, T, TPredicate>(
        this TEnumerable source,
        TPredicate predicate)
        where TEnumerable : IValueEnumerable<T, TEnumerator>
        where TEnumerator : struct, IEnumerator<T>
        where T : INumber<T>
        where TPredicate : struct, IFunction<T, bool>
    {
        using var enumerator = source.GetEnumerator();

        // Find first matching element
        while (enumerator.MoveNext())
        {
            if (predicate.Invoke(enumerator.Current))
            {
                var max = enumerator.Current;

                // Process remaining elements without branching on hasValue
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current;
                    if (predicate.Invoke(item) && item > max)
                    {
                        max = item;
                    }
                }

                return Option<T>.Some(max);
            }
        }

        return Option<T>.None();
    }

    public static Option<T> MaxOrNone<TEnumerable, TEnumerator, T>(
        this TEnumerable source,
        Func<T, bool> predicate)
        where TEnumerable : IValueEnumerable<T, TEnumerator>
        where TEnumerator : struct, IEnumerator<T>
        where T : INumber<T>
        => MaxOrNone<TEnumerable, TEnumerator, T, FunctionWrapper<T, bool>>(source, new FunctionWrapper<T, bool>(predicate));
}
