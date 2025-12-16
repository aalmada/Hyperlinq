using System;
using System.Linq;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class ValueEnumerableEmptyTests
{
    [Test]
    public void Empty_Should_ReturnExpectedSequence()
    {
        // Arrange
        var expected = Enumerable.Empty<int>().ToArray();

        // Act
        var result = ValueEnumerable.Empty<int>();

        // Assert
        _ = result.ToArray().Must().BeEqualTo(expected);
    }

    [Test]
    public void Empty_String_Should_ReturnExpectedSequence()
    {
        // Arrange
        var expected = Enumerable.Empty<string>().ToArray();

        // Act
        var result = ValueEnumerable.Empty<string>();

        // Assert
        _ = result.ToArray().Must().BeEqualTo(expected);
    }

    [Test]
    public void Empty_ToArray_Should_ReturnExpectedArray()
    {
        // Arrange
        var expected = Enumerable.Empty<int>().ToArray();

        // Act
        var result = ValueEnumerable.Empty<int>().ToArray();

        // Assert
        _ = result.Must().BeEqualTo(expected);
    }

    [Test]
    public void Empty_String_ToArray_Should_ReturnExpectedArray()
    {
        // Arrange
        var expected = Enumerable.Empty<string>().ToArray();

        // Act
        var result = ValueEnumerable.Empty<string>().ToArray();

        // Assert
        _ = result.Must().BeEqualTo(expected);
    }

    [Test]
    public void Empty_ToList_Should_ReturnExpectedList()
    {
        // Arrange
        var expected = Enumerable.Empty<int>().ToList();

        // Act
        var result = ValueEnumerable.Empty<int>().ToList();

        // Assert
        _ = result.SequenceEqual(expected).Must().BeTrue();
    }

    [Test]
    public void Empty_String_ToList_Should_ReturnExpectedList()
    {
        // Arrange
        var expected = Enumerable.Empty<string>().ToList();

        // Act
        var result = ValueEnumerable.Empty<string>().ToList();

        // Assert
        _ = result.SequenceEqual(expected).Must().BeTrue();
    }

    [Test]
    public void Empty_ToArrayPooled_Should_ReturnExpectedBuffer()
    {
        // Arrange
        var expected = Enumerable.Empty<int>().ToArray();

        // Act

        // Assert
    }

    [Test]
    public void Empty_String_ToArrayPooled_Should_ReturnExpectedBuffer()
    {
        // Arrange
        var expected = Enumerable.Empty<string>().ToArray();

        // Act

        // Assert
    }
}
