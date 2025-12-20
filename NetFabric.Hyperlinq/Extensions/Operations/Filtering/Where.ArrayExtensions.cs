using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArrayExtensions
{
    extension<T>(T[] source)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereArrayEnumerable<T, TPredicate> Where<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => new WhereArrayEnumerable<T, TPredicate>(source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereArrayEnumerable<T, FunctionWrapper<T, bool>> Where(Func<T, bool> predicate)
            => new WhereArrayEnumerable<T, FunctionWrapper<T, bool>>(source, new FunctionWrapper<T, bool>(predicate));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereArrayInEnumerable<T, TPredicate> Where<TPredicate>(in TPredicate predicate)
            where TPredicate : struct, IFunctionIn<T, bool>
            => new WhereArrayInEnumerable<T, TPredicate>(source, predicate);
    }
}
