using NetFabric.Hyperlinq;

namespace TestData.HLQ010.Diagnostic.LargeStruct;

struct LargeStruct : IFunction<int, int>
{
    // 10 longs = 80 bytes
    long f1, f2, f3, f4, f5, f6, f7, f8, f9, f10;

    public int Invoke(int instance) => instance;
}
