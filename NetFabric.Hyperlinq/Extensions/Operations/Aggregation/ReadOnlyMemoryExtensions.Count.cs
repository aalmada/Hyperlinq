using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlyMemoryExtensions
{
    extension<T>(ReadOnlyMemory<T> source)
    {
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count()
            => source.Length;

        /// <summary>
        /// Returns the number of elements that satisfy a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Count(Func<T, bool> predicate)
            => source.Span.Count(predicate);
    }
}
