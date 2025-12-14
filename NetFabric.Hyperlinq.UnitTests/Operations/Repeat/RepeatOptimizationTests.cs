using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Repeat;

public class RepeatOptimizationTests
{
    [Test]
    public void Repeat_Array_Should_ReturnExpectedSequence()
    {
        var source = new[] { 1, 2, 3 };
        var expected = Enumerable.Repeat(source, 2).SelectMany(x => x).ToArray();

        var result = source.Repeat(2);

        _ = result.ToArray().Must().BeEqualTo(expected);
        _ = result.Count.Must().BeEqualTo(6);
        _ = result[0].Must().BeEqualTo(1);
        _ = result[3].Must().BeEqualTo(1);
    }

    [Test]
    public void Repeat_List_Should_ReturnExpectedSequence()
    {
        var source = new List<int> { 1, 2, 3 };
        var expected = Enumerable.Repeat(source, 2).SelectMany(x => x).ToArray();

        var result = source.Repeat(2);

        _ = result.ToArray().Must().BeEqualTo(expected);
        _ = result.Count.Must().BeEqualTo(6);
        _ = result[0].Must().BeEqualTo(1);
        _ = result[3].Must().BeEqualTo(1);
        _ = ((IList<int>)result).IndexOf(1).Must().BeEqualTo(0);
        _ = ((IList<int>)result).IndexOf(3).Must().BeEqualTo(2);

        var zeroCountResult = source.Repeat(0);
        _ = zeroCountResult.Count.Must().BeEqualTo(0);
        _ = ((IList<int>)zeroCountResult).IndexOf(1).Must().BeEqualTo(-1);
    }

    [Test]
    public void Repeat_List_CopyTo_Should_Work_With_Offsets()
    {
        var source = new List<int> { 1, 2, 3 };
        var repeatCount = 2;
        var result = source.Repeat(repeatCount);

        var array = new int[10];
        var arrayIndex = 2;

        ((ICollection<int>)result).CopyTo(array, arrayIndex);

        _ = array[0].Must().BeEqualTo(default);
        _ = array[1].Must().BeEqualTo(default);
        _ = array[2].Must().BeEqualTo(1);
        _ = array[3].Must().BeEqualTo(2);
        _ = array[4].Must().BeEqualTo(3);
        _ = array[5].Must().BeEqualTo(1);
        _ = array[6].Must().BeEqualTo(2);
        _ = array[7].Must().BeEqualTo(3);
        _ = array[8].Must().BeEqualTo(default);
        _ = array[9].Must().BeEqualTo(default);
    }

    [Test]
    public void Repeat_ReadOnlySpan_Should_ReturnExpectedSequence()
    {
        var source = (ReadOnlySpan<int>)new[] { 1, 2, 3 };
        var expected = Enumerable.Repeat(source.ToArray(), 2).SelectMany(x => x).ToArray();

        var result = source.Repeat(2);

        _ = result.ToArray().Must().BeEqualTo(expected);
        _ = result.Count.Must().BeEqualTo(6);
        _ = result[0].Must().BeEqualTo(1);
        _ = result[3].Must().BeEqualTo(1);
    }

    [Test]
    public void Repeat_Range_Should_ReturnExpectedSequence_And_UseCollectionOptimization()
    {
        var source = ValueEnumerable.Range(1, 3); // 1, 2, 3
        var expected = Enumerable.Repeat(Enumerable.Range(1, 3), 2).SelectMany(x => x).ToArray();

        var result = source.Repeat(2);

        _ = result.ToArray().Must().BeEqualTo(expected);
        _ = result.Count.Must().BeEqualTo(6);
    }
}
