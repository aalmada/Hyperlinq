using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using NetFabric.Hyperlinq.UnitTests.Fixtures;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Aggregation;

public class ValueEnumerableAverageWithFuncPredicateTests
{
    [Test]
    public void ValueEnumerable_Average_WithFuncPredicate_ShouldCalculateAverageOfMatching()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6 };
        
        var result = array.AsValueEnumerable().Average(x => x % 2 == 0);
        
        // Average of even numbers: (2 + 4 + 6) / 3 = 4
        _ = result.Must().BeEqualTo(4);
    }

    [Test]
    public void ValueEnumerable_Average_WithFuncPredicate_NoMatches_ShouldReturnNone()
    {
        var array = new[] { 1, 3, 5, 7 }; // All odd
        
        var result = array.AsValueEnumerable().AverageOrNone(x => x % 2 == 0);
        
        // When no matches, returns None
        _ = result.HasValue.Must().BeFalse();
    }

    [Test]
    public void ValueEnumerable_Average_WithFuncPredicate_AllMatch_ShouldCalculateAverage()
    {
        var array = new[] { 2, 4, 6, 8 }; // All even
        
        var result = array.AsValueEnumerable().Average(x => x % 2 == 0);
        
        // Average: (2 + 4 + 6 + 8) / 4 = 5
        _ = result.Must().BeEqualTo(5);
    }

    [Test]
    public void List_Average_WithFuncPredicate_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6 };
        
        var result = list.AsValueEnumerable().Average(x => x % 2 == 0);
        
        _ = result.Must().BeEqualTo(4);
    }

    [Test]
    public void ValueEnumerable_Average_WithFuncPredicate_LargeCollection_ShouldWork()
    {
        var array = Enumerable.Range(1, 1000).ToArray();
        
        var result = array.AsValueEnumerable().Average(x => x % 2 == 0);
        var expected = (int)Enumerable.Where(array, x => x % 2 == 0).Average();
        
        _ = result.Must().BeEqualTo(expected);
    }

    [Test]
    public void ValueEnumerable_Average_WithFuncPredicate_EmptyArray_ShouldReturnNone()
    {
        var array = Array.Empty<int>();
        
        var result = array.AsValueEnumerable().AverageOrNone(x => x % 2 == 0);
        
        _ = result.HasValue.Must().BeFalse();
    }
}
