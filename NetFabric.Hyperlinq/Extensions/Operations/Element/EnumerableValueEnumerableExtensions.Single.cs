using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class EnumerableValueEnumerableExtensions
{
    extension<T>(EnumerableValueEnumerable<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single(Func<T, bool> predicate)
            => source.SingleOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault(Func<T, bool> predicate)
            => source.SingleOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault(Func<T, bool> predicate, T defaultValue)
            => source.SingleOrNone(predicate).GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> SingleOrNone()
        {
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return Option<T>.None();
            }

            var first = enumerator.Current;
            if (enumerator.MoveNext())
            {
                throw new InvalidOperationException("Sequence contains more than one element");
            }

            return Option<T>.Some(first);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> SingleOrNone(Func<T, bool> predicate)
        {
            var found = false;
            var result = default(T);
            foreach (var item in source)
            {
                if (predicate(item))
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
}
