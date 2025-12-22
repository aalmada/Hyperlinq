namespace NetFabric.Hyperlinq.UnitTests.Fixtures;

/// <summary>
/// Common predicate implementations for use across unit tests.
/// </summary>
public static class CommonPredicates
{
    /// <summary>
    /// Predicate that returns true for even numbers.
    /// </summary>
    public readonly struct IsEven : IFunction<int, bool>
    {
        public bool Invoke(int element) => element % 2 == 0;
    }

    /// <summary>
    /// Predicate that returns true for odd numbers.
    /// </summary>
    public readonly struct IsOdd : IFunction<int, bool>
    {
        public bool Invoke(int element) => element % 2 != 0;
    }

    /// <summary>
    /// Predicate that returns true for numbers greater than 5.
    /// </summary>
    public readonly struct GreaterThanFive : IFunction<int, bool>
    {
        public bool Invoke(int element) => element > 5;
    }

    /// <summary>
    /// Predicate that returns true for positive numbers.
    /// </summary>
    public readonly struct IsPositive : IFunction<int, bool>
    {
        public bool Invoke(int element) => element > 0;
    }
}
