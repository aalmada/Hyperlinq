using System;

namespace NetFabric.Hyperlinq.UnitTests;

public readonly record struct TestCase<T>(Func<T> Factory, string Description)
{
    public override string ToString() => Description;
}
