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
        /// Returns the number of elements in a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count()
            => source.Count;

        /// <summary>
        /// Returns the number of elements that satisfy a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).Count(predicate);
    }
}
