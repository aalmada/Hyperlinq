using System;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanSingleOrDefaultTests
{
    [Test]
    public void Array_SingleOrDefault_SingleElement_ShouldReturnElement()
    {
        var array = new int[] { 42 };
        var result = array.SingleOrDefault();
        
        result.Must().BeEqualTo(42);
    }
    
    [Test]
    public void Array_SingleOrDefault_Empty_ShouldReturnDefault()
    {
        var array = Array.Empty<int>();
        var result = array.SingleOrDefault();
        
        result.Must().BeEqualTo(0);
    }
    
    [Test]
    public void Array_SingleOrDefault_Empty_WithDefault_ShouldReturnProvidedDefault()
    {
        var array = Array.Empty<int>();
        var result = array.SingleOrDefault(99);
        
        result.Must().BeEqualTo(99);
    }
    
    [Test]
    public void Array_SingleOrDefault_MultipleElements_ShouldThrow()
    {
        var array = new int[] { 1, 2 };
        
        Action action = () => array.SingleOrDefault();
        action.Must().Throw<InvalidOperationException>();
    }
    
    [Test]
    public void List_SingleOrDefault_SingleElement_ShouldReturnElement()
    {
        var list = new System.Collections.Generic.List<int> { 99 };
        var result = list.SingleOrDefault();
        
        result.Must().BeEqualTo(99);
    }
    
    [Test]
    public void Memory_SingleOrDefault_SingleElement_ShouldReturnElement()
    {
        var memory = new int[] { 7 }.AsMemory();
        var result = memory.SingleOrDefault();
        
        result.Must().BeEqualTo(7);
    }
}
