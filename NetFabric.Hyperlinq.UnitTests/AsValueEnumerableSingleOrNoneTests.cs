using System;
using System.Collections.Generic;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableSingleOrNoneTests
{
    [Test]
    public void List_AsValueEnumerable_SingleOrNone_SingleElement_ShouldReturnSome()
    {
        var list = new List<int> { 42 };
        var option = list.AsValueEnumerable().SingleOrNone();
        
        option.HasValue.Must().BeTrue();
        option.Value.Must().BeEqualTo(42);
    }
    
    [Test]
    public void List_AsValueEnumerable_SingleOrNone_Empty_ShouldReturnNone()
    {
        var list = new List<int>();
        var option = list.AsValueEnumerable().SingleOrNone();
        
        option.HasValue.Must().BeFalse();
    }
    
    [Test]
    public void List_AsValueEnumerable_SingleOrNone_MultipleElements_ShouldThrow()
    {
        var list = new List<int> { 1, 2, 3 };
        
        Action action = () => list.AsValueEnumerable().SingleOrNone();
        action.Must().Throw<InvalidOperationException>();
    }
    
    [Test]
    public void IList_AsValueEnumerable_SingleOrNone_ShouldUseOptimization()
    {
        IList<int> list = new List<int> { 99 };
        var option = list.AsValueEnumerable().SingleOrNone();
        
        option.HasValue.Must().BeTrue();
        option.Value.Must().BeEqualTo(99);
    }
    
    [Test]
    public void IEnumerable_AsValueEnumerable_SingleOrNone_SingleElement_ShouldReturnSome()
    {
        IEnumerable<int> enumerable = System.Linq.Enumerable.Range(42, 1);
        var option = enumerable.AsValueEnumerable().SingleOrNone();
        
        option.HasValue.Must().BeTrue();
        option.Value.Must().BeEqualTo(42);
    }
    
    [Test]
    public void IEnumerable_AsValueEnumerable_SingleOrNone_Empty_ShouldReturnNone()
    {
        IEnumerable<int> enumerable = System.Linq.Enumerable.Empty<int>();
        var option = enumerable.AsValueEnumerable().SingleOrNone();
        
        option.HasValue.Must().BeFalse();
    }
}
