using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> source)
    {
        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        public T Last()
        {
            if (source.Length == 0)
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            return source[^1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => LastImpl(source, predicate);

        public T Last(Func<T, bool> predicate)
            => LastImpl(source, new FunctionWrapper<T, bool>(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> LastOrNone()
            => source.Length == 0 ? Option<T>.None() : Option<T>.Some(source[^1]);

        public Option<T> LastOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => LastOrNoneImpl(source, predicate);

        public Option<T> LastOrNone(Func<T, bool> predicate)
            => LastOrNoneImpl(source, new FunctionWrapper<T, bool>(predicate));
    }

    static Option<T> LastOrNoneImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where TPredicate : struct, IFunction<T, bool>
    {
        for (var index = source.Length - 1; index >= 0; index--)
        {
            if (predicate.Invoke(source[index]))
            {
                return Option<T>.Some(source[index]);
            }
        }
        return Option<T>.None();
    }

    static T LastImpl<T, TPredicate>(ReadOnlySpan<T> source, TPredicate predicate)
        where TPredicate : struct, IFunction<T, bool>
    {
        for (var index = source.Length - 1; index >= 0; index--)
        {
            if (predicate.Invoke(source[index]))
            {
                return source[index];
            }
        }
        throw new InvalidOperationException("Sequence contains no matching element");
    }
}
