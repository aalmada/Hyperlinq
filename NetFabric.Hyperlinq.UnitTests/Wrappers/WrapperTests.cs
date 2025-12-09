using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NetFabric.Assertive;
using TUnit.Core;

namespace NetFabric.Hyperlinq.UnitTests.Wrappers;

public class WrapperTests
{
    public readonly struct EnumerableGetEnumeratorFn : IFunction<IEnumerable<int>, List<int>.Enumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<int>.Enumerator Invoke(IEnumerable<int> instance) => ((List<int>)instance).GetEnumerator();
    }

    public readonly struct CollectionGetEnumeratorFn : IFunction<ICollection<int>, List<int>.Enumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<int>.Enumerator Invoke(ICollection<int> instance) => ((List<int>)instance).GetEnumerator();
    }

    public readonly struct ListGetEnumeratorFn : IFunction<IList<int>, List<int>.Enumerator>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<int>.Enumerator Invoke(IList<int> instance) => ((List<int>)instance).GetEnumerator();
    }

    [Test]
    public void Extensions_Enumerable_AsValueEnumerable_Struct_Should_Succeed()
    {
        var source = new List<int> { 1, 2, 3 };
        IEnumerable<int> enumerable = source;
        var wrapper = enumerable.AsValueEnumerable<int, List<int>.Enumerator, EnumerableGetEnumeratorFn>(new EnumerableGetEnumeratorFn());

        wrapper.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(source);
    }

    [Test]
    public void Extensions_Enumerable_AsValueEnumerable_Func_Should_Succeed()
    {
        var source = new List<int> { 1, 2, 3 };
        IEnumerable<int> enumerable = source;
        var wrapper = enumerable.AsValueEnumerable<int, List<int>.Enumerator>(s => ((List<int>)s).GetEnumerator());

        wrapper.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(source);
    }
    
    [Test]
    public async Task Extensions_Collection_AsValueEnumerable_Struct_Should_Succeed()
    {
        var source = new List<int> { 1, 2, 3 };
        ICollection<int> collection = source;
        var wrapper = collection.AsValueEnumerable<int, List<int>.Enumerator, CollectionGetEnumeratorFn>(new CollectionGetEnumeratorFn());

        wrapper.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(source);
        
        await Assert.That(wrapper.Count).IsEqualTo(source.Count);
    }

    [Test]
    public async Task Extensions_Collection_AsValueEnumerable_Func_Should_Succeed()
    {
        var source = new List<int> { 1, 2, 3 };
        ICollection<int> collection = source;
        var wrapper = collection.AsValueEnumerable<int, List<int>.Enumerator>(s => ((List<int>)s).GetEnumerator());

        wrapper.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(source);
        
        await Assert.That(wrapper.Count).IsEqualTo(source.Count);
    }

    [Test]
    public async Task Extensions_List_AsValueEnumerable_Struct_Should_Succeed()
    {
        var source = new List<int> { 1, 2, 3 };
        IList<int> list = source;
        var wrapper = list.AsValueEnumerable<int, List<int>.Enumerator, ListGetEnumeratorFn>(new ListGetEnumeratorFn());

        wrapper.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(source);
        
        await Assert.That(wrapper.Count).IsEqualTo(source.Count);
        await Assert.That(wrapper[0]).IsEqualTo(source[0]);
    }

    [Test]
    public async Task Extensions_List_AsValueEnumerable_Func_Should_Succeed()
    {
        var source = new List<int> { 1, 2, 3 };
        IList<int> list = source;
        var wrapper = list.AsValueEnumerable<int, List<int>.Enumerator>(s => ((List<int>)s).GetEnumerator());

        wrapper.Must()
            .BeEnumerableOf<int>()
            .BeEqualTo(source);
        
        await Assert.That(wrapper.Count).IsEqualTo(source.Count);
        await Assert.That(wrapper[0]).IsEqualTo(source[0]);
    }
}

