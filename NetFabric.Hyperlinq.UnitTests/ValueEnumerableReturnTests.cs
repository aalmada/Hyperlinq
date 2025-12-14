using System;
using System.Linq;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class ValueEnumerableReturnTests
{
    [Test]
    [Arguments(10)]
    [Arguments(42)]
    public void Return_Should_ReturnExpectedSequence(int expected)
    {
        // Act
        var result = ValueEnumerable.Return(expected);

        // Assert
        _ = result.Count.Must().BeEqualTo(1);
        _ = result.ToArray().SequenceEqual([expected]).Must().BeTrue();
        _ = result.Contains(expected).Must().BeTrue();
        _ = result.Contains(expected + 1).Must().BeFalse();
    }

    [Test]
    [Arguments("test")]
    public void Return_String_Should_ReturnExpectedSequence(string expected)
    {
        // Act
        var result = ValueEnumerable.Return(expected);

        // Assert
        _ = result.Count.Must().BeEqualTo(1);
        _ = result.ToArray().SequenceEqual([expected]).Must().BeTrue();
        _ = result.Contains(expected).Must().BeTrue();
        _ = result.Contains(expected + "a").Must().BeFalse();
    }

    [Test]
    [Arguments(10)]
    public void Return_ToArray_Should_ReturnExpectedArray(int expected)
    {
        // Act
        var result = ValueEnumerable.Return(expected).ToArray();

        // Assert
        _ = result.SequenceEqual([expected]).Must().BeTrue();
    }

    [Test]
    [Arguments(10)]
    public void Return_ToList_Should_ReturnExpectedList(int expected)
    {
        // Act
        var result = ValueEnumerable.Return(expected).ToList();

        // Assert
        _ = result.SequenceEqual([expected]).Must().BeTrue();
    }

    [Test]
    [Arguments(10)]
    public void Return_ToArrayPooled_Should_ReturnExpectedBuffer(int expected)
    {
        // Act
        using var buffer = ValueEnumerable.Return(expected).ToArrayPooled();

        // Assert
        _ = buffer.Length.Must().BeEqualTo(1);
        _ = buffer.AsSpan().ToArray().SequenceEqual([expected]).Must().BeTrue();
    }
    [Test]
    [Arguments(10)]
    public void Return_Reset_Should_ResetEnumerator(int expected)
    {
        // Arrange
        using var enumerator = ValueEnumerable.Return(expected).GetEnumerator();
        _ = enumerator.MoveNext().Must().BeTrue();
        _ = enumerator.MoveNext().Must().BeFalse();

        // Act
        enumerator.Reset();

        // Assert
        _ = enumerator.MoveNext().Must().BeTrue();
        _ = enumerator.Current.Must().BeEqualTo(expected);
        _ = enumerator.MoveNext().Must().BeFalse();
    }
}
