using System;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests
{
    public class ValueDelegateMemoryTests
    {
        // Test IFunctionIn for ReadOnlyMemory<T>.Select
        [Test]
        public void ReadOnlyMemory_Select_WithIFunctionIn_ShouldTransformElements()
        {
            // Arrange
            ReadOnlyMemory<int> memory = new[] { 1, 2, 3, 4, 5 }.AsMemory();
            var selector = new DoubleSelector();
            
            // Act
            var result = memory.Select<int, DoubleSelector>(in selector).ToArray();
            
            // Assert
            result.Must().BeEqualTo(new[] { 2, 4, 6, 8, 10 });
        }

        // Test IFunctionIn for ReadOnlyMemory<T>.Where
        [Test]
        public void ReadOnlyMemory_Where_WithIFunctionIn_ShouldFilterElements()
        {
            // Arrange
            ReadOnlyMemory<int> memory = new[] { 1, 2, 3, 4, 5, 6 }.AsMemory();
            var predicate = new IsEvenPredicate();
            
            // Act
            var result = memory.Where<IsEvenPredicate>(in predicate).ToArray();
            
            // Assert
            result.Must().BeEqualTo(new[] { 2, 4, 6 });
        }

        // Test IFunctionIn for ArraySegment<T>.Select
        [Test]
        public void ArraySegment_Select_WithIFunctionIn_ShouldTransformElements()
        {
            // Arrange
            var segment = new ArraySegment<int>(new[] { 10, 20, 30, 40 });
            var selector = new HalfSelector();
            
            // Act
            var result = segment.Select<int, HalfSelector>(in selector).ToArray();
            
            // Assert
            result.Must().BeEqualTo(new[] { 5, 10, 15, 20 });
        }

        // Test IFunctionIn for ArraySegment<T>.Where
        [Test]
        public void ArraySegment_Where_WithIFunctionIn_ShouldFilterElements()
        {
            // Arrange
            var segment = new ArraySegment<int>(new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            var predicate = new IsOddPredicate();
            
            // Act
            var result = segment.Where<IsOddPredicate>(in predicate).ToArray();
            
            // Assert
            result.Must().BeEqualTo(new[] { 1, 3, 5, 7 });
        }

        // Test chaining with IFunctionIn
        [Test]
        public void ReadOnlyMemory_Where_Select_WithIFunctionIn_ShouldChain()
        {
            // Arrange
            ReadOnlyMemory<int> memory = new[] { 1, 2, 3, 4, 5, 6 }.AsMemory();
            var predicate = new IsEvenPredicate();
            var selector = new DoubleSelector();
            
            // Act
            var result = memory
                .Where<IsEvenPredicate>(in predicate)
                .Select<int, DoubleSelector>(in selector)
                .ToArray();
            
            // Assert
            result.Must().BeEqualTo(new[] { 4, 8, 12 });
        }

        // Value delegate implementations
        private struct DoubleSelector : IFunctionIn<int, int>
        {
            public int Invoke(in int item) => item * 2;
        }

        private struct HalfSelector : IFunctionIn<int, int>
        {
            public int Invoke(in int item) => item / 2;
        }

        private struct IsEvenPredicate : IFunctionIn<int, bool>
        {
            public bool Invoke(in int item) => item % 2 == 0;
        }

        private struct IsOddPredicate : IFunctionIn<int, bool>
        {
            public bool Invoke(in int item) => item % 2 != 0;
        }
    }
}
