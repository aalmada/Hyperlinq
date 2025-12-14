using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NetFabric.Assertive;
using NetFabric.Hyperlinq;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests;

public class ValueDelegateTests
{
    public readonly struct DoubleFn : IFunction<int, int>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Invoke(int item) => item * 2;
    }

    public readonly struct IsEvenFn : IFunction<int, bool>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(int item) => item % 2 == 0;
    }

    [Test]
    public void Array_Select_With_ValueDelegate_Should_Succeed()
    {
        var source = new[] { 1, 2, 3 };
        var expected = new[] { 2, 4, 6 };

        // Using generic explicit call to force value delegate usage
        var result = source.AsValueEnumerable()
            .Select<int, int, DoubleFn>(new DoubleFn())
            .ToArray();

        _ = result.Must().BeEqualTo(expected);
    }

    [Test]
    public void List_Select_With_ValueDelegate_Should_Succeed()
    {
        var source = new List<int> { 1, 2, 3 };
        var expected = new[] { 2, 4, 6 };

        var result = source
            .Select<int, int, DoubleFn>(new DoubleFn())
            .ToArray();

        _ = result.Must().BeEqualTo(expected);
    }

    [Test]
    public void List_Where_With_ValueDelegate_Should_Succeed()
    {
        var source = new List<int> { 1, 2, 3, 4 };
        var expected = new[] { 2, 4 };

        var result = source
            .Where<int, IsEvenFn>(new IsEvenFn())
            .ToArray();

        _ = result.Must().BeEqualTo(expected);
    }
}
