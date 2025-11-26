using System;
using System.Collections.Generic;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableSingleOrDefaultTests
{
    [Test]
    public void List_AsValueEnumerable_SingleOrDefault_SingleElement_ShouldReturnElement()
    {
        var list = new List<int> { 42 };
        var result = list.AsValueEnumerable().SingleOrDefault();
        
        result.Must().BeEqualTo(42);
    }
    
    [Test]
    public void List_AsValueEnumerable_SingleOrDefault_Empty_ShouldReturnDefault()
    {
        var list = new List<int>();
        var result = list.AsValueEnumerable().SingleOrDefault();
        
        result.Must().BeEqualTo(0);
    }
    
    [Test]
    public void List_AsValueEnumerable_SingleOrDefault_Empty_WithDefault_ShouldReturnProvidedDefault()
    {
        var list = new List<int>();
        var result = list.AsValueEnumerable().SingleOrDefault(99);
        
        result.Must().BeEqualTo(99);
    }
    
    [Test]
    public void List_AsValueEnumerable_SingleOrDefault_MultipleElements_ShouldThrow()
    {
        var list = new List<int> { 1, 2 };
        
        Action action = () => list.AsValueEnumerable().SingleOrDefault();
        action.Must().Throw<InvalidOperationException>();
    }
    
    [Test]
    public void IEnumerable_AsValueEnumerable_SingleOrDefault_SingleElement_ShouldReturnElement()
    {
        IEnumerable<int> enumerable = System.Linq.Enumerable.Range(42, 1);
        var result = enumerable.AsValueEnumerable().SingleOrDefault();
        
        result.Must().BeEqualTo(42);
    }
}
