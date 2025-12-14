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
    public bool HasValue 
        => _hasValue;

    /// <summary>
    /// Gets the value if present, otherwise throws an exception.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the option has no value.</exception>
    public T Value 
        => _hasValue 
            ? _value 
            : throw new InvalidOperationException("Option has no value");

    /// <summary>
    /// Creates an option with a value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some(T value) 
        => new Option<T>(value);

    /// <summary>
    /// Creates an option without a value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> None() 
        => default;

    /// <summary>
    /// Gets the value if present, otherwise returns the default value for the type.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetValueOrDefault() 
        => _hasValue 
            ? _value 
            : default!;

    /// <summary>
    /// Gets the value if present, otherwise returns the specified default value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T GetValueOrDefault(T defaultValue) 
        => _hasValue 
            ? _value 
            : defaultValue;

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
        => _hasValue 
            ? $"Some({_value})" 
            : "None";

    /// <summary>
    /// Evaluates the appropriate function based on whether the option has a value.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="some">The function to evaluate if the option has a value.</param>
    /// <param name="none">The function to evaluate if the option has no value.</param>
    /// <returns>The result of the evaluated function.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none)
        => _hasValue 
            ? some(_value) 
            : none();

    /// <summary>
    /// Evaluates the appropriate function based on whether the option has a value.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="some">The function to evaluate if the option has a value.</param>
    /// <param name="none">The value to return if the option has no value.</param>
    /// <returns>The result of the evaluated function or the default value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult>(Func<T, TResult> some, TResult none)
        => _hasValue 
            ? some(_value) 
            : none;

    /// <summary>
    /// Executes the appropriate action based on whether the option has a value.
    /// </summary>
    /// <param name="some">The action to execute if the option has a value.</param>
    /// <param name="none">The action to execute if the option has no value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Match(Action<T> some, Action none)
    {
        if (_hasValue)
        {
            some(_value);
        }
        else
        {
            none();
        }
    }

    /// <summary>
    /// Transforms the value of the option if present.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="map">The function to transform the value.</param>
    /// <returns>An option containing the transformed value, or None if the original option had no value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<TResult> Map<TResult>(Func<T, TResult> map)
        => _hasValue 
            ? Option<TResult>.Some(map(_value)) 
            : Option<TResult>.None();

    /// <summary>
    /// Transforms the value of the option if present, flattening the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="bind">The function to transform the value into an option.</param>
    /// <returns>The result of the transformation, or None if the original option had no value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<TResult> Bind<TResult>(Func<T, Option<TResult>> bind)
        => _hasValue 
            ? bind(_value) 
            : Option<TResult>.None();
}
