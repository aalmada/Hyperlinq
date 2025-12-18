using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class ReadOnlyMemoryExtensions
{
    extension<T>(ReadOnlyMemory<T> source)
    {
        /// <summary>
        /// Returns the only element of a sequence.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single()
            => source.SingleOrNone().Value;

        /// <summary>
        /// Returns the only element that satisfies a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Single(Func<T, bool> predicate)
            => source.SingleOrNone(predicate).Value;

        /// <summary>
        /// Returns the only element, or a default value if empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault()
            => source.SingleOrNone().GetValueOrDefault();

        /// <summary>
        /// Returns the only element, or a specified default value if empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault(T defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);

        /// <summary>
        /// Returns the only element that satisfies a condition, or a default value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault(Func<T, bool> predicate)
            => source.SingleOrNone(predicate).GetValueOrDefault();

        /// <summary>
        /// Returns the only element that satisfies a condition, or a specified default value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T SingleOrDefault(Func<T, bool> predicate, T defaultValue)
            => source.SingleOrNone(predicate).GetValueOrDefault(defaultValue);

        /// <summary>
        /// Returns the only element as an Option.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> SingleOrNone()
            => source.Span.SingleOrNone();

        /// <summary>
        /// Returns the only element that satisfies a condition as an Option.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> SingleOrNone(Func<T, bool> predicate)
            => source.Span.SingleOrNone(predicate);
    }
}
