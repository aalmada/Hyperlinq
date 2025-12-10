using NetFabric.Hyperlinq;

namespace TestData.HLQ011.Diagnostic.Lambda;

class C
{
    void M()
    {
        var array = new int[0];
        var x = array.AsValueEnumerable().Where(new GeneratedFunction());
    }

    private readonly struct GeneratedFunction : NetFabric.Hyperlinq.IFunction<int, bool>
    {
        public bool Invoke(int x) => x > 0;
    }
}
