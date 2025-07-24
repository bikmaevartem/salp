using System.Numerics;
using Salp.Core.Tensors.Memory;

namespace Salp.Core.Tensors
{
    public class Tensor<T> : ITensor<T> where T : unmanaged, INumber<T>
    {
        private readonly ITensorMemory<T> _memory;

        public Tensor(ITensorMemory<T> memory)
        {
            _memory = memory;
        }

        public int[] Shape => Memory.HostShape;

        public T[] Data => Memory.HostData;

        public int Length => Memory.HostLength;

        public int[] Strides => Memory.HostStrides;

        public ITensorMemory<T> Memory => _memory;
    }
}
