using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TUnit.Core;
using NetFabric.Hyperlinq.LinqHelper;

namespace NetFabric.Hyperlinq.UnitTests;

public class SumTests
{
    [Test]
    [MethodDataSource(nameof(GetIntTestData))]
    public async Task Sum_Int_WithVariousSources_ShouldMatchLinq(Func<IEnumerable<int>> sourceFactory, int expectedSum, string description)
    {
        var source = sourceFactory();
        var hyperlinqResult = source.Sum();
        var linqResult = StandardLinq.SumInt(source);
        
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
        await Assert.That(hyperlinqResult).IsEqualTo(expectedSum);
    }
    
    [Test]
    [MethodDataSource(nameof(GetDoubleTestData))]
    public async Task Sum_Double_WithVariousSources_ShouldMatchLinq(Func<IEnumerable<double>> sourceFactory, double expectedSum, string description)
    {
        var source = sourceFactory();
        var hyperlinqResult = source.Sum();
        var linqResult = StandardLinq.SumDouble(source);
        
        // Use tolerance for floating-point comparison due to potential precision differences
        await Assert.That(Math.Abs(hyperlinqResult - linqResult)).IsLessThan(1e-10);
    }
    
    [Test]
    [MethodDataSource(nameof(GetLongTestData))]
    public async Task Sum_Long_WithVariousSources_ShouldMatchLinq(Func<IEnumerable<long>> sourceFactory, long expectedSum, string description)
    {
        var source = sourceFactory();
        var hyperlinqResult = source.Sum();
        var linqResult = StandardLinq.SumLong(source);
        
        await Assert.That(hyperlinqResult).IsEqualTo(linqResult);
        await Assert.That(hyperlinqResult).IsEqualTo(expectedSum);
    }
    
    [Test]
    [MethodDataSource(nameof(GetFloatTestData))]
    public async Task Sum_Float_WithVariousSources_ShouldMatchLinq(Func<IEnumerable<float>> sourceFactory, float expectedSum, string description)
    {
        var source = sourceFactory();
        var hyperlinqResult = source.Sum();
        var linqResult = StandardLinq.SumFloat(source);
        
        // Use tolerance for floating-point comparison due to potential precision differences
        await Assert.That(Math.Abs(hyperlinqResult - linqResult)).IsLessThan(1e-6f);
    }
    
    public static IEnumerable<(Func<IEnumerable<int>> sourceFactory, int expectedSum, string description)> GetIntTestData()
    {
        yield return (() => new List<int> { 1, 2, 3, 4, 5 }, 15, "List with positive integers");
        yield return (() => new int[] { 10, 20, 30, 40, 50 }, 150, "Array with larger integers");
        yield return (() => new List<int>(), 0, "Empty list");
        yield return (() => new int[] { -1, -2, -3 }, -6, "Negative integers");
        yield return (() => Enumerable.Range(1, 10), 55, "Range 1-10");
    }
    
    public static IEnumerable<(Func<IEnumerable<double>> sourceFactory, double expectedSum, string description)> GetDoubleTestData()
    {
        yield return (() => new List<double> { 1.5, 2.5, 3.5 }, 7.5, "List with doubles");
        yield return (() => new double[] { 0.1, 0.2, 0.3 }, 0.6, "Small doubles");
        yield return (() => new List<double>(), 0.0, "Empty list");
    }
    
    public static IEnumerable<(Func<IEnumerable<long>> sourceFactory, long expectedSum, string description)> GetLongTestData()
    {
        yield return (() => new List<long> { 1L, 2L, 3L }, 6L, "List with longs");
        yield return (() => new long[] { 1000000L, 2000000L }, 3000000L, "Large longs");
        yield return (() => new List<long>(), 0L, "Empty list");
    }
    
    public static IEnumerable<(Func<IEnumerable<float>> sourceFactory, float expectedSum, string description)> GetFloatTestData()
    {
        yield return (() => new List<float> { 1.5f, 2.5f }, 4.0f, "List with floats");
        yield return (() => new float[] { 0.1f, 0.2f }, 0.3f, "Small floats");
        yield return (() => new List<float>(), 0.0f, "Empty list");
    }
}
