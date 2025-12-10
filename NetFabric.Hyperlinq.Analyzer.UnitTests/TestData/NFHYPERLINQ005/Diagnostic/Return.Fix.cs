using NetFabric.Hyperlinq;

namespace System.Linq
{
    public static class EnumerableEx
    {
        public static System.Collections.Generic.IEnumerable<TResult> Return<TResult>(TResult value)
            => throw new NotImplementedException();
    }
}

class C
{
    void M()
    {
        var x = ValueEnumerable.Return(10);
    }
}
