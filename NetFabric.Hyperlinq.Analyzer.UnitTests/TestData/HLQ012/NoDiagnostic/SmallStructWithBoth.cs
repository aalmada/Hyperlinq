using NetFabric.Hyperlinq;

namespace TestData.HLQ012.NoDiagnostic.SmallStructWithBoth;

struct SmallStructWithBoth : IFunction<int, int>, IFunctionIn<int, int>
{
    public long f1 = 0; // 8 bytes

    public SmallStructWithBoth() { }

    public int Invoke(int instance) => instance;
    public int Invoke(in int instance) => instance;
}
