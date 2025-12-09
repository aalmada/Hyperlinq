using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Wrappers;

public class WrapperTests
{
    public readonly struct GetEnumeratorFn : IFunction<List<int>, List<int>.Enumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<int>.Enumerator Invoke(List<int> instance) => instance.GetEnumerator();
    }

    [Test]
    public void ValueEnumerableWrapper_Should_Succeed()
    {
        var source = new List<int> { 1, 2, 3 };
        var wrapper = new ValueEnumerableWrapper<List<int>, List<int>.Enumerator, GetEnumeratorFn, int>(source, new GetEnumeratorFn());

        wrapper.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(source);
    }

    [Test]
    public async Task ValueReadOnlyCollectionWrapper_Should_Succeed()
    {
        var source = new List<int> { 1, 2, 3 };
        var wrapper = new ValueReadOnlyCollectionWrapper<List<int>, List<int>.Enumerator, GetEnumeratorFn, int>(source, new GetEnumeratorFn());

        wrapper.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(source);
        
        // Assert Count
        await Assert.That(wrapper.Count).IsEqualTo(source.Count);
    }

    [Test]
    public async Task ValueReadOnlyListWrapper_Should_Succeed()
    {
        var source = new List<int> { 1, 2, 3 };
        var wrapper = new ValueReadOnlyListWrapper<List<int>, List<int>.Enumerator, GetEnumeratorFn, int>(source, new GetEnumeratorFn());

        wrapper.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(source);

        // Assert Count and Indexer
        await Assert.That(wrapper.Count).IsEqualTo(source.Count);
        await Assert.That(wrapper[0]).IsEqualTo(source[0]);
    }

    [Test]
    public void Extensions_ValueEnumerableWrapper_Should_Succeed()
    {
        var source = new List<int> { 1, 2, 3 };
        var wrapper = source.AsValueEnumerable<List<int>, List<int>.Enumerator, GetEnumeratorFn, int>(new GetEnumeratorFn());

        wrapper.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(source);
    }

    [Test]
    public void Extensions_ValueEnumerableWrapper_Func_Should_Succeed()
    {
        var source = new List<int> { 1, 2, 3 };
        var wrapper = source.AsValueEnumerable<List<int>, List<int>.Enumerator, int>(s => s.GetEnumerator());

        wrapper.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(source);
    }
}
