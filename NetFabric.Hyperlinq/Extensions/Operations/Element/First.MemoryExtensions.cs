using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

public static partial class MemoryExtensions
{
    extension<T>(Memory<T> source)
    {
        /// <summary>
        /// Returns the first element of a memory.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First()
            => ((ReadOnlyMemory<T>)source).FirstOrNone().Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).FirstOrNone(predicate).Value;

        /// <summary>
        /// Returns the first element that satisfies a condition.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T First(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).FirstOrNone(predicate).Value;

        /// <summary>
        /// Returns the first element, or a default value if empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault()
            => ((ReadOnlyMemory<T>)source).FirstOrNone().GetValueOrDefault();

        /// <summary>
        /// Returns the first element, or a specified default value if empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault(T defaultValue)
            => ((ReadOnlyMemory<T>)source).FirstOrNone().GetValueOrDefault(defaultValue);

        /// <summary>
        /// Returns the first element that satisfies a condition, or a default value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).FirstOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).FirstOrNone(predicate).GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault<TPredicate>(TPredicate predicate, T defaultValue)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).FirstOrNone(predicate).GetValueOrDefault(defaultValue);

        /// <summary>
        /// Returns the first element that satisfies a condition, or a specified default value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T FirstOrDefault(Func<T, bool> predicate, T defaultValue)
            => ((ReadOnlyMemory<T>)source).FirstOrNone(predicate).GetValueOrDefault(defaultValue);

        /// <summary>
        /// Returns the first element as an Option.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> FirstOrNone()
            => ((ReadOnlyMemory<T>)source).FirstOrNone();

        /// <summary>
        /// Returns the first element that satisfies a condition as an Option.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> FirstOrNone(Func<T, bool> predicate)
            => ((ReadOnlyMemory<T>)source).FirstOrNone(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> FirstOrNone<TPredicate>(TPredicate predicate)
            where TPredicate : struct, IFunction<T, bool>
            => ((ReadOnlyMemory<T>)source).FirstOrNone(predicate);
    }
}
