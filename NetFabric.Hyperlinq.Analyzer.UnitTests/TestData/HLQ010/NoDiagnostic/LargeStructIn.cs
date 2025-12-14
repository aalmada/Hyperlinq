using NetFabric.Hyperlinq;

namespace TestData.HLQ010.NoDiagnostic.LargeStructIn;

struct LargeStructIn : IFunctionIn<int, int>
{
    public long f1 = 0, f2 = 0, f3 = 0, f4 = 0, f5 = 0, f6 = 0, f7 = 0, f8 = 0, f9 = 0, f10 = 0;

    public LargeStructIn() { }

    public int Invoke(in int instance) => instance;
}
