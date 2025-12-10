using System;
using System.Linq;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests
{
    public class ValueEnumerableRepeatTests
    {
        [Test]
        [Arguments(0, 0)]
        [Arguments(42, 1)]
        [Arguments(42, 10)]
        [Arguments(-10, 10)]
        public void Repeat_Should_ReturnExpectedSequence(int element, int count)
        {
            // Arrange
            var expected = Enumerable.Repeat(element, count).ToArray();

            // Act
            var result = ValueEnumerable.Repeat(element, count);

            // Assert
            result.ToArray().Must().BeEqualTo(expected);
        }

        [Test]
        [Arguments("test", 0)]
        [Arguments("test", 1)]
        [Arguments("test", 10)]
        public void Repeat_String_Should_ReturnExpectedSequence(string element, int count)
        {
            // Arrange
            var expected = Enumerable.Repeat(element, count).ToArray();

            // Act
            var result = ValueEnumerable.Repeat(element, count);

            // Assert
            result.ToArray().Must().BeEqualTo(expected);
        }

        [Test]
        [Arguments(0, 0)]
        [Arguments(42, 1)]
        [Arguments(42, 10)]
        public void Repeat_ToArray_Should_ReturnExpectedArray(int element, int count)
        {
            // Arrange
            var expected = Enumerable.Repeat(element, count).ToArray();

            // Act
            var result = ValueEnumerable.Repeat(element, count).ToArray();

            // Assert
            result.Must().BeEqualTo(expected);
        }

        [Test]
        [Arguments("test", 0)]
        [Arguments("test", 1)]
        [Arguments("test", 10)]
        public void Repeat_String_ToArray_Should_ReturnExpectedArray(string element, int count)
        {
            // Arrange
            var expected = Enumerable.Repeat(element, count).ToArray();

            // Act
            var result = ValueEnumerable.Repeat(element, count).ToArray();

            // Assert
            result.Must().BeEqualTo(expected);
        }

        [Test]
        [Arguments(0, 0)]
        [Arguments(42, 1)]
        [Arguments(42, 10)]
        public void Repeat_ToList_Should_ReturnExpectedList(int element, int count)
        {
            // Arrange
            var expected = Enumerable.Repeat(element, count).ToList();

            // Act
            var result = ValueEnumerable.Repeat(element, count).ToList();

            // Assert
            result.SequenceEqual(expected).Must().BeTrue();
        }

        [Test]
        [Arguments("test", 0)]
        [Arguments("test", 1)]
        [Arguments("test", 10)]
        public void Repeat_String_ToList_Should_ReturnExpectedList(string element, int count)
        {
            // Arrange
            var expected = Enumerable.Repeat(element, count).ToList();

            // Act
            var result = ValueEnumerable.Repeat(element, count).ToList();

            // Assert
            result.SequenceEqual(expected).Must().BeTrue();
        }

        [Test]
        [Arguments(0, 0)]
        [Arguments(42, 1)]
        [Arguments(42, 10)]
        public void Repeat_ToArrayPooled_Should_ReturnExpectedBuffer(int element, int count)
        {
            // Arrange
            var expected = Enumerable.Repeat(element, count).ToArray();

            // Act
            using var buffer = ValueEnumerable.Repeat(element, count).ToArrayPooled();

            // Assert
            buffer.Length.Must().BeEqualTo(expected.Length);
            buffer.AsSpan().ToArray().Must().BeEqualTo(expected);
        }

        [Test]
        [Arguments("test", 0)]
        [Arguments("test", 1)]
        [Arguments("test", 10)]
        public void Repeat_String_ToArrayPooled_Should_ReturnExpectedBuffer(string element, int count)
        {
            // Arrange
            var expected = Enumerable.Repeat(element, count).ToArray();

            // Act
            using var buffer = ValueEnumerable.Repeat(element, count).ToArrayPooled();

            // Assert
            buffer.Length.Must().BeEqualTo(expected.Length);
            buffer.AsSpan().ToArray().Must().BeEqualTo(expected);
        }

        [Test]
        public void Repeat_WithNegativeCount_Should_Throw()
        {
            // Arrange
            Action action = () => ValueEnumerable.Repeat(42, -1);

            // Assert
            action.Must().Throw<ArgumentOutOfRangeException>();
        }
    }
}
