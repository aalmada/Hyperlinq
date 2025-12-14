using System;
using System.Linq;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Repeat;

public class RepeatTests
{
    [Test]
    public void Repeat_Infinite_Should_ReturnInfiniteSequence()
    {
        var source = ValueEnumerable.Range(0, 3);
        var expected = Enumerable.Repeat(Enumerable.Range(0, 3), 4).SelectMany(x => x).ToArray();

        var result = source.Repeat().Take(12);

        _ = result.ToArray().Must().BeEqualTo(expected);
    }

    [Test]
    public void Repeat_Finite_Should_ReturnRepeatedSequence()
    {
        var source = ValueEnumerable.Range(0, 3);
        var expected = Enumerable.Repeat(Enumerable.Range(0, 3), 2).SelectMany(x => x).ToArray();

        var result = source.Repeat(2);

        _ = result.ToArray().Must().BeEqualTo(expected);
    }

    [Test]
    public void Repeat_WithZeroCount_Should_ReturnEmpty()
    {
        var source = ValueEnumerable.Range(0, 3);

        var result = source.Repeat(0);

        _ = result.Count().Must().BeEqualTo(0);
    }

    [Test]
    public void Repeat_WithEmptySource_Should_ReturnEmpty()
    {
        var source = ValueEnumerable.Empty<int>();
        var result = source.Repeat(5);
        _ = result.Count().Must().BeEqualTo(0);
    }

    [Test]
    public void Repeat_WithSingleElement_Should_Work()
    {
        var source = ValueEnumerable.Return(1);
        var result = source.Repeat(3);
        _ = result.ToArray().Must().BeEqualTo(new[] { 1, 1, 1 });
    }
}
