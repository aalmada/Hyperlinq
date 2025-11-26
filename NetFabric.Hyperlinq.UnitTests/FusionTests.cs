using System;
using System.Collections.Generic;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class FusionTests
{
    // ValueEnumerable Tests
    [Test]
    public void ValueEnumerable_Count_Predicate_ShouldMatch()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.AsValueEnumerable().Count(x => x % 2 == 0);
        
        result.Must().BeEqualTo(2);
    }

    [Test]
    public void ValueEnumerable_Any_Predicate_ShouldMatch()
    {
        var list = new List<int> { 1, 2, 3 };
        var result = list.AsValueEnumerable().Any(x => x > 2);
        
        result.Must().BeEqualTo(true);
    }

    [Test]
    public void ValueEnumerable_First_Predicate_ShouldMatch()
    {
        var list = new List<int> { 1, 2, 3 };
        var result = list.AsValueEnumerable().First(x => x > 1);
        
        result.Must().BeEqualTo(2);
    }

    [Test]
    public void ValueEnumerable_Sum_Predicate_ShouldMatch()
    {
        var list = new List<int> { 1, 2, 3, 4 };
        // Sum should add up the values where predicate is true (2 + 4 = 6)
        var result = list.AsValueEnumerable().Sum(x => x % 2 == 0);
        
        result.Must().BeEqualTo(6);
    }

    // Span Tests
    [Test]
    public void Span_Count_Predicate_ShouldMatch()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var result = array.AsSpan().Count(x => x % 2 == 0);
        
        result.Must().BeEqualTo(2);
    }

    [Test]
    public void Span_Any_Predicate_ShouldMatch()
    {
        var array = new int[] { 1, 2, 3 };
        var result = array.AsSpan().Any(x => x > 2);
        
        result.Must().BeEqualTo(true);
    }

    [Test]
    public void Span_First_Predicate_ShouldMatch()
    {
        var array = new int[] { 1, 2, 3 };
        var result = array.AsSpan().First(x => x > 1);
        
        result.Must().BeEqualTo(2);
    }

    [Test]
    public void Span_Sum_Predicate_ShouldMatch()
    {
        var array = new int[] { 1, 2, 3, 4 };
        var result = array.AsSpan().Sum(x => x % 2 == 0);
        
        result.Must().BeEqualTo(6);
    }

    // Array Tests (Delegating)
    [Test]
    public void Array_Count_Predicate_ShouldMatch()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var result = array.Count(x => x % 2 == 0);
        
        result.Must().BeEqualTo(2);
    }

    [Test]
    public void Array_Sum_Predicate_ShouldMatch()
    {
        var array = new int[] { 1, 2, 3, 4 };
        var result = array.Sum(x => x % 2 == 0);
        
        result.Must().BeEqualTo(6);
    }

    // List Tests (Delegating)
    [Test]
    public void List_Count_Predicate_ShouldMatch()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.Count(x => x % 2 == 0);
        
        result.Must().BeEqualTo(2);
    }

    [Test]
    public void List_Sum_Predicate_ShouldMatch()
    {
        var list = new List<int> { 1, 2, 3, 4 };
        var result = list.Sum(x => x % 2 == 0);
        
        result.Must().BeEqualTo(6);
    }
}
