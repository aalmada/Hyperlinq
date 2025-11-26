using System;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanFirstOrNoneTests
{
    [Test]
    public void Array_FirstOrNone_NonEmpty_ShouldReturnSome()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        var option = array.FirstOrNone();
        
        option.HasValue.Must().BeTrue();
        option.Value.Must().BeEqualTo(1);
    }
    
    [Test]
    public void Array_FirstOrNone_Empty_ShouldReturnNone()
    {
        var array = Array.Empty<int>();
        var option = array.FirstOrNone();
        
        option.HasValue.Must().BeFalse();
    }
    
    [Test]
    public void List_FirstOrNone_NonEmpty_ShouldReturnSome()
    {
        var list = new System.Collections.Generic.List<int> { 10, 20, 30 };
        var option = list.FirstOrNone();
        
        option.HasValue.Must().BeTrue();
        option.Value.Must().BeEqualTo(10);
    }
    
    [Test]
    public void Memory_FirstOrNone_NonEmpty_ShouldReturnSome()
    {
        var memory = new int[] { 5, 6, 7 }.AsMemory();
        var option = memory.FirstOrNone();
        
        option.HasValue.Must().BeTrue();
        option.Value.Must().BeEqualTo(5);
    }
    
    [Test]
    public void Memory_FirstOrNone_Empty_ShouldReturnNone()
    {
        var memory = Array.Empty<int>().AsMemory();
        var option = memory.FirstOrNone();
        
        option.HasValue.Must().BeFalse();
    }
}
