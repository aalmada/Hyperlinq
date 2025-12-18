using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFabric.Hyperlinq;

public static partial class ListExtensions
{
    extension<T>(List<T> source)
    {
        /// <summary>
        /// Returns the last element of a list.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last()
            => CollectionsMarshal.AsSpan(source).Last();

        /// <summary>
        /// Returns the last element that satisfies a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Last(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).Last(predicate);
    }
}
