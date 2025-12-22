using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Aggregation;

public class ValueEnumerableSumTests
{
    [Test]
    public void ValueEnumerable_Sum_ShouldCalculateCorrectly()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        
        var result = array.AsValueEnumerable().Sum();
        
        _ = result.Must().BeEqualTo(15);
    }

    [Test]
    public void ValueEnumerable_Sum_EmptyCollection_ShouldReturnZero()
    {
        var array = Array.Empty<int>();
        
        var result = array.AsValueEnumerable().Sum();
        
        _ = result.Must().BeEqualTo(0);
    }

    [Test]
    public void ValueEnumerable_Sum_SingleElement_ShouldReturnElement()
    {
        var array = new[] { 42 };
        
        var result = array.AsValueEnumerable().Sum();
        
        _ = result.Must().BeEqualTo(42);
    }

    [Test]
    public void ValueEnumerable_Sum_LargeCollection_ShouldWork()
    {
        var array = Enumerable.Range(1, 1000).ToArray();
        
        var result = array.AsValueEnumerable().Sum();
        var expected = Enumerable.Sum(array);
        
        _ = result.Must().BeEqualTo(expected);
    }

    [Test]
    public void ValueEnumerable_Sum_WithNegativeNumbers_ShouldWork()
    {
        var array = new[] { -5, -3, 2, 4, -1 };
        
        var result = array.AsValueEnumerable().Sum();
        
        _ = result.Must().BeEqualTo(-3);
    }

    [Test]
    public void List_Sum_ShouldCalculateCorrectly()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        
        var result = list.Sum();
        
        _ = result.Must().BeEqualTo(15);
    }

    [Test]
    public void List_Sum_EmptyList_ShouldReturnZero()
    {
        var list = new List<int>();
        
        var result = list.Sum();
        
        _ = result.Must().BeEqualTo(0);
    }
}
