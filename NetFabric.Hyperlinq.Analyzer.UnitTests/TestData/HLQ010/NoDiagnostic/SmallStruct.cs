using NetFabric.Hyperlinq;

namespace TestData.HLQ010.NoDiagnostic.SmallStruct;

struct SmallStruct : IFunction<int, int>
{
    long f1; // 8 bytes

    public int Invoke(int instance) => instance;
}
