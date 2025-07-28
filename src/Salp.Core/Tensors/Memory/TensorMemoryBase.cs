using System.Numerics;
using Salp.Core.Extensions;

namespace Salp.Core.Tensors.Memory
{
    public abstract class TensorMemoryBase<T> : ITensorMemory<T> where T : unmanaged, INumber<T>
    {
        #region Factory

        public static ITensorMemory<T> Create(int[] shape)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Host

        public virtual int[] HostShape => throw new NotImplementedException();

        public virtual T[] HostData => throw new NotImplementedException();

        public virtual int HostLength => throw new NotImplementedException();

        public virtual int[] HostStrides => throw new NotImplementedException();

        #endregion

        #region Device

        public virtual nint DeviceShape => throw new NotImplementedException();

        public virtual nint DeviceData => throw new NotImplementedException();

        public virtual nint DeviceLength => throw new NotImplementedException();

        public virtual nint DeviceStrides => throw new NotImplementedException();

        #endregion

        public virtual ITensorMemory<T> Clone()
        {
            throw new NotImplementedException();
        }

        protected int[] ComputeStrides(int[] shape)
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

        protected int ComputeLength(int[] shape)
        {
            return shape.Product();
        }
    }
}
