using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class MemoryExtensions
    {
        extension<T>(Memory<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            /// <summary>
            /// Computes the sum of a sequence of numeric values using SIMD acceleration.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum()
                => source.Span.Sum();

            /// <summary>
            /// Computes the sum of elements that satisfy a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum(Func<T, bool> predicate)
                => source.Span.Sum(predicate);
        }

        extension<T>(Memory<T> source)
        {
            /// <summary>
            /// Determines whether a sequence contains any elements.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any()
                => source.Length > 0;

            /// <summary>
            /// Determines whether any element satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any(Func<T, bool> predicate)
                => source.Span.Any(predicate);

            /// <summary>
            /// Returns the number of elements in a sequence.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count()
                => source.Length;

            /// <summary>
            /// Returns the number of elements that satisfy a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count(Func<T, bool> predicate)
                => source.Span.Count(predicate);

            /// <summary>
            /// Returns the first element of a memory.
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
                => source.Span.FirstOrNone();

            /// <summary>
            /// Returns the first element that satisfies a condition as an Option.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone(Func<T, bool> predicate)
                => source.Span.FirstOrNone(predicate);

            /// <summary>
            /// Projects each element into a new form.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SelectMemoryEnumerable<T, TResult> Select<TResult>(Func<T, TResult> selector)
                => new SelectMemoryEnumerable<T, TResult>(source, selector);

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
                => source.Span.SingleOrNone();

            /// <summary>
            /// Returns the only element that satisfies a condition as an Option.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> SingleOrNone(Func<T, bool> predicate)
                => source.Span.SingleOrNone(predicate);

            /// <summary>
            /// Filters elements based on a predicate.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public WhereMemoryEnumerable<T> Where(Func<T, bool> predicate)
                => new WhereMemoryEnumerable<T>(source, predicate);

            /// <summary>
            /// Returns the last element of a sequence.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last()
                => source.Span.Last();

            /// <summary>
            /// Returns the last element that satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last(Func<T, bool> predicate)
                => source.Span.Last(predicate);
        }

        extension<T>(ReadOnlyMemory<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            /// <summary>
            /// Computes the sum of a sequence of numeric values using SIMD acceleration.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum()
                => source.Span.Sum();

            /// <summary>
            /// Computes the sum of elements that satisfy a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum(Func<T, bool> predicate)
                => source.Span.Sum(predicate);
        }

        extension<T>(ReadOnlyMemory<T> source)
        {
            /// <summary>
            /// Determines whether a sequence contains any elements.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any()
                => source.Length > 0;

            /// <summary>
            /// Determines whether any element satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any(Func<T, bool> predicate)
                => source.Span.Any(predicate);

            /// <summary>
            /// Returns the number of elements in a sequence.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count()
                => source.Length;

            /// <summary>
            /// Returns the number of elements that satisfy a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count(Func<T, bool> predicate)
                => source.Span.Count(predicate);

            /// <summary>
            /// Returns the first element of a memory.
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
                => source.Span.FirstOrNone();

            /// <summary>
            /// Returns the first element that satisfies a condition as an Option.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone(Func<T, bool> predicate)
                => source.Span.FirstOrNone(predicate);

            /// <summary>
            /// Projects each element into a new form.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SelectMemoryEnumerable<T, TResult> Select<TResult>(Func<T, TResult> selector)
                => new SelectMemoryEnumerable<T, TResult>(source, selector);

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
                => source.Span.SingleOrNone();

            /// <summary>
            /// Returns the only element that satisfies a condition as an Option.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> SingleOrNone(Func<T, bool> predicate)
                => source.Span.SingleOrNone(predicate);

            /// <summary>
            /// Filters elements based on a predicate.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public WhereMemoryEnumerable<T> Where(Func<T, bool> predicate)
                => new WhereMemoryEnumerable<T>(source, predicate);

            /// <summary>
            /// Returns the last element of a sequence.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last()
                => source.Span.Last();

            /// <summary>
            /// Returns the last element that satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last(Func<T, bool> predicate)
                => source.Span.Last(predicate);
        }
    }
}
