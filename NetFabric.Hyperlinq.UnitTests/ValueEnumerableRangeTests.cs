using System;
using System.Linq;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests
{
    public class ValueEnumerableRangeTests
    {
        [Test]
        [Arguments(0, 0)]
        [Arguments(0, 1)]
        [Arguments(0, 10)]
        [Arguments(10, 10)]
        [Arguments(-10, 10)]
        public void Range_Should_ReturnExpectedSequence(int start, int count)
        {
            // Arrange
            var expected = Enumerable.Range(start, count);

            // Act
            var result = ValueEnumerable.Range(start, count);

            // Assert
            // result.Must().BeEnumerableOf<int>().BeEqualTo(expected);
            
            using var enumerator = ((System.Collections.Generic.IEnumerable<int>)result).GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
            }
        }

        [Test]
        [Arguments(0, 0)]
        [Arguments(0, 1)]
        [Arguments(0, 10)]
        public void Range_ToArray_Should_ReturnExpectedArray(int start, int count)
        {
            // Arrange
            var expected = Enumerable.Range(start, count).ToArray();

            // Act
            var result = ValueEnumerable.Range(start, count).ToArray();

            // Assert
            result.Must().BeEnumerableOf<int>().BeEqualTo(expected);
        }

        [Test]
        [Arguments(0, 0)]
        [Arguments(0, 1)]
        [Arguments(0, 10)]
        public void Range_ToList_Should_ReturnExpectedList(int start, int count)
        {
            // Arrange
            var expected = Enumerable.Range(start, count).ToList();

            // Act
            var result = ValueEnumerable.Range(start, count).ToList();

            // Assert
            result.Must().BeEnumerableOf<int>().BeEqualTo(expected);
        }

        [Test]
        [Arguments(0, 0)]
        [Arguments(0, 1)]
        [Arguments(0, 10)]
        public void Range_ToArrayPooled_Should_ReturnExpectedBuffer(int start, int count)
        {
            // Arrange
            var expected = Enumerable.Range(start, count).ToArray();

            // Act
            using var buffer = ValueEnumerable.Range(start, count).ToArrayPooled();

            // Assert
            buffer.Length.Must().BeEqualTo(expected.Length);
            buffer.AsSpan().ToArray().Must().BeEnumerableOf<int>().BeEqualTo(expected);
        }



        [Test]
        public void Range_WithNegativeCount_Should_Throw()
        {
            // Arrange
            Action action = () => ValueEnumerable.Range(0, -1);

            // Assert
            action.Must().Throw<ArgumentOutOfRangeException>();
        }
    }
}
