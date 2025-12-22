using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using NetFabric.Hyperlinq.UnitTests.Fixtures;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Quantifier;

public class ValueEnumerableAnyAllWithPredicateTests
{
    #region Any Tests

    [Test]
    public void ValueEnumerable_Any_WithFuncPredicate_ShouldReturnTrueWhenMatch()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        
        var result = array.AsValueEnumerable().Any(x => x % 2 == 0);
        
        _ = result.Must().BeTrue();
    }

    [Test]
    public void ValueEnumerable_Any_WithFuncPredicate_NoMatches_ShouldReturnFalse()
    {
        var array = new[] { 1, 3, 5, 7 }; // All odd
        
        var result = array.AsValueEnumerable().Any(x => x % 2 == 0);
        
        _ = result.Must().BeFalse();
    }

    [Test]
    public void ValueEnumerable_Any_WithFuncPredicate_EmptyArray_ShouldReturnFalse()
    {
        var array = Array.Empty<int>();
        
        var result = array.AsValueEnumerable().Any(x => x % 2 == 0);
        
        _ = result.Must().BeFalse();
    }

    [Test]
    public void List_Any_WithFuncPredicate_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        
        var result = list.AsValueEnumerable().Any(x => x % 2 == 0);
        
        _ = result.Must().BeTrue();
    }

    #endregion

    #region All Tests

    [Test]
    public void ValueEnumerable_All_WithFuncPredicate_AllMatch_ShouldReturnTrue()
    {
        var array = new[] { 2, 4, 6, 8 }; // All even
        
        var result = array.AsValueEnumerable().All(x => x % 2 == 0);
        
        _ = result.Must().BeTrue();
    }

    [Test]
    public void ValueEnumerable_All_WithFuncPredicate_SomeMatch_ShouldReturnFalse()
    {
        var array = new[] { 1, 2, 3, 4 }; // Mixed
        
        var result = array.AsValueEnumerable().All(x => x % 2 == 0);
        
        _ = result.Must().BeFalse();
    }

    [Test]
    public void ValueEnumerable_All_WithFuncPredicate_EmptyArray_ShouldReturnTrue()
    {
        var array = Array.Empty<int>();
        
        var result = array.AsValueEnumerable().All(x => x % 2 == 0);
        
        // All() on empty collection returns true
        _ = result.Must().BeTrue();
    }

    [Test]
    public void List_All_WithFuncPredicate_ShouldWork()
    {
        var list = new List<int> { 2, 4, 6, 8 };
        
        var result = list.AsValueEnumerable().All(x => x % 2 == 0);
        
        _ = result.Must().BeTrue();
    }

    [Test]
    public void ValueEnumerable_Any_WithFuncPredicate_LargeCollection_ShouldWork()
    {
        var array = Enumerable.Range(1, 1000).ToArray();
        
        var result = array.AsValueEnumerable().Any(x => x > 500);
        
        _ = result.Must().BeTrue();
    }

    [Test]
    public void ValueEnumerable_All_WithFuncPredicate_LargeCollection_ShouldWork()
    {
        var array = Enumerable.Range(1, 1000).ToArray();
        
        var result = array.AsValueEnumerable().All(x => x > 0);
        
        _ = result.Must().BeTrue();
    }

    #endregion
}
