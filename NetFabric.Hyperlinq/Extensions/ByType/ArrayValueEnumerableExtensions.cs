using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

using NetFabric.Numerics.Tensors;

namespace NetFabric.Hyperlinq
{
    public static partial class ArrayValueEnumerableExtensions
    {

        extension<T>(ArrayValueEnumerable<T> source)
            where T : struct, INumberBase<T>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum()
                => NetFabric.Numerics.Tensors.TensorOperations.Sum<T>(source.Source.AsSpan());

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.Sum(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum(Func<T, bool> predicate)
                => source.Source.Sum(predicate);
        }

        extension<T>(ArrayValueEnumerable<T> source)
            where T : struct, INumber<T>, IMinMaxValue<T>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Min()
                => source.Source.Min();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Min<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.Min(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Min(Func<T, bool> predicate)
                => source.Source.Min(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Max()
                => source.Source.Max();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Max<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.Max(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Max(Func<T, bool> predicate)
                => source.Source.Max(predicate);



            /// <summary>
            /// Computes both minimum and maximum values in a single iteration.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public (T Min, T Max) MinMax()
                => source.Source.MinMax();

            /// <summary>
            /// Computes both minimum and maximum values for elements that satisfy a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public (T Min, T Max) MinMax(Func<T, bool> predicate)
                => source.Source.MinMax(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> MinOrNone()
                => source.Source.MinOrNone();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> MinOrNone(Func<T, bool> predicate)
                => source.Source.MinOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> MaxOrNone()
                => source.Source.MaxOrNone();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> MaxOrNone(Func<T, bool> predicate)
                => source.Source.MaxOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<(T Min, T Max)> MinMaxOrNone()
                => source.Source.MinMaxOrNone();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<(T Min, T Max)> MinMaxOrNone(Func<T, bool> predicate)
                => source.Source.MinMaxOrNone(predicate);
        }

        extension<T>(ArrayValueEnumerable<T> source)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any()
                => source.Source.Length != 0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.Any(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any(Func<T, bool> predicate)
                => source.Source.Any(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.Count(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count(Func<T, bool> predicate)
                => source.Source.Count(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T First()
                => source.Source.First();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T First<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.First(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T First(Func<T, bool> predicate)
                => source.Source.First(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault()
                => source.Source.FirstOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.FirstOrDefault(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(Func<T, bool> predicate)
                => source.Source.FirstOrDefault(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.FirstOrDefault(predicate, defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(Func<T, bool> predicate, T defaultValue)
                => source.Source.FirstOrDefault(predicate, defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone()
                => source.Source.FirstOrNone();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.FirstOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone(Func<T, bool> predicate)
                => source.Source.FirstOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Single()
                => source.Source.Single();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Single<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.Single(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Single(Func<T, bool> predicate)
                => source.Source.Single(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault()
                => source.Source.SingleOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.SingleOrDefault(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault(Func<T, bool> predicate)
                => source.Source.SingleOrDefault(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.SingleOrDefault(predicate, defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault(Func<T, bool> predicate, T defaultValue)
                => source.Source.SingleOrDefault(predicate, defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> SingleOrNone()
                => source.Source.SingleOrNone();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> SingleOrNone<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.SingleOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> SingleOrNone(Func<T, bool> predicate)
                => source.Source.SingleOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last()
                => source.Source.Last();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.Last(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last(Func<T, bool> predicate)
                => source.Source.Last(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> LastOrNone()
                => source.Source.LastOrNone();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> LastOrNone<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.LastOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> LastOrNone(Func<T, bool> predicate)
                => source.Source.LastOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T[] ToArray()
                => source.Source.ToArray();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T[] ToArray<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.ToArray(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T[] ToArray(Func<T, bool> predicate)
                => source.Source.ToArray(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToArrayPooled()
                => source.Source.ToArrayPooled((ArrayPool<T>?)null);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToArrayPooled(ArrayPool<T>? pool)
                => source.Source.ToArrayPooled(pool);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToArrayPooled<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.ToArrayPooled(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToArrayPooled<TPredicate>(TPredicate predicate, ArrayPool<T>? pool)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.ToArrayPooled(predicate, pool);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public List<T> ToList()
                => new List<T>(source.Source);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public List<T> ToList<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => source.Source.ToList(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public List<T> ToList(Func<T, bool> predicate)
                => source.Source.ToList(predicate);

            /// <summary>
            /// Bypasses a specified number of elements and returns the remaining elements.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlySpan<T> Skip(int count)
                => source.Source.AsSpan().Skip(count);

            /// <summary>
            /// Returns a specified number of contiguous elements from the start.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlySpan<T> Take(int count)
                => source.Source.AsSpan().Take(count);
        }

        // Direct array extensions returning ref struct enumerables (maximum performance, foreach-only)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectArrayEnumerable<T, TResult, TSelector> Select<T, TResult, TSelector>(this ArrayValueEnumerable<T> source, TSelector selector)
            where TSelector : struct, IFunction<T, TResult>
            => new SelectArrayEnumerable<T, TResult, TSelector>(source.Source, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectArrayEnumerable<T, TResult, FunctionWrapper<T, TResult>> Select<T, TResult>(this ArrayValueEnumerable<T> source, Func<T, TResult> selector)
            => new SelectArrayEnumerable<T, TResult, FunctionWrapper<T, TResult>>(source.Source, new FunctionWrapper<T, TResult>(selector));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectArrayInEnumerable<T, TResult, TSelector> Select<T, TResult, TSelector>(this ArrayValueEnumerable<T> source, in TSelector selector)
            where TSelector : struct, IFunctionIn<T, TResult>
            => new SelectArrayInEnumerable<T, TResult, TSelector>(source.Source, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereReadOnlySpanEnumerable<T, TPredicate> Where<T, TPredicate>(this ArrayValueEnumerable<T> source, TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => new WhereReadOnlySpanEnumerable<T, TPredicate>(source.Source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereReadOnlySpanInEnumerable<T, TPredicate> Where<T, TPredicate>(this ArrayValueEnumerable<T> source, in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => new WhereReadOnlySpanInEnumerable<T, TPredicate>(source.Source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>> Where<T>(this ArrayValueEnumerable<T> source, Func<T, bool> predicate)
            => new WhereReadOnlySpanEnumerable<T, FunctionWrapper<T, bool>>(source.Source, new FunctionWrapper<T, bool>(predicate));

        extension<T>(ArrayValueEnumerable<T> source)
            where T : struct, INumberBase<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>, IDivisionOperators<T, T, T>
        {
            /// <summary>
            /// Computes the average of an array using SIMD acceleration.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Average()
                => source.Source.Average();

            /// <summary>
            /// Computes the average of elements that satisfy a condition.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Average(Func<T, bool> predicate)
                => source.Source.Average(predicate);

            /// <summary>
            /// Computes the average of an array, returning None if empty.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> AverageOrNone()
                => source.Source.AverageOrNone();

            /// <summary>
            /// Computes the average of elements that satisfy a condition, returning None if no matches.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> AverageOrNone(Func<T, bool> predicate)
                => source.Source.AverageOrNone(predicate);
        }
    }
}
