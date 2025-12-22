using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using NetFabric.Hyperlinq.UnitTests.TestData;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Operations.Quantifier;

public class ListSelectIndexOfTests
{
    [Test]
    public void List_Select_IndexOf_WithMatchingElement_ShouldReturnIndex()
    {
        var list = CommonTestData.SmallList;
        
        var result = list
            .Select(x => x * 2)
            .IndexOf(6); // 3 * 2 = 6, at index 2
        
        _ = result.Must().BeEqualTo(2);
    }

    [Test]
    public void List_Select_IndexOf_WithNoMatch_ShouldReturnMinusOne()
    {
        var list = CommonTestData.SmallList;
        
        var result = list
            .Select(x => x * 2)
            .IndexOf(11);
        
        _ = result.Must().BeEqualTo(-1);
    }

    [Test]
    public void List_Select_IndexOf_EmptyList_ShouldReturnMinusOne()
    {
        var list = CommonTestData.EmptyList;
        
        var result = list
            .Select(x => x * 2)
            .IndexOf(6);
        
        _ = result.Must().BeEqualTo(-1);
    }

    [Test]
    public void List_Select_IndexOf_FirstElement_ShouldReturnZero()
    {
        var list = CommonTestData.SmallList;
        
        var result = list
            .Select(x => x * 2)
            .IndexOf(2); // 1 * 2 = 2, at index 0
        
        _ = result.Must().BeEqualTo(0);
    }

    [Test]
    public void List_Select_IndexOf_LastElement_ShouldReturnLastIndex()
    {
        var list = CommonTestData.SmallList;
        
        var result = list
            .Select(x => x * 2)
            .IndexOf(10); // 5 * 2 = 10, at index 4
        
        _ = result.Must().BeEqualTo(4);
    }

    [Test]
    public void List_Select_IndexOf_MultipleOccurrences_ShouldReturnFirstIndex()
    {
        var list = new List<int> { 1, 2, 3, 2, 5 };
        
        var result = list
            .Select(x => x * 2)
            .IndexOf(4); // 2 * 2 = 4, first at index 1
        
        _ = result.Must().BeEqualTo(1);
    }

    [Test]
    public void List_Select_IndexOf_LargeList_ShouldWork()
    {
        var list = CommonTestData.LargeList;
        
        var result = list
            .Select(x => x * 2)
            .IndexOf(1000); // 500 * 2 = 1000, at index 499
        
        _ = result.Must().BeEqualTo(499);
    }
}
