using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ArraySegmentExtensions
{
    extension<T>(ArraySegment<T> source)
    {
        /// <summary>
        /// Returns a specified number of contiguous elements from the start.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArraySegment<T> Take(int count)
        {
            if (count <= 0)
            {
                return new ArraySegment<T>(source.Array!, source.Offset, 0);
            }

            return new ArraySegment<T>(source.Array!, source.Offset, count < source.Count ? count : source.Count);
        }
    }
}
