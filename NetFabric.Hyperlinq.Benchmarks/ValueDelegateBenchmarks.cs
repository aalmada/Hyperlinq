using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace NetFabric.Hyperlinq.Benchmarks
{
    public class ValueDelegateBenchmarks
    {
        int[] source;

        [Params(10_000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            source = Enumerable.Range(0, Count).ToArray();
        }

        readonly struct DoubleFn : IFunction<int, int>
        {
            public int Invoke(int item) => item * 2;
        }

        readonly struct IsEvenFn : IFunction<int, bool>
        {
            public bool Invoke(int item) => item % 2 == 0;
        }

        [Benchmark(Baseline = true)]
        public int Linq_Select_Sum()
            => Enumerable.Sum(Enumerable.Select(source, item => item * 2));

        [Benchmark]
        public int Hyperlinq_Select_Sum()
            => source.AsValueEnumerable().Select(item => item * 2).Sum();

        [Benchmark]
        public int Hyperlinq_ValueDelegate_Select_Sum()
            => source.AsValueEnumerable().Select<int, int, DoubleFn>(new DoubleFn()).Sum();

        [Benchmark]
        public int Linq_Where_Sum()
            => Enumerable.Sum(Enumerable.Where(source, item => item % 2 == 0));

        [Benchmark]
        public int Hyperlinq_Where_Sum()
            => source.AsValueEnumerable().Where(item => item % 2 == 0).Sum();

        [Benchmark]
        public int Hyperlinq_ValueDelegate_Where_Sum()
            => source.AsValueEnumerable().Where<int, IsEvenFn>(new IsEvenFn()).Sum();

        readonly struct DoubleFnIn : IFunctionIn<int, int>
        {
            public int Invoke(in int item) => item * 2;
        }

        [Benchmark]
        public int Hyperlinq_ValueDelegateIn_Select_Sum()
            => source.AsValueEnumerable().Select<int, int, DoubleFnIn>(new DoubleFnIn()).Sum();

        readonly struct IsEvenFnIn : IFunctionIn<int, bool>
        {
            public bool Invoke(in int item) => item % 2 == 0;
        }

        [Benchmark]
        public int Hyperlinq_ValueDelegateIn_Where_Sum()
            => source.AsValueEnumerable().Where<int, IsEvenFnIn>(new IsEvenFnIn()).Sum();

        // ---------------------------------------------------------------------
        // Large Struct Benchmarks
        // ---------------------------------------------------------------------

        public struct LargeStruct
        {
            public long L1, L2, L3, L4; // 32 bytes
        }

        LargeStruct[] largeSource;

        [GlobalSetup(Targets = new[] { nameof(LargeStruct_ValueDelegate_Select), nameof(LargeStruct_ValueDelegateIn_Select) })]
        public void LargeStructSetup()
        {
            largeSource = new LargeStruct[Count];
        }

        readonly struct LargeStructFn : IFunction<LargeStruct, int>
        {
            public int Invoke(LargeStruct item) => (int)(item.L1 + item.L2);
        }

        readonly struct LargeStructFnIn : IFunctionIn<LargeStruct, int>
        {
            public int Invoke(in LargeStruct item) => (int)(item.L1 + item.L2);
        }

        [Benchmark]
        public int LargeStruct_ValueDelegate_Select()
            => largeSource.AsValueEnumerable().Select<LargeStruct, int, LargeStructFn>(new LargeStructFn()).Sum();

        [Benchmark]
        public int LargeStruct_ValueDelegateIn_Select()
            => largeSource.AsValueEnumerable().Select<LargeStruct, int, LargeStructFnIn>(new LargeStructFnIn()).Sum();


    }
}
