using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

/// <summary>
/// Represents an optional value that may or may not be present.
/// This type provides a type-safe alternative to nullable references and default values.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public readonly record struct Option<T>
{
    private readonly bool _hasValue;
    private readonly T _value;

    private Option(T value)
    {
        _hasValue = true;
        _value = value;
    }

    /// <summary>
    /// Gets a value indicating whether this option contains a value.
    /// </summary>
    public bool HasValue => _hasValue;

    /// <summary>
    /// Gets the value if present, otherwise throws an exception.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the option has no value.</exception>
    public T Value => _hasValue ? _value : throw new InvalidOperationException("Option has no value");

    /// <summary>
    /// Creates an option with a value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some(T value) => new Option<T>(value);

    /// <summary>
    /// Creates an option without a value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> None() => default;

    /// <summary>
    /// Gets the value if present, otherwise returns the default value for the type.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetValueOrDefault() => _hasValue ? _value : default!;

    /// <summary>
    /// Gets the value if present, otherwise returns the specified default value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetValueOrDefault(T defaultValue) => _hasValue ? _value : defaultValue;

    /// <summary>
    /// Deconstructs the option into its components for pattern matching.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out bool hasValue, out T value)
    {
        hasValue = _hasValue;
        value = _value;
    }

    /// <summary>
    /// Returns a string representation of this option.
    /// </summary>
    public override string ToString()
        => _hasValue ? $"Some({_value})" : "None";
}
