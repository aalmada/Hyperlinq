using System;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public readonly struct PredicateAnd<T, TPre1, TPre2>
        : IFunction<T, bool>
        where TPre1 : struct, IFunction<T, bool>
        where TPre2 : struct, IFunction<T, bool>
    {
        readonly TPre1 first;
        readonly TPre2 second;

        public PredicateAnd(TPre1 first, TPre2 second)
        {
            this.first = first;
            this.second = second;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(T item)
            => first.Invoke(item) && second.Invoke(item);
    }
}
