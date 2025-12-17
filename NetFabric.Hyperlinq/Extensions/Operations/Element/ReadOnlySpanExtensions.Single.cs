using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single()
            => source.SingleOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault()
        {
            if (source.Length == 0)
            {
                return default!;
            }

            if (source.Length > 1)
            {
                throw new InvalidOperationException("Sequence contains more than one element");
            }

            return source[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.SingleOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single(Func<T, bool> predicate)
            => source.SingleOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => source.SingleOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault(Func<T, bool> predicate)
            => source.SingleOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
            where TPredicate : struct, IFunction<T, bool>
            => source.SingleOrNone(predicate).GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault(Func<T, bool> predicate, T defaultValue)
            => source.SingleOrNone(predicate).GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> SingleOrNone()
        {
            if (source.Length == 0)
            {
                return Option<T>.None();
            }

            if (source.Length > 1)
            {
                throw new InvalidOperationException("Sequence contains more than one element");
            }

            return Option<T>.Some(source[0]);
        }

        public Option<T> SingleOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => SingleOrNoneImpl(source, predicate);

        public Option<T> SingleOrNone(Func<T, bool> predicate)
            => SingleOrNoneImpl(source, new FunctionWrapper<T, bool>(predicate));
    }

    static Option<T> SingleOrNoneImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where TPredicate : struct, IFunction<T, bool>
    {
        var found = false;
        var result = default(T);
        foreach (var item in source)
        {
            if (predicate.Invoke(item))
            {
                if (found)
                {
                    throw new InvalidOperationException("Sequence contains more than one matching element");
                }

                found = true;
                result = item;
            }
        }
        return found ? Option<T>.Some(result!) : Option<T>.None();
    }
}
