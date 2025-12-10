using NetFabric.Hyperlinq;

namespace TestData.HLQ012.NoDiagnostic.SmallStructWithBoth;

struct SmallStructWithBoth : IFunction<int, int>, IFunctionIn<int, int>
{
    long f1; // 8 bytes

    public int Invoke(int instance) => instance;
    public int Invoke(in int instance) => instance;
}
