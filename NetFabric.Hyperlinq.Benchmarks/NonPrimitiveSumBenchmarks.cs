using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using NetFabric.Hyperlinq;

namespace NetFabric.Hyperlinq.Benchmarks
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class NonPrimitiveSumBenchmarks
    {
        private MyInt[] array = null!;

        [Params(100, 1_000, 10_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            array = Enumerable.Range(0, Count).Select(i => new MyInt(i)).ToArray();
        }

        [BenchmarkCategory("Sum")]
        [Benchmark(Baseline = true)]
        public MyInt Sum_LINQ()
            => Enumerable.Aggregate(array, MyInt.Zero, (acc, x) => acc + x);

        [BenchmarkCategory("Sum")]
        [Benchmark]
        public MyInt Sum_Hyperlinq()
            => array.AsValueEnumerable().Sum();

        [BenchmarkCategory("Sum")]
        [Benchmark]
        public MyInt Sum_Span()
            => array.AsSpan().Sum();

        [BenchmarkCategory("Sum")]
        [Benchmark]
        public MyInt Sum_TensorPrimitives()
            => System.Numerics.Tensors.TensorPrimitives.Sum<MyInt>(array);

        public readonly struct MyInt : INumber<MyInt>
        {
            private readonly int value;

            public MyInt(int value)
            {
                this.value = value;
            }

            public static MyInt Zero => new(0);
            public static MyInt One => new(1);
            public static int Radix => 2;
            public static MyInt AdditiveIdentity => Zero;
            public static MyInt MultiplicativeIdentity => One;

            public static MyInt Abs(MyInt value) => new(Math.Abs(value.value));
            public static bool IsCanonical(MyInt value) => true;
            public static bool IsComplexNumber(MyInt value) => false;
            public static bool IsEvenInteger(MyInt value) => (value.value & 1) == 0;
            public static bool IsFinite(MyInt value) => true;
            public static bool IsImaginaryNumber(MyInt value) => false;
            public static bool IsInfinity(MyInt value) => false;
            public static bool IsInteger(MyInt value) => true;
            public static bool IsNaN(MyInt value) => false;
            public static bool IsNegative(MyInt value) => value.value < 0;
            public static bool IsNegativeInfinity(MyInt value) => false;
            public static bool IsNormal(MyInt value) => true;
            public static bool IsOddInteger(MyInt value) => (value.value & 1) != 0;
            public static bool IsPositive(MyInt value) => value.value >= 0;
            public static bool IsPositiveInfinity(MyInt value) => false;
            public static bool IsRealNumber(MyInt value) => true;
            public static bool IsSubnormal(MyInt value) => false;
            public static bool IsZero(MyInt value) => value.value == 0;
            public static MyInt MaxMagnitude(MyInt x, MyInt y) 
            {
                var absX = Math.Abs(x.value);
                var absY = Math.Abs(y.value);
                if (absX > absY) return x;
                if (absX < absY) return y;
                return x.value > y.value ? x : y;
            }
            public static MyInt MaxMagnitudeNumber(MyInt x, MyInt y) => MaxMagnitude(x, y);
            public static MyInt MinMagnitude(MyInt x, MyInt y)
            {
                var absX = Math.Abs(x.value);
                var absY = Math.Abs(y.value);
                if (absX < absY) return x;
                if (absX > absY) return y;
                return x.value < y.value ? x : y;
            }
            public static MyInt MinMagnitudeNumber(MyInt x, MyInt y) => MinMagnitude(x, y);
            public static MyInt Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => new(int.Parse(s, style, provider));
            public static MyInt Parse(string s, NumberStyles style, IFormatProvider? provider) => new(int.Parse(s, style, provider));
            public static MyInt Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => new(int.Parse(s, provider));
            public static MyInt Parse(string s, IFormatProvider? provider) => new(int.Parse(s, provider));
            public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out MyInt result)
            {
                if (int.TryParse(s, style, provider, out var i))
                {
                    result = new MyInt(i);
                    return true;
                }
                result = default;
                return false;
            }
            public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out MyInt result)
            {
                if (int.TryParse(s, style, provider, out var i))
                {
                    result = new MyInt(i);
                    return true;
                }
                result = default;
                return false;
            }
            public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out MyInt result)
            {
                if (int.TryParse(s, provider, out var i))
                {
                    result = new MyInt(i);
                    return true;
                }
                result = default;
                return false;
            }
            public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out MyInt result)
            {
                if (int.TryParse(s, provider, out var i))
                {
                    result = new MyInt(i);
                    return true;
                }
                result = default;
                return false;
            }

            public static bool operator ==(MyInt left, MyInt right) => left.value == right.value;
            public static bool operator !=(MyInt left, MyInt right) => left.value != right.value;
            public static bool operator >(MyInt left, MyInt right) => left.value > right.value;
            public static bool operator >=(MyInt left, MyInt right) => left.value >= right.value;
            public static bool operator <(MyInt left, MyInt right) => left.value < right.value;
            public static bool operator <=(MyInt left, MyInt right) => left.value <= right.value;
            public static MyInt operator +(MyInt left, MyInt right) => new(left.value + right.value);
            public static MyInt operator -(MyInt left, MyInt right) => new(left.value - right.value);
            public static MyInt operator *(MyInt left, MyInt right) => new(left.value * right.value);
            public static MyInt operator /(MyInt left, MyInt right) => new(left.value / right.value);
            public static MyInt operator %(MyInt left, MyInt right) => new(left.value % right.value);
            public static MyInt operator +(MyInt value) => new(+value.value);
            public static MyInt operator -(MyInt value) => new(-value.value);
            public static MyInt operator ++(MyInt value) => new(value.value + 1);
            public static MyInt operator --(MyInt value) => new(value.value - 1);

            public int CompareTo(object? obj) => obj is MyInt other ? CompareTo(other) : 1;
            public int CompareTo(MyInt other) => value.CompareTo(other.value);
            public bool Equals(MyInt other) => value.Equals(other.value);
            public override bool Equals(object? obj) => obj is MyInt other && Equals(other);
            public override int GetHashCode() => value.GetHashCode();
            public override string ToString() => value.ToString();
            public string ToString(string? format, IFormatProvider? formatProvider) => value.ToString(format, formatProvider);
            public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => value.TryFormat(destination, out charsWritten, format, provider);

            // Explicit interface implementations for things we don't really care about but must implement
            static bool INumberBase<MyInt>.TryConvertFromChecked<TOther>(TOther value, out MyInt result)
            {
                // Simple implementation for int
                if (typeof(TOther) == typeof(int))
                {
                    result = new MyInt((int)(object)value!);
                    return true;
                }
                result = default;
                return false;
            }
            static bool INumberBase<MyInt>.TryConvertFromSaturating<TOther>(TOther value, out MyInt result) 
            {
                if (typeof(TOther) == typeof(int))
                {
                    result = new MyInt((int)(object)value!);
                    return true;
                }
                result = default;
                return false;
            }
            static bool INumberBase<MyInt>.TryConvertFromTruncating<TOther>(TOther value, out MyInt result) 
            {
                if (typeof(TOther) == typeof(int))
                {
                    result = new MyInt((int)(object)value!);
                    return true;
                }
                result = default;
                return false;
            }
            static bool INumberBase<MyInt>.TryConvertToChecked<TOther>(MyInt value, out TOther result)
            {
                if (typeof(TOther) == typeof(int))
                {
                    result = (TOther)(object)value.value;
                    return true;
                }
                result = default!;
                return false;
            }
            static bool INumberBase<MyInt>.TryConvertToSaturating<TOther>(MyInt value, out TOther result) 
            {
                if (typeof(TOther) == typeof(int))
                {
                    result = (TOther)(object)value.value;
                    return true;
                }
                result = default!;
                return false;
            }
            static bool INumberBase<MyInt>.TryConvertToTruncating<TOther>(MyInt value, out TOther result) 
            {
                if (typeof(TOther) == typeof(int))
                {
                    result = (TOther)(object)value.value;
                    return true;
                }
                result = default!;
                return false;
            }

        }
    }
}
