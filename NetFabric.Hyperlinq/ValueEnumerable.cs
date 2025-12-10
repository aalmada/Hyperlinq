using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeEnumerable Range(int start, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            var end = (long)start + count - 1;
            if (end > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(count));

            return new RangeEnumerable(start, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EmptyEnumerable<T> Empty<T>()
            => new EmptyEnumerable<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RepeatEnumerable<T> Repeat<T>(T element, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return new RepeatEnumerable<T>(element, count);
        }
    }
}
