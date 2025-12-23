using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class MemoryExtensions
{
    extension<T>(Memory<T> source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last()
            => ((ReadOnlyMemory<T>)source).LastOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).LastOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).LastOrNone(predicate).Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T LastOrDefault()
            => ((ReadOnlyMemory<T>)source).LastOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T LastOrDefault<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).LastOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T LastOrDefault(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).LastOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T LastOrDefault(T defaultValue)
            => ((ReadOnlyMemory<T>)source).LastOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T LastOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).LastOrNone(predicate).GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T LastOrDefault(Func<T, bool> predicate, T defaultValue)
            => ((ReadOnlyMemory<T>)source).LastOrNone(predicate).GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> LastOrNone()
            => ((ReadOnlyMemory<T>)source).LastOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> LastOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).LastOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> LastOrNone(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).LastOrNone(predicate);
    }
}
