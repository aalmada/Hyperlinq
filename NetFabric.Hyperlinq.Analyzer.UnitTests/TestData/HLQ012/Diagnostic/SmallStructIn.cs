using NetFabric.Hyperlinq;

namespace TestData.HLQ012.Diagnostic.SmallStructIn;

struct SmallStructIn : IFunctionIn<int, int>
{
    public long f1 = 0; // 8 bytes

    public SmallStructIn() { }

    public int Invoke(in int instance) => instance;
}
