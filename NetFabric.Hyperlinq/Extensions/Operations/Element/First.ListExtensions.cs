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
        /// Returns the first element of a list.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First()
            => source.FirstOrNone().Value;

        /// <summary>
        /// Returns the first element that satisfies a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First(Func<T, bool> predicate)
            => source.FirstOrNone(predicate).Value;

        /// <summary>
        /// Returns the first element, or a default value if empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault()
            => source.FirstOrNone().GetValueOrDefault();

        /// <summary>
        /// Returns the first element, or a specified default value if empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault(T defaultValue)
            => source.FirstOrNone().GetValueOrDefault(defaultValue);

        /// <summary>
        /// Returns the first element that satisfies a condition, or a default value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault(Func<T, bool> predicate)
            => source.FirstOrNone(predicate).GetValueOrDefault();

        /// <summary>
        /// Returns the first element that satisfies a condition, or a specified default value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault(Func<T, bool> predicate, T defaultValue)
            => source.FirstOrNone(predicate).GetValueOrDefault(defaultValue);

        /// <summary>
        /// Returns the first element as an Option.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> FirstOrNone()
            => CollectionsMarshal.AsSpan(source).FirstOrNone();

        /// <summary>
        /// Returns the first element that satisfies a condition as an Option.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> FirstOrNone(Func<T, bool> predicate)
            => CollectionsMarshal.AsSpan(source).FirstOrNone(predicate);
    }
}
