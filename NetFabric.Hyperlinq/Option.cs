using System;
using System.Diagnostics.CodeAnalysis;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq;

/// <summary>
/// Represents an optional value that may or may not be present.
/// This type provides a type-safe alternative to nullable references and default values.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public readonly record struct Option<T>
    : IValueReadOnlyList<T, Option<T>.Enumerator>, IList<T>
{
    readonly bool _hasValue;
    readonly T _value;

    Option(T value)
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
    
    public int Count 
        => _hasValue ? 1 : 0;

    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (_hasValue && index == 0)
            {
                return _value;
            }
            throw new ArgumentOutOfRangeException(nameof(index));
        }
    }

    T IList<T>.this[int index]
    {
        get => this[index];
        set => throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enumerator GetEnumerator() => new Enumerator(in this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(in this);
    IEnumerator IEnumerable.GetEnumerator() => new Enumerator(in this);

    public struct Enumerator
        : IEnumerator<T>
    {
        readonly T _value;
        readonly bool _hasValue;
        bool _enumerated;

        internal Enumerator(in Option<T> option)
        {
            _value = option._value;
            _hasValue = option._hasValue;
            _enumerated = false;
        }

        public readonly T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _value;
        }

        readonly object? IEnumerator.Current 
            => _hasValue ? _value : null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            if (_hasValue && !_enumerated)
            {
                _enumerated = true;
                return true;
            }
            return false;
        }

        public void Reset() => _enumerated = false;

        public void Dispose() { }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Any() 
        => _hasValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(T item)
        => _hasValue && EqualityComparer<T>.Default.Equals(_value, item);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int IndexOf(T item)
        => _hasValue && EqualityComparer<T>.Default.Equals(_value, item)
            ? 0
            : -1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array is null)
        {
             throw new ArgumentNullException(nameof(array));
        }

        if (arrayIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        }

        if (array.Length - arrayIndex < Count)
        {
            throw new ArgumentException("Destination array is not long enough.");
        }

        if (_hasValue)
        {
            array[arrayIndex] = _value;
        }
    }

    bool ICollection<T>.IsReadOnly => true;
    void ICollection<T>.Add(T item) => throw new NotSupportedException();
    void ICollection<T>.Clear() => throw new NotSupportedException();
    bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
    void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
    void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T[] ToArray()
        => _hasValue ? [_value] : [];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<T> ToList()
        => _hasValue ? [_value] : [];
}
