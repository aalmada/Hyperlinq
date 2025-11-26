using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WrapperExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this ArrayValueEnumerable<T> source)
        {
            if (source.Count == 0)
                return Option<T>.None();
            if (source.Count > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<T>.Some(source[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this ListValueEnumerable<T> source)
        {
            if (source.Count == 0)
                return Option<T>.None();
            if (source.Count > 1)
                throw new InvalidOperationException("Sequence contains more than one element");
            return Option<T>.Some(source[0]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Option<T> SingleOrNone<T>(this EnumerableValueEnumerable<T> source)
        {
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                return Option<T>.None();
            
            var first = enumerator.Current;
            if (enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains more than one element");

            return Option<T>.Some(first);
        }
    }
}
