using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Aggregation;

public class ValueEnumerableMinMaxWithFuncPredicateTests
{
    [Test]
    public void ValueEnumerable_Min_WithFuncPredicate_ShouldFindMinOfMatching()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        
        var result = array.AsValueEnumerable().Min(x => x % 2 == 0);
        
        // Min of even numbers: 2
        _ = result.Must().BeEqualTo(2);
    }

    [Test]
    public void ValueEnumerable_Min_WithFuncPredicate_NoMatches_ShouldReturnNone()
    {
        var array = new[] { 1, 3, 5, 7 }; // All odd
        
        var result = array.AsValueEnumerable().MinOrNone(x => x % 2 == 0);
        
        // When no matches, should return None
        _ = result.HasValue.Must().BeFalse();
    }

    [Test]
    public void ValueEnumerable_Max_WithFuncPredicate_ShouldFindMaxOfMatching()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        
        var result = array.AsValueEnumerable().Max(x => x % 2 == 0);
        
        // Max of even numbers: 8
        _ = result.Must().BeEqualTo(8);
    }

    [Test]
    public void ValueEnumerable_Max_WithFuncPredicate_NoMatches_ShouldReturnNone()
    {
        var array = new[] { 1, 3, 5, 7 }; // All odd
        
        var result = array.AsValueEnumerable().MaxOrNone(x => x % 2 == 0);
        
        // When no matches, should return None
        _ = result.HasValue.Must().BeFalse();
    }

    [Test]
    public void ValueEnumerable_MinMax_WithFuncPredicate_ShouldFindBoth()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        
        var result = array.AsValueEnumerable().MinMax(x => x % 2 == 0);
        
        // Min: 2, Max: 8
        _ = result.Min.Must().BeEqualTo(2);
        _ = result.Max.Must().BeEqualTo(8);
    }

    [Test]
    public void List_Min_WithFuncPredicate_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6 };
        
        var result = list.AsValueEnumerable().Min(x => x % 2 == 0);
        
        _ = result.Must().BeEqualTo(2);
    }

    [Test]
    public void List_Max_WithFuncPredicate_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6 };
        
        var result = list.AsValueEnumerable().Max(x => x % 2 == 0);
        
        _ = result.Must().BeEqualTo(6);
    }

    [Test]
    public void ValueEnumerable_Min_WithFuncPredicate_LargeCollection_ShouldWork()
    {
        var array = Enumerable.Range(1, 1000).ToArray();
        
        var result = array.AsValueEnumerable().Min(x => x % 2 == 0);
        
        _ = result.Must().BeEqualTo(2);
    }

    [Test]
    public void ValueEnumerable_Max_WithFuncPredicate_LargeCollection_ShouldWork()
    {
        var array = Enumerable.Range(1, 1000).ToArray();
        
        var result = array.AsValueEnumerable().Max(x => x % 2 == 0);
        
        _ = result.Must().BeEqualTo(1000);
    }
}
