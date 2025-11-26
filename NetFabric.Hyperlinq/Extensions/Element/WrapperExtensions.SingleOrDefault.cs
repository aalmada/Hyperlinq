using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WrapperExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ArrayValueEnumerable<T> source)
            => source.SingleOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ArrayValueEnumerable<T> source, T defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ListValueEnumerable<T> source)
            => source.SingleOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this ListValueEnumerable<T> source, T defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this EnumerableValueEnumerable<T> source)
            => source.SingleOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SingleOrDefault<T>(this EnumerableValueEnumerable<T> source, T defaultValue)
            => source.SingleOrNone().GetValueOrDefault(defaultValue);
    }
}
