using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class WrapperExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this ArrayValueEnumerable<T> source)
            => source.FirstOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this ArrayValueEnumerable<T> source, T defaultValue)
            => source.FirstOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this ListValueEnumerable<T> source)
            => source.FirstOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this ListValueEnumerable<T> source, T defaultValue)
            => source.FirstOrNone().GetValueOrDefault(defaultValue);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this EnumerableValueEnumerable<T> source)
            => source.FirstOrNone().GetValueOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this EnumerableValueEnumerable<T> source, T defaultValue)
            => source.FirstOrNone().GetValueOrDefault(defaultValue);
    }
}
