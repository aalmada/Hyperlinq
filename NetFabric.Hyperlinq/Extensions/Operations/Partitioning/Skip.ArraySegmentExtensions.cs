using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArraySegmentExtensions
{
    extension<T>(ArraySegment<T> source)
    {
        /// <summary>
        /// Bypasses a specified number of elements and returns the remaining elements.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArraySegment<T> Skip(int count)
        {
            if (count <= 0)
            {
                return source;
            }

            if (count >= source.Count)
            {
                return new ArraySegment<T>(source.Array!, source.Offset + source.Count, 0);
            }

            return new ArraySegment<T>(source.Array!, source.Offset + count, source.Count - count);
        }
    }
}
