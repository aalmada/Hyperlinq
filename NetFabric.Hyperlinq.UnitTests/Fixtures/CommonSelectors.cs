namespace NetFabric.Hyperlinq.UnitTests.Fixtures;

/// <summary>
/// Common selector implementations for use across unit tests.
/// </summary>
public static class CommonSelectors
{
    /// <summary>
    /// Selector that doubles the input value.
    /// </summary>
    public readonly struct Double : IFunction<int, int>
    {
        public int Invoke(int element) => element * 2;
    }

    /// <summary>
    /// Selector that doubles the input value (pass-by-reference variant).
    /// </summary>
    public readonly struct DoubleIn : IFunctionIn<int, int>
    {
        public int Invoke(in int element) => element * 2;
    }

    /// <summary>
    /// Selector that converts int to string.
    /// </summary>
    public readonly struct ToStringSelector : IFunction<int, string>
    {
        public string Invoke(int element) => element.ToString();
    }

    /// <summary>
    /// Selector that returns the square of the input.
    /// </summary>
    public readonly struct Square : IFunction<int, int>
    {
        public int Invoke(int element) => element * element;
    }
}
