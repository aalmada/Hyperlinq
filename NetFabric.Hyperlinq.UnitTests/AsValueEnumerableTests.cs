using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests;

public class AsValueEnumerableTests
{
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntEnumerableSources))]
    public async Task AsValueEnumerable_List_ShouldReturnListValueEnumerable(Func<IEnumerable<int>> sourceFactory, string description)
    {
        var source = sourceFactory();
        if (source is List<int> list)
        {
            var valueEnum = list.AsValueEnumerable();
            await Assert.That(valueEnum.GetType().Name).IsEqualTo("ListValueEnumerable`1");
        }
    }
    
    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntEnumerableSources))]
    public async Task AsValueEnumerable_Array_ShouldReturnArrayValueEnumerable(Func<IEnumerable<int>> sourceFactory, string description)
    {
        var source = sourceFactory();
        if (source is int[] array)
        {
            var valueEnum = array.AsValueEnumerable();
            await Assert.That(valueEnum.GetType().Name).IsEqualTo("ArrayValueEnumerable`1");
        }
    }
    
    [Test]
    public async Task AsValueEnumerable_Any_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3 };
        var result = list.AsValueEnumerable().Any();
        await Assert.That(result).IsTrue();
    }
    
    [Test]
    public async Task AsValueEnumerable_Count_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.AsValueEnumerable().Count();
        await Assert.That(result).IsEqualTo(5);
    }
    
    [Test]
    public async Task AsValueEnumerable_First_ShouldWork()
    {
        var list = new List<int> { 10, 20, 30 };
        var result = list.AsValueEnumerable().First();
        await Assert.That(result).IsEqualTo(10);
    }
    
    [Test]
    public async Task AsValueEnumerable_Single_ShouldWork()
    {
        var list = new List<int> { 42 };
        var result = list.AsValueEnumerable().Single();
        await Assert.That(result).IsEqualTo(42);
    }
    
    [Test]
    public async Task AsValueEnumerable_Sum_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.AsValueEnumerable().Sum();
        await Assert.That(result).IsEqualTo(15);
    }
    
    [Test]
    public async Task AsValueEnumerable_Chained_WhereSelect_ShouldWork()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.AsValueEnumerable()
                        .Where(x => x % 2 == 0)
                        .Select(x => x * 10)
                        .Sum();
        await Assert.That(result).IsEqualTo(60);
    }
    
    [Test]
    public async Task AsValueEnumerable_Array_Sum_ShouldWork()
    {
        var array = new int[] { 10, 20, 30 };
        var result = array.AsValueEnumerable().Sum();
        await Assert.That(result).IsEqualTo(60);
    }
}
