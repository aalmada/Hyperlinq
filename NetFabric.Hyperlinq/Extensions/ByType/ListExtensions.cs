using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ListExtensions
    {
        extension<T>(List<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            /// <summary>
            /// Computes the sum of a sequence of numeric values using SIMD acceleration.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum()
                => CollectionsMarshal.AsSpan(source).Sum();

            /// <summary>
            /// Computes the sum of elements that satisfy a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source).Sum(predicate);
        }

        extension<T>(List<T> source)
            where T : INumber<T>
        {
            /// <summary>
            /// Returns the minimum value in a list using SIMD acceleration.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Min()
                => CollectionsMarshal.AsSpan(source).Min();

            /// <summary>
            /// Returns the minimum value that satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Min(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source).Min(predicate);

            /// <summary>
            /// Returns the maximum value in a list using SIMD acceleration.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Max()
                => CollectionsMarshal.AsSpan(source).Max();

            /// <summary>
            /// Returns the maximum value that satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Max(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source).Max(predicate);

            /// <summary>
            /// Computes the average of a list using SIMD acceleration.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Average()
                => CollectionsMarshal.AsSpan(source).Average();

            /// <summary>
            /// Computes the average of elements that satisfy a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Average(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source).Average(predicate);

            /// <summary>
            /// Computes the average of a list, returning None if empty.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> AverageOrNone()
                => CollectionsMarshal.AsSpan(source).AverageOrNone();

            /// <summary>
            /// Computes the average of elements that satisfy a condition, returning None if no matches.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> AverageOrNone(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source).AverageOrNone(predicate);

            /// <summary>
            /// Computes both minimum and maximum values in a single iteration.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public (T Min, T Max) MinMax()
                => CollectionsMarshal.AsSpan(source).MinMax();

            /// <summary>
            /// Computes both minimum and maximum values for elements that satisfy a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public (T Min, T Max) MinMax(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source).MinMax(predicate);
        }

        extension<T>(List<T> source)
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
                => CollectionsMarshal.AsSpan(source).Any(predicate);

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
                => CollectionsMarshal.AsSpan(source).Count(predicate);

            /// <summary>
            /// Returns the first element of a list.
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
                => CollectionsMarshal.AsSpan(source).FirstOrNone();

            /// <summary>
            /// Returns the first element that satisfies a condition as an Option.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source).FirstOrNone(predicate);

            /// <summary>
            /// Returns the last element of a list.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last()
                => CollectionsMarshal.AsSpan(source).Last();

            /// <summary>
            /// Returns the last element that satisfies a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source).Last(predicate);

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
                => CollectionsMarshal.AsSpan(source).SingleOrNone();

            /// <summary>
            /// Returns the only element that satisfies a condition as an Option.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> SingleOrNone(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source).SingleOrNone(predicate);

            /// <summary>
            /// Bypasses a specified number of elements and returns the remaining elements.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlySpan<T> Skip(int count)
                => CollectionsMarshal.AsSpan(source).Skip(count);

            /// <summary>
            /// Returns a specified number of contiguous elements from the start.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlySpan<T> Take(int count)
                => CollectionsMarshal.AsSpan(source).Take(count);
        }

        /// <summary>
        /// Projects each element into a new form.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectListEnumerable<T, TResult, TSelector> Select<T, TResult, TSelector>(this List<T> source, TSelector selector)
            where TSelector : struct, IFunction<T, TResult>
            => new SelectListEnumerable<T, TResult, TSelector>(source, selector);

        /// <summary>
        /// Projects each element into a new form.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectListEnumerable<T, TResult, FunctionWrapper<T, TResult>> Select<T, TResult>(this List<T> source, Func<T, TResult> selector)
            => new SelectListEnumerable<T, TResult, FunctionWrapper<T, TResult>>(source, new FunctionWrapper<T, TResult>(selector));

        /// <summary>
        /// Projects each element into a new form.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectListInEnumerable<T, TResult, TSelector> Select<T, TResult, TSelector>(this List<T> source, in TSelector selector)
            where TSelector : struct, IFunctionIn<T, TResult>
            => new SelectListInEnumerable<T, TResult, TSelector>(source, selector);

        /// <summary>
        /// Filters elements based on a predicate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereListEnumerable<T, TPredicate> Where<T, TPredicate>(this List<T> source, TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => new WhereListEnumerable<T, TPredicate>(source, predicate);

        /// <summary>
        /// Filters elements based on a predicate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereListEnumerable<T, FunctionWrapper<T, bool>> Where<T>(this List<T> source, Func<T, bool> predicate)
            => new WhereListEnumerable<T, FunctionWrapper<T, bool>>(source, new FunctionWrapper<T, bool>(predicate));

        /// <summary>
        /// Filters elements based on a predicate.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereListInEnumerable<T, TPredicate> Where<T, TPredicate>(this List<T> source, in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => new WhereListInEnumerable<T, TPredicate>(source, predicate);
    }
}
