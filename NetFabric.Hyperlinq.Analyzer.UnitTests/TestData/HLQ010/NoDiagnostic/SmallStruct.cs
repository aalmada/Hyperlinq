using NetFabric.Hyperlinq;

namespace TestData.HLQ010.NoDiagnostic.SmallStruct;

struct SmallStruct : IFunction<int, int>
{
    public long f1 = 0; // 8 bytes

    public SmallStruct() { }

    public int Invoke(int instance) => instance;
}
