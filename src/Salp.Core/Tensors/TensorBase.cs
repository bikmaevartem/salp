using System.Numerics;
using Salp.Core.Tensors.Memory;

namespace Salp.Core.Tensors
{
    public abstract class TensorBase<T> : ITensor<T> where T : unmanaged, INumber<T>
    {
        public int[] Shape => throw new NotImplementedException();

        public T[] Data => throw new NotImplementedException();

        public int Length => throw new NotImplementedException();

        public int[] Strides => throw new NotImplementedException();

        public ITensorMemory<T> Memory => throw new NotImplementedException();

        private int[] ComputeStrides(int[] shape)
        {
            int dimensions = shape.Length;
            int[] strides = new int[dimensions];

            // Last dimension has a step like 1, always.
            strides[dimensions - 1] = 1;

            for (int i = dimensions - 2; i >= 0; i--)
            {
                strides[i] = strides[i + 1] * shape[i + 1];
            }

            return strides;
        }
    }
}
