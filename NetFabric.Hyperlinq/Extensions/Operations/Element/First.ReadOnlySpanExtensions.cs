using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First()
            => source.FirstOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.FirstOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First(Func<T, bool> predicate)
            => source.FirstOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault()
            => source.FirstOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.FirstOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault(Func<T, bool> predicate)
            => source.FirstOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
            where TPredicate : struct, IFunction<T, bool>
            => source.FirstOrNone(predicate).GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault(Func<T, bool> predicate, T defaultValue)
            => source.FirstOrNone(predicate).GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> FirstOrNone()
            => source.Length == 0 ? Option<T>.None() : Option<T>.Some(source[0]);

        public Option<T> FirstOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => FirstOrNoneImpl(source, predicate);

        public Option<T> FirstOrNone(Func<T, bool> predicate)
            => FirstOrNoneImpl(source, new FunctionWrapper<T, bool>(predicate));
    }

    static Option<T> FirstOrNoneImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where TPredicate : struct, IFunction<T, bool>
    {
        foreach (var item in source)
        {
            if (predicate.Invoke(item))
            {
                return Option<T>.Some(item);
            }
        }
        return Option<T>.None();
    }
}
