using System.Numerics;
using Salp.Core.Tensors.Memory;

namespace Salp.Core.Tensors
{
    public interface ITensor<T> where T : unmanaged, INumber<T>
    {
        int[] Shape { get; }

        T[] Data { get; }

        int Length { get; }

        int[] Strides { get; }

        ITensorMemory<T> Memory { get; }
    }
}
