using System;

namespace NetFabric.Hyperlinq.Benchmarks;

public readonly struct DoubleFunction : IFunction<int, int>
{
    public int Invoke(int element) => element * 2;
}

public readonly struct IsEvenFunction : IFunction<int, bool>
{
    public bool Invoke(int element) => element % 2 == 0;
}
