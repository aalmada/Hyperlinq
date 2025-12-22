using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using NetFabric.Hyperlinq.UnitTests.Fixtures;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Aggregation;

public class ValueEnumerableSumWithPredicateTests
{
    [Test]
    public void ValueEnumerable_Sum_WithPredicate_ShouldSumMatchingElements()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6 };
        
        var result = array.AsValueEnumerable().Sum(new CommonPredicates.IsEven());
        
        // Sum of even numbers: 2 + 4 + 6 = 12
        _ = result.Must().BeEqualTo(12);
    }

    [Test]
    public void ValueEnumerable_Sum_WithPredicate_EmptyArray_ShouldReturnZero()
    {
        var array = Array.Empty<int>();
        
        var result = array.AsValueEnumerable().Sum(new CommonPredicates.IsEven());
        
        _ = result.Must().BeEqualTo(0);
    }

    [Test]
    public void ValueEnumerable_Sum_WithPredicate_NoMatches_ShouldReturnZero()
    {
        var array = new[] { 1, 3, 5, 7, 9 }; // All odd
        
        var result = array.AsValueEnumerable().Sum(new CommonPredicates.IsEven());
        
        _ = result.Must().BeEqualTo(0);
    }

    [Test]
    public void ValueEnumerable_Sum_WithPredicate_AllMatch_ShouldSumAll()
    {
        var array = new[] { 2, 4, 6, 8 }; // All even
        
        var result = array.AsValueEnumerable().Sum(new CommonPredicates.IsEven());
        
        _ = result.Must().BeEqualTo(20);
    }

    [Test]
    public void ValueEnumerable_Sum_WithFuncPredicate_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6 };
        
        var result = array.AsValueEnumerable().Sum(x => x % 2 == 0);
        
        _ = result.Must().BeEqualTo(12);
    }

    [Test]
    public void List_Sum_WithPredicate_ShouldSumMatchingElements()
    {
        var list = new List<int> { 1, 2, 3, 4, 5, 6 };
        
        var result = list.AsValueEnumerable().Sum(new CommonPredicates.IsEven());
        
        _ = result.Must().BeEqualTo(12);
    }

    [Test]
    public void ValueEnumerable_Sum_WithPredicate_LargeCollection_ShouldWork()
    {
        var array = Enumerable.Range(1, 1000).ToArray();
        
        var result = array.AsValueEnumerable().Sum(new CommonPredicates.IsEven());
        var expected = Enumerable.Where(array, x => x % 2 == 0).Sum();
        
        _ = result.Must().BeEqualTo(expected);
    }
}
