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
        /// Determines whether a sequence contains any elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any()
            => CollectionsMarshal.AsSpan(source).Any();

        /// <summary>
        /// Determines whether any element satisfies a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Any(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).Any(predicate);
    }
}
