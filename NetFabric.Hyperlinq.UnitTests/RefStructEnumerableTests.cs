using System;
using System.Collections.Generic;
using System.Linq;
using TUnit.Core;
using NetFabric.Assertive;

namespace NetFabric.Hyperlinq.UnitTests;

public class RefStructEnumerableTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Select_RefStruct_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        // Direct Select returns ref struct enumerable
        var refStructResult = array.Select(x => x * 2);
        
        var linqResult = Enumerable.Select(array, x => x * 2);
        
        // Verify results match by converting to array
        var refStructArray = refStructResult.ToArray();
        refStructArray.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Array_Where_RefStruct_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var array = testCase.arrayFactory();
        
        // Direct Where returns ref struct enumerable
        var refStructResult = array.Where(x => x % 2 == 0);
        
        var linqResult = Enumerable.Where(array, x => x % 2 == 0);
        
        // Verify results match by converting to array
        var refStructArray = refStructResult.ToArray();
        refStructArray.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Select_RefStruct_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        // Direct Select returns ref struct enumerable
        var refStructResult = list.Select(x => x * 2);
        
        var linqResult = Enumerable.Select(list, x => x * 2);
        
        // Verify results match by converting to array
        var refStructArray = refStructResult.ToArray();
        refStructArray.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_Where_RefStruct_ShouldMatchLinq((Func<int[]> arrayFactory, string description) testCase)
    {
        var list = new List<int>(testCase.arrayFactory());
        
        // Direct Where returns ref struct enumerable
        var refStructResult = list.Where(x => x % 2 == 0);
        
        var linqResult = Enumerable.Where(list, x => x % 2 == 0);
        
        // Verify results match by converting to array
        var refStructArray = refStructResult.ToArray();
        refStructArray.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }

    [Test]
    public void Array_Select_RefStruct_Count_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.Select(x => x * 2);
        
        result.Count().Must().BeEqualTo(5);
    }

    [Test]
    public void Array_Select_RefStruct_Any_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.Select(x => x * 2);
        
        result.Any().Must().BeTrue();
        
        var emptyArray = Array.Empty<int>();
        var emptyResult = emptyArray.Select(x => x * 2);
        emptyResult.Any().Must().BeFalse();
    }

    [Test]
    public void Array_Select_RefStruct_First_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.Select(x => x * 2);
        
        result.First().Must().BeEqualTo(2);
    }

    [Test]
    public void Array_Select_RefStruct_FirstOrNone_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.Select(x => x * 2);
        
        var firstOption = result.FirstOrNone();
        firstOption.HasValue.Must().BeTrue();
        firstOption.Value.Must().BeEqualTo(2);
        
        var emptyArray = Array.Empty<int>();
        var emptyResult = emptyArray.Select(x => x * 2);
        var emptyOption = emptyResult.FirstOrNone();
        emptyOption.HasValue.Must().BeFalse();
    }

    [Test]
    public void Array_Select_RefStruct_Single_ShouldWork()
    {
        var array = new[] { 42 };
        var result = array.Select(x => x * 2);
        
        result.Single().Must().BeEqualTo(84);
    }

    [Test]
    public void Array_Select_RefStruct_ToArray_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.Select(x => x * 2);
        
        var resultArray = result.ToArray();
        resultArray.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4, 6, 8, 10 });
    }

    [Test]
    public void Array_Select_RefStruct_ToList_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.Select(x => x * 2);
        
        var resultList = result.ToList();
        resultList.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4, 6, 8, 10 });
    }

    [Test]
    public void Array_Where_RefStruct_Count_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.Where(x => x % 2 == 0);
        
        result.Count().Must().BeEqualTo(2);
    }

    [Test]
    public void Array_Where_RefStruct_ToArray_ShouldWork()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var result = array.Where(x => x % 2 == 0);
        
        var resultArray = result.ToArray();
        resultArray.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 2, 4 });
    }

    [Test]
    public void List_Select_RefStruct_Count_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.Select(x => x * 2);
        
        result.Count().Must().BeEqualTo(5);
    }

    [Test]
    public void List_Where_RefStruct_Count_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.Where(x => x % 2 == 0);
        
        result.Count().Must().BeEqualTo(2);
    }

    [Test]
    public void AsValueEnumerable_Select_ShouldBeChainable()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        
        // AsValueEnumerable allows chaining
        var result = array.AsValueEnumerable()
                          .Select(x => x * 2)
                          .Where(x => x > 5)
                          .ToArray();
        
        result.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 6, 8, 10 });
    }

    [Test]
    public void AsValueEnumerable_List_Select_ShouldBeChainable()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        
        // AsValueEnumerable allows chaining
        var result = list.AsValueEnumerable()
                         .Select(x => x * 2)
                         .Where(x => x > 5)
                         .ToArray();
        
        result.Must().BeEnumerableOf<int>().BeEqualTo(new[] { 6, 8, 10 });
    }
}
