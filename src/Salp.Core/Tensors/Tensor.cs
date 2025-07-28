using System.Numerics;
using Salp.Core.Tensors.Memory;

namespace Salp.Core.Tensors
{
    public class Tensor<T> : ITensor<T> where T : unmanaged, INumber<T>
    {
        public static ITensor<T> CreateTensor(ITensorMemory<T> memory)
        {
            return new Tensor<T>(memory);
        }

        private readonly ITensorMemory<T> _memory;

        private Tensor(ITensorMemory<T> memory)
        {
            _memory = memory;
        }

        public int[] Shape => Memory.HostShape;

        public T[] Data => Memory.HostData;

        public int Length => Memory.HostLength;

        public int[] Strides => Memory.HostStrides;

        public ITensorMemory<T> Memory => _memory;

        public ITensor<T> Clone()
        {
            ITensor<T> t = Tensor<T>.CreateTensor(memory: _memory.Clone());
            return t;
        }
    }
}
