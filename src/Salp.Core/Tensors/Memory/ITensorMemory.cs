using System.Numerics;

namespace Salp.Core.Tensors.Memory
{
    public interface ITensorMemory<T> where T : unmanaged, INumber<T>
    {
        #region Factory

        static abstract ITensorMemory<T> Create(int[] shape);

        #endregion

        #region Host

        int[] HostShape { get; }

        T[] HostData { get; }

        int HostLength { get; }

        int[] HostStrides { get; }

        #endregion

        #region Device

        nint DeviceShape { get; }

        nint DeviceData { get; }

        nint DeviceLength { get; }

        nint DeviceStrides { get; }

        #endregion

        ITensorMemory<T> Clone();
    }
}
