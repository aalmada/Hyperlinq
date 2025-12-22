using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Conversion;

public class SelectToArrayTests
{
    [Test]
    public void Array_Select_ToArray_ShouldReturnTransformedArray()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        
        var result = array.AsValueEnumerable().Select(x => x * 2).ToArray();
        
        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4, 6, 8, 10 });
    }

    [Test]
    public void Array_Select_ToArray_EmptyArray_ShouldReturnEmptyArray()
    {
        var array = Array.Empty<int>();
        
        var result = array.AsValueEnumerable().Select(x => x * 2).ToArray();
        
        _ = result.Must().BeEnumerableOf<int>().BeEmpty();
    }

    [Test]
    public void List_Select_ToArray_ShouldReturnTransformedArray()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        
        var result = list.Select(x => x * 2).ToArray();
        
        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4, 6, 8, 10 });
    }

    [Test]
    public void List_Select_ToArray_EmptyList_ShouldReturnEmptyArray()
    {
        var list = new List<int>();
        
        var result = list.Select(x => x * 2).ToArray();
        
        _ = result.Must().BeEnumerableOf<int>().BeEmpty();
    }

    [Test]
    public void Array_Select_ToArray_LargeArray_ShouldWork()
    {
        var array = Enumerable.Range(1, 1000).ToArray();
        
        var result = array.AsValueEnumerable().Select(x => x * 2).ToArray();
        
        var expected = Enumerable.Range(1, 1000).Select(x => x * 2).ToArray();
        _ = result.Must().BeEnumerableOf<int>().BeEqualTo(expected);
    }

}
