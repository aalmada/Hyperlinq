using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using NetFabric.Hyperlinq.UnitTests.Fixtures;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Aggregation;

public class ValueEnumerableCountWithPredicateTests
{

    [Test]
    public void ValueEnumerable_Count_WithPredicate_ShouldCountMatchingElements()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6 };
        
        var result = array.AsValueEnumerable().Count(new CommonPredicates.IsEven());
        
        // Count of even numbers: 3 (2, 4, 6)
        _ = result.Must().BeEqualTo(3);
    }

    [Test]
    public void ValueEnumerable_Count_WithPredicate_EmptyArray_ShouldReturnZero()
    {
        var array = Array.Empty<int>();
        
        var result = array.AsValueEnumerable().Count(new CommonPredicates.IsEven());
        
        _ = result.Must().BeEqualTo(0);
    }

    [Test]
    public void ValueEnumerable_Count_WithPredicate_NoMatches_ShouldReturnZero()
    {
        var array = new[] { 1, 3, 5, 7, 9 }; // All odd
        
        var result = array.AsValueEnumerable().Count(new CommonPredicates.IsEven());
        
        _ = result.Must().BeEqualTo(0);
    }

    [Test]
    public void ValueEnumerable_Count_WithPredicate_AllMatch_ShouldReturnCount()
    {
        var array = new[] { 2, 4, 6, 8 }; // All even
        
        var result = array.AsValueEnumerable().Count(new CommonPredicates.IsEven());
        
        _ = result.Must().BeEqualTo(4);
    }

    [Test]
    public void ValueEnumerable_Count_WithFuncPredicate_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6 };
        
        var result = array.AsValueEnumerable().Count(x => x % 2 == 0);
        
        _ = result.Must().BeEqualTo(3);
    }

    [Test]
    public void List_Count_WithPredicate_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6 };
        
        var result = list.AsValueEnumerable().Count(new CommonPredicates.IsEven());
        
        _ = result.Must().BeEqualTo(3);
    }

    [Test]
    public void ValueEnumerable_Count_WithPredicate_LargeCollection_ShouldWork()
    {
        var array = Enumerable.Range(1, 1000).ToArray();
        
        var result = array.AsValueEnumerable().Count(new CommonPredicates.IsEven());
        
        _ = result.Must().BeEqualTo(500);
    }

    [Test]
    public void ValueEnumerable_Count_WithPredicate_SingleMatch_ShouldReturnOne()
    {
        var array = new[] { 1, 3, 4, 5, 7 }; // Only 4 is even
        
        var result = array.AsValueEnumerable().Count(new CommonPredicates.IsEven());
        
        _ = result.Must().BeEqualTo(1);
    }
}
