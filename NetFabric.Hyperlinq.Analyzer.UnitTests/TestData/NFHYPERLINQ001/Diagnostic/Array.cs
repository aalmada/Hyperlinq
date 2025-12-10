using System.Linq;

namespace TestData.NFHYPERLINQ001.Diagnostic.Array;

class C
{
    void M()
    {
        var array = new int[0];
        var x = array.Select(i => i * 2);
    }
}
