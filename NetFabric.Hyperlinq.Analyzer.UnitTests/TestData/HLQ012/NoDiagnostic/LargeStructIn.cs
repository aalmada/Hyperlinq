using NetFabric.Hyperlinq;

namespace TestData.HLQ012.NoDiagnostic.LargeStructIn;

struct LargeStructIn : IFunctionIn<int, int>
{
    // 10 longs = 80 bytes
    long f1, f2, f3, f4, f5, f6, f7, f8, f9, f10;

    public int Invoke(in int instance) => instance;
}
