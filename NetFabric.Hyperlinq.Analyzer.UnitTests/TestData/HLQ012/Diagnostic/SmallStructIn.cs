using NetFabric.Hyperlinq;

namespace TestData.HLQ012.Diagnostic.SmallStructIn;

struct SmallStructIn : IFunctionIn<int, int>
{
    long f1; // 8 bytes

    public int Invoke(in int instance) => instance;
}
