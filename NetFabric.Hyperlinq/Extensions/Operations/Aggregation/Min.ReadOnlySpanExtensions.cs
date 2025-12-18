using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> source)
        where T : struct, INumber<T>, IMinMaxValue<T>
    {
        public T Min()
            => source.MinOrNone().Value;

        public T Min<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.MinOrNone(predicate).Value;

        public T Min(Func<T, bool> predicate)
            => source.MinOrNone(predicate).Value;

        /// <summary>
        /// Returns the minimum value in a sequence, or None if empty.
        /// </summary>
        public Option<T> MinOrNone()
        {
            if (source.Length == 0)
            {
                return Option<T>.None();
            }

            return Option<T>.Some(NetFabric.Numerics.Tensors.TensorOperations.Min<T>(source));
        }

        public Option<T> MinOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => MinOrNoneImpl(source, predicate);

        public Option<T> MinOrNone(Func<T, bool> predicate)
            => MinOrNoneImpl(source, new FunctionWrapper<T, bool>(predicate));

        public T Max()
            => source.MaxOrNone().Value;

        public T Max<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.MaxOrNone(predicate).Value;

        public T Max(Func<T, bool> predicate)
            => source.MaxOrNone(predicate).Value;

        /// <summary>
        /// Returns the maximum value in a sequence, or None if empty.
        /// </summary>
        public Option<T> MaxOrNone()
        {
            if (source.Length == 0)
            {
                return Option<T>.None();
            }

            return Option<T>.Some(NetFabric.Numerics.Tensors.TensorOperations.Max<T>(source));
        }

        public Option<T> MaxOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => MaxOrNoneImpl(source, predicate);

        public Option<T> MaxOrNone(Func<T, bool> predicate)
            => MaxOrNoneImpl(source, new FunctionWrapper<T, bool>(predicate));

        /// <summary>
        /// Computes both minimum and maximum values in a single iteration.
        /// </summary>
        public (T Min, T Max) MinMax()
            => source.MinMaxOrNone().Value;

        public (T Min, T Max) MinMax<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.MinMaxOrNone(predicate).Value;

        public (T Min, T Max) MinMax(Func<T, bool> predicate)
            => source.MinMaxOrNone(predicate).Value;

        /// <summary>
        /// Computes both minimum and maximum values in a single iteration, or None if empty.
        /// </summary>
        public Option<(T Min, T Max)> MinMaxOrNone()
        {
            if (source.Length == 0)
            {
                return Option<(T Min, T Max)>.None();
            }

            var result = NetFabric.Numerics.Tensors.TensorOperations.MinMax<T>(source);
            return Option<(T Min, T Max)>.Some((result.Min, result.Max));
        }

        public Option<(T Min, T Max)> MinMaxOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => MinMaxOrNoneImpl(source, predicate);

        public Option<(T Min, T Max)> MinMaxOrNone(Func<T, bool> predicate)
            => MinMaxOrNoneImpl(source, new FunctionWrapper<T, bool>(predicate));
    }

    static Option<(T Min, T Max)> MinMaxOrNoneImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where T : struct, INumber<T>
        where TPredicate : struct, IFunction<T, bool>
    {
        // Find first matching element
        var index = 0;
        while ((uint)index < (uint)source.Length && !predicate.Invoke(source[index]))
        {
            index++;
        }

        if (index >= source.Length)
        {
            return Option<(T Min, T Max)>.None();
        }

        var min = source[index];
        var max = source[index];
        index++;

        // Process remaining elements with minimal branching
        while ((uint)index < (uint)source.Length)
        {
            var item = source[index];
            if (predicate.Invoke(item))
            {
                // If item < min, it cannot be > max, so use else if
                if (item < min)
                {
                    min = item;
                }
                else if (item > max)
                {
                    max = item;
                }
            }
            index++;
        }

        return Option<(T Min, T Max)>.Some((min, max));
    }


    static Option<T> MinOrNoneImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where T : struct, INumber<T>
        where TPredicate : struct, IFunction<T, bool>
    {
        // Find first matching element
        var index = 0;
        while ((uint)index < (uint)source.Length && !predicate.Invoke(source[index]))
        {
            index++;
        }

        if (index >= source.Length)
        {
            return Option<T>.None();
        }

        var min = source[index];
        index++;

        // Process remaining elements without branching on hasValue
        while ((uint)index < (uint)source.Length)
        {
            var item = source[index];
            if (predicate.Invoke(item) && item < min)
            {
                min = item;
            }

            index++;
        }

        return Option<T>.Some(min);
    }

    static Option<T> MaxOrNoneImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where T : struct, INumber<T>
        where TPredicate : struct, IFunction<T, bool>
    {
        // Find first matching element
        var index = 0;
        while ((uint)index < (uint)source.Length && !predicate.Invoke(source[index]))
        {
            index++;
        }

        if (index >= source.Length)
        {
            return Option<T>.None();
        }

        var max = source[index];
        index++;

        // Process remaining elements without branching on hasValue
        while ((uint)index < (uint)source.Length)
        {
            var item = source[index];
            if (predicate.Invoke(item) && item > max)
            {
                max = item;
            }

            index++;
        }

        return Option<T>.Some(max);
    }
}
