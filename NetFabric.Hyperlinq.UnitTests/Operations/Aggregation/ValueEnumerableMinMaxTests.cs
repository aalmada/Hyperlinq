using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Aggregation;

public class ValueEnumerableMinMaxTests
{
    [Test]
    public void ValueEnumerable_Min_ShouldFindMinimum()
    {
        var array = new[] { 5, 2, 8, 1, 9 };
        
        var result = array.AsValueEnumerable().Min();
        
        _ = result.Must().BeEqualTo(1);
    }

    [Test]
    public void ValueEnumerable_Min_SingleElement_ShouldReturnElement()
    {
        var array = new[] { 42 };
        
        var result = array.AsValueEnumerable().Min();
        
        _ = result.Must().BeEqualTo(42);
    }

    [Test]
    public void ValueEnumerable_Max_ShouldFindMaximum()
    {
        var array = new[] { 5, 2, 8, 1, 9 };
        
        var result = array.AsValueEnumerable().Max();
        
        _ = result.Must().BeEqualTo(9);
    }

    [Test]
    public void ValueEnumerable_Max_SingleElement_ShouldReturnElement()
    {
        var array = new[] { 42 };
        
        var result = array.AsValueEnumerable().Max();
        
        _ = result.Must().BeEqualTo(42);
    }

    [Test]
    public void ValueEnumerable_Min_WithNegativeNumbers_ShouldWork()
    {
        var array = new[] { -5, -3, 2, 4, -10 };
        
        var result = array.AsValueEnumerable().Min();
        
        _ = result.Must().BeEqualTo(-10);
    }

    [Test]
    public void ValueEnumerable_Max_WithNegativeNumbers_ShouldWork()
    {
        var array = new[] { -5, -3, 2, 4, -10 };
        
        var result = array.AsValueEnumerable().Max();
        
        _ = result.Must().BeEqualTo(4);
    }

    [Test]
    public void List_Min_ShouldFindMinimum()
    {
        var list = new List<int> { 5, 2, 8, 1, 9 };
        
        var result = list.Min();
        
        _ = result.Must().BeEqualTo(1);
    }

    [Test]
    public void List_Max_ShouldFindMaximum()
    {
        var list = new List<int> { 5, 2, 8, 1, 9 };
        
        var result = list.Max();
        
        _ = result.Must().BeEqualTo(9);
    }

    [Test]
    public void ValueEnumerable_Min_LargeCollection_ShouldWork()
    {
        var array = Enumerable.Range(1, 1000).ToArray();
        
        var result = array.AsValueEnumerable().Min();
        
        _ = result.Must().BeEqualTo(1);
    }

    [Test]
    public void ValueEnumerable_Max_LargeCollection_ShouldWork()
    {
        var array = Enumerable.Range(1, 1000).ToArray();
        
        var result = array.AsValueEnumerable().Max();
        
        _ = result.Must().BeEqualTo(1000);
    }
}
