using System;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanFirstOrDefaultTests
{
    [Test]
    public void Array_FirstOrDefault_NonEmpty_ShouldReturnFirst()
    {
        var array = new int[] { 1, 2, 3 };
        var result = array.FirstOrDefault();
        
        result.Must().BeEqualTo(1);
    }
    
    [Test]
    public void Array_FirstOrDefault_Empty_ShouldReturnDefault()
    {
        var array = Array.Empty<int>();
        var result = array.FirstOrDefault();
        
        result.Must().BeEqualTo(0);
    }
    
    [Test]
    public void Array_FirstOrDefault_Empty_WithDefault_ShouldReturnProvidedDefault()
    {
        var array = Array.Empty<int>();
        var result = array.FirstOrDefault(99);
        
        result.Must().BeEqualTo(99);
    }
    
    [Test]
    public void List_FirstOrDefault_NonEmpty_ShouldReturnFirst()
    {
        var list = new System.Collections.Generic.List<int> { 10, 20 };
        var result = list.FirstOrDefault();
        
        result.Must().BeEqualTo(10);
    }
    
    [Test]
    public void Memory_FirstOrDefault_NonEmpty_ShouldReturnFirst()
    {
        var memory = new int[] { 5, 6 }.AsMemory();
        var result = memory.FirstOrDefault();
        
        result.Must().BeEqualTo(5);
    }
}
