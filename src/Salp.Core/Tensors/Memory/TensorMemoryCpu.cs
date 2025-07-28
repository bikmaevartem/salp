using System.Numerics;
using Salp.Core.Extensions;

namespace Salp.Core.Tensors.Memory
{
    public class TensorMemoryCpu<T> : TensorMemoryBase<T> where T : unmanaged, INumber<T>
    {
        private readonly int[] _hostShape;

        private readonly T[] _hostData;

        private readonly int[] _hostStrides;

        public TensorMemoryCpu(int[] shape, T[] data, int[] strides)
        {
            _hostShape = shape;
            _hostData = data;
            _hostStrides = strides;
        }

        #region Factory

        public new static ITensorMemory<T> Create(int[] shape)
        {
            return new TensorMemoryCpu<T>(shape);
        }


        private TensorMemoryCpu(int[] shape)
        {
            _hostShape = shape;
            _hostStrides = ComputeStrides(shape);
            _hostData = new T[ComputeLength(shape)];
        }

        #endregion

        #region Host

        public override int[] HostShape => _hostShape;

        public override T[] HostData => _hostData;

        public override int HostLength => _hostData.Length;

        public override int[] HostStrides => _hostStrides;

        #endregion

        #region Device

        public override nint DeviceShape => throw new NotImplementedException();

        public override nint DeviceData => throw new NotImplementedException();

        public override nint DeviceLength => throw new NotImplementedException();

        public override nint DeviceStrides => throw new NotImplementedException();

        #endregion 

        public override ITensorMemory<T> Clone()
        {
            return new TensorMemoryCpu<T>(shape: _hostShape.CloneExact(), data: _hostData.CloneExact(), strides: _hostStrides.CloneExact());
        }
    }
}
