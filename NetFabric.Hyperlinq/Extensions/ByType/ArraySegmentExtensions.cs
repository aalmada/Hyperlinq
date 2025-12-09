using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ArraySegmentExtensions
    {
        extension<T>(ArraySegment<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            /// <summary>
            /// Computes the sum of a sequence of numeric values using SIMD acceleration.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum()
                => source.AsSpan().Sum();

            /// <summary>
            /// Computes the sum of elements that satisfy a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum(Func<T, bool> predicate)
                => source.AsSpan().Sum(predicate);
        }

        extension<T>(ArraySegment<T> source)
            where T : INumber<T>
        {
            /// <summary>
            /// Returns the minimum value in an array segment using SIMD acceleration.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Min()
                => source.AsSpan().Min();

            /// <summary>
            /// Returns the minimum value that satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Min(Func<T, bool> predicate)
                => source.AsSpan().Min(predicate);

            /// <summary>
            /// Returns the maximum value in an array segment using SIMD acceleration.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Max()
                => source.AsSpan().Max();

            /// <summary>
            /// Returns the maximum value that satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Max(Func<T, bool> predicate)
                => source.AsSpan().Max(predicate);

            /// <summary>
            /// Computes the average of an array segment using SIMD acceleration.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Average()
                => source.AsSpan().Average();

            /// <summary>
            /// Computes the average of elements that satisfy a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Average(Func<T, bool> predicate)
                => source.AsSpan().Average(predicate);

            /// <summary>
            /// Computes the average of an array segment, returning None if empty.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> AverageOrNone()
                => source.AsSpan().AverageOrNone();

            /// <summary>
            /// Computes the average of elements that satisfy a condition, returning None if no matches.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> AverageOrNone(Func<T, bool> predicate)
                => source.AsSpan().AverageOrNone(predicate);
        }

        extension<T>(ArraySegment<T> source)
        {
            /// <summary>
            /// Determines whether a sequence contains any elements.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any()
                => source.Count > 0;

            /// <summary>
            /// Determines whether any element satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any(Func<T, bool> predicate)
                => source.AsSpan().Any(predicate);

            /// <summary>
            /// Returns the number of elements in a sequence.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count()
                => source.Count;

            /// <summary>
            /// Returns the number of elements that satisfy a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count(Func<T, bool> predicate)
                => source.AsSpan().Count(predicate);

            /// <summary>
            /// Returns the first element of an array segment.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T First()
                => source.FirstOrNone().Value;

            /// <summary>
            /// Returns the first element that satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T First(Func<T, bool> predicate)
                => source.FirstOrNone(predicate).Value;

            /// <summary>
            /// Returns the first element, or a default value if empty.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault()
                => source.FirstOrNone().GetValueOrDefault();

            /// <summary>
            /// Returns the first element, or a specified default value if empty.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(T defaultValue)
                => source.FirstOrNone().GetValueOrDefault(defaultValue);

            /// <summary>
            /// Returns the first element that satisfies a condition, or a default value.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(Func<T, bool> predicate)
                => source.FirstOrNone(predicate).GetValueOrDefault();

            /// <summary>
            /// Returns the first element that satisfies a condition, or a specified default value.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(Func<T, bool> predicate, T defaultValue)
                => source.FirstOrNone(predicate).GetValueOrDefault(defaultValue);

            /// <summary>
            /// Returns the first element as an Option.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone()
                => source.AsSpan().FirstOrNone();

            /// <summary>
            /// Returns the first element that satisfies a condition as an Option.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone(Func<T, bool> predicate)
                => source.AsSpan().FirstOrNone(predicate);

            /// <summary>
            /// Returns the last element of an array segment.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last()
                => source.AsSpan().Last();

            /// <summary>
            /// Returns the last element that satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last(Func<T, bool> predicate)
                => source.AsSpan().Last(predicate);

            /// <summary>
            /// Projects each element into a new form.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SelectReadOnlySpanEnumerable<T, TResult, FunctionWrapper<T, TResult>> Select<TResult>(Func<T, TResult> selector)
                => new SelectReadOnlySpanEnumerable<T, TResult, FunctionWrapper<T, TResult>>(source.AsSpan(), new FunctionWrapper<T, TResult>(selector));

            /// <summary>
            /// Projects each element into a new form using a value delegate passed by reference.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SelectReadOnlySpanInEnumerable<T, TResult, TSelector> Select<TResult, TSelector>(in TSelector selector)
                where TSelector : struct, IFunctionIn<T, TResult>
                => new SelectReadOnlySpanInEnumerable<T, TResult, TSelector>(source.AsSpan(), selector);

            /// <summary>
            /// Returns the only element of a sequence.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Single()
                => source.SingleOrNone().Value;

            /// <summary>
            /// Returns the only element that satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Single(Func<T, bool> predicate)
                => source.SingleOrNone(predicate).Value;

            /// <summary>
            /// Returns the only element, or a default value if empty.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault()
                => source.SingleOrNone().GetValueOrDefault();

            /// <summary>
            /// Returns the only element, or a specified default value if empty.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault(T defaultValue)
                => source.SingleOrNone().GetValueOrDefault(defaultValue);

            /// <summary>
            /// Returns the only element that satisfies a condition, or a default value.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault(Func<T, bool> predicate)
                => source.SingleOrNone(predicate).GetValueOrDefault();

            /// <summary>
            /// Returns the only element that satisfies a condition, or a specified default value.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault(Func<T, bool> predicate, T defaultValue)
                => source.SingleOrNone(predicate).GetValueOrDefault(defaultValue);

            /// <summary>
            /// Returns the only element as an Option.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> SingleOrNone()
                => source.AsSpan().SingleOrNone();

            /// <summary>
            /// Returns the only element that satisfies a condition as an Option.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> SingleOrNone(Func<T, bool> predicate)
                => source.AsSpan().SingleOrNone(predicate);

            /// <summary>
            /// Bypasses a specified number of elements and returns the remaining elements.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ArraySegment<T> Skip(int count)
            {
                if (count <= 0) return source;
                if (count >= source.Count) return new ArraySegment<T>(source.Array!, source.Offset + source.Count, 0);
                return new ArraySegment<T>(source.Array!, source.Offset + count, source.Count - count);
            }

            /// <summary>
            /// Returns a specified number of contiguous elements from the start.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ArraySegment<T> Take(int count)
            {
                if (count <= 0) return new ArraySegment<T>(source.Array!, source.Offset, 0);
                return new ArraySegment<T>(source.Array!, source.Offset, count < source.Count ? count : source.Count);
            }

            /// <summary>
            /// Filters elements based on a predicate.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>> Where(Func<T, bool> predicate)
                => new WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>>(source.AsSpan(), new FunctionWrapper<T, bool>(predicate));

            /// <summary>
            /// Filters elements based on a predicate using a value delegate passed by reference.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public WhereReadOnlySpanInEnumerable<T, TPredicate> Where<TPredicate>(in TPredicate predicate)
                where TPredicate : struct, IFunctionIn<T, bool>
                => new WhereReadOnlySpanInEnumerable<T, TPredicate>(source.AsSpan(), predicate);
        }
    }
}
