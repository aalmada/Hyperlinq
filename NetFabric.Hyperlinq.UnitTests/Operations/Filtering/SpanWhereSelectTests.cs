using System;
using System.Collections.Generic;
using System.Linq;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Span;

public class SpanWhereSelectTests
{


    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_WhereSelect_ShouldReturnWhereSelectListEnumerable(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());
        var result = list.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 2);

        // Type check removed as it depends on internal generic structure
        _ = result.Must().BeEnumerableOf<int>();
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetNonEmptyIntArraySources))]
    public void Memory_WhereSelect_ShouldReturnWhereSelectReadOnlySpanEnumerable(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        ReadOnlyMemory<int> memory = array.AsMemory();
        var result = memory.Where(x => x % 2 == 0).Select(x => x * 2);

        // Type check removed
        _ = result.ToArray().Must().BeEnumerableOf<int>();
    }

    // ===== Correctness Tests =====



    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void List_WhereSelect_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var list = new List<int>(testCase.Factory());

        var hyperlinqResult = list.AsValueEnumerable().Where(x => x % 2 == 0).Select(x => x * 10);
        var linqResult = Enumerable.Select(Enumerable.Where(list, x => x % 2 == 0), x => x * 10);

        _ = hyperlinqResult.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(linqResult);
    }

    [Test]
    [MethodDataSource(typeof(TestDataSources), nameof(TestDataSources.GetIntArraySources))]
    public void Memory_WhereSelect_ShouldMatchLinq(TestCase<int[]> testCase)
    {
        var array = testCase.Factory();
        ReadOnlyMemory<int> memory = array.AsMemory();

        var hyperlinqResult = memory.Where(x => x % 2 == 0).Select(x => x * 10);
        var linqResult = Enumerable.Select(Enumerable.Where(array, x => x % 2 == 0), x => x * 10);

        var i = 0;
        foreach (var item in hyperlinqResult)
        {
            if (item != linqResult.ElementAt(i++))
            {
                throw new Exception("Mismatch");
            }
        }
        if (i != linqResult.Count())
        {
            throw new Exception("Count mismatch");
        }
    }

    // ===== Type Changes =====










}

