using NetFabric.Hyperlinq;

namespace TestData.NFHYPERLINQ005.NoDiagnostic;

class C
{
    void M()
    {
        var x = ValueEnumerable.Range(0, 10);
        var y = ValueEnumerable.Repeat(0, 10);
    }
}
