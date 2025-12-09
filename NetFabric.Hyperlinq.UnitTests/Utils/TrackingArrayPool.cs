using System;
using System.Buffers;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq.UnitTests
{
    public class TrackingArrayPool<T> : ArrayPool<T>
    {
        public List<(int minimumLength, T[] array)> Rents { get; } = new();
        public List<(T[] array, bool clearArray)> Returns { get; } = new();

        public override T[] Rent(int minimumLength)
        {
            var array = new T[minimumLength];
            Rents.Add((minimumLength, array));
            return array;
        }

        public override void Return(T[] array, bool clearArray = false)
        {
            Returns.Add((array, clearArray));
        }
    }
}
