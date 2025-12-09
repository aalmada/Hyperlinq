using System;
using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ListValueEnumerableExtensions
    {
        extension<T>(ListValueEnumerable<T> source)
            where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum()
                => System.Numerics.Tensors.TensorPrimitives.Sum<T>(CollectionsMarshal.AsSpan(source.Source));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).Sum(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Sum(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).Sum(predicate);
        }

        extension<T>(ListValueEnumerable<T> source)
            where T : INumber<T>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Min()
                => CollectionsMarshal.AsSpan(source.Source).Min();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Min<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).Min(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Min(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).Min(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Max()
                => CollectionsMarshal.AsSpan(source.Source).Max();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Max<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).Max(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Max(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).Max(predicate);
        }

        extension<T>(ListValueEnumerable<T> source)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any()
                => source.Source.Count != 0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).Any(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).Any(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).Count(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Count(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).Count(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T First()
                => CollectionsMarshal.AsSpan(source.Source).First();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T First<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).First(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T First(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).First(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault()
                => CollectionsMarshal.AsSpan(source.Source).FirstOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).FirstOrDefault(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).FirstOrDefault(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).FirstOrDefault(predicate, defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T FirstOrDefault(Func<T, bool> predicate, T defaultValue)
                => CollectionsMarshal.AsSpan(source.Source).FirstOrDefault(predicate, defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone()
                => CollectionsMarshal.AsSpan(source.Source).FirstOrNone();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).FirstOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> FirstOrNone(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).FirstOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Single()
                => CollectionsMarshal.AsSpan(source.Source).Single();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Single<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).Single(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Single(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).Single(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault()
                => CollectionsMarshal.AsSpan(source.Source).SingleOrDefault();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).SingleOrDefault(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).SingleOrDefault(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).SingleOrDefault(predicate, defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T SingleOrDefault(Func<T, bool> predicate, T defaultValue)
                => CollectionsMarshal.AsSpan(source.Source).SingleOrDefault(predicate, defaultValue);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> SingleOrNone()
                => CollectionsMarshal.AsSpan(source.Source).SingleOrNone();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> SingleOrNone<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).SingleOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> SingleOrNone(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).SingleOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last()
                => CollectionsMarshal.AsSpan(source.Source).Last();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).Last(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Last(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).Last(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> LastOrNone()
                => CollectionsMarshal.AsSpan(source.Source).LastOrNone();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> LastOrNone<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).LastOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Option<T> LastOrNone(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).LastOrNone(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T[] ToArray()
                => CollectionsMarshal.AsSpan(source.Source).ToArray();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T[] ToArray<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).ToArray(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T[] ToArray(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).ToArray(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToArrayPooled()
                => CollectionsMarshal.AsSpan(source.Source).ToArrayPooled((ArrayPool<T>?)null);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToArrayPooled(ArrayPool<T>? pool)
                => CollectionsMarshal.AsSpan(source.Source).ToArrayPooled(pool);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToArrayPooled<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).ToArrayPooled(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public PooledBuffer<T> ToArrayPooled<TPredicate>(TPredicate predicate, ArrayPool<T>? pool)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).ToArrayPooled(predicate, pool);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public List<T> ToList()
                => new List<T>(source.Source);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public List<T> ToList<TPredicate>(TPredicate predicate)
                where TPredicate : struct, IFunction<T, bool>
                => CollectionsMarshal.AsSpan(source.Source).ToList(predicate);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public List<T> ToList(Func<T, bool> predicate)
                => CollectionsMarshal.AsSpan(source.Source).ToList(predicate);
        }

        // Direct List extensions returning ref struct enumerables (maximum performance, foreach-only)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectListEnumerable<T, TResult, FunctionWrapper<T, TResult>> Select<T, TResult>(this ListValueEnumerable<T> source, Func<T, TResult> selector)
            => new SelectListEnumerable<T, TResult, FunctionWrapper<T, TResult>>(source.Source, new FunctionWrapper<T, TResult>(selector));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SelectListInEnumerable<T, TResult, TSelector> Select<T, TResult, TSelector>(this ListValueEnumerable<T> source, in TSelector selector)
            where TSelector : struct, IFunctionIn<T, TResult>
            => new SelectListInEnumerable<T, TResult, TSelector>(source.Source, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereListEnumerable<T, FunctionWrapper<T, bool>> Where<T>(this ListValueEnumerable<T> source, Func<T, bool> predicate)
            => new WhereListEnumerable<T, FunctionWrapper<T, bool>>(source.Source, new FunctionWrapper<T, bool>(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static WhereListInEnumerable<T, TPredicate> Where<T, TPredicate>(this ListValueEnumerable<T> source, in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => new WhereListInEnumerable<T, TPredicate>(source.Source, predicate);
    }
}
