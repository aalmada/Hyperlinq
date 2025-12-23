using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class MemoryExtensions
{
    extension<T>(Memory<T> source)
        where T : struct, INumberBase<T>, IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>, IDivisionOperators<T, T, T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Average()
            => ((ReadOnlyMemory<T>)source).Average();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Average<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).Average(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Average(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).Average(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> AverageOrNone()
            => ((ReadOnlyMemory<T>)source).AverageOrNone();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> AverageOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).AverageOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> AverageOrNone(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).AverageOrNone(predicate);
    }
}
