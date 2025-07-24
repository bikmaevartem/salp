using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salp.Core.Tensors.Memory
{
    public abstract class TensorMemoryBase<T> : ITensorMemory<T>
    {
        protected int[] _hostShape;

        protected T[] _hostData;

        protected int _hostLength;

        protected int[] _hostStrides;

        public virtual int[] HostShape => _hostShape;

        public virtual T[] HostData => _hostData;

        public virtual int HostLength => _hostLength;

        public virtual int[] HostStrides => _hostStrides;

        public virtual nint DeviceShape => throw new NotImplementedException();

        public virtual nint DeviceData => throw new NotImplementedException();

        public virtual nint DeviceLength => throw new NotImplementedException();

        public virtual nint DeviceStrides => throw new NotImplementedException();

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
