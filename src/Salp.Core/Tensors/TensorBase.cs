using System.Numerics;
using Salp.Core.Extensions;

namespace Salp.Core.Tensors
{
    public abstract partial class TensorBase<T> : ITensor<T> where T : INumber<T>
    {
        /// <summary>
        /// Data.
        /// </summary>
        protected readonly T[] _data;

        /// <summary>
        /// Shape of the Tensor.
        /// </summary>
        protected readonly int[] _shape;

        /// <summary>
        /// Need for correct access by index to elements in the Data.
        /// </summary>
        protected readonly int[] _strides;

        /// <summary>
        /// Creating new Tensor.
        /// </summary>
        /// <param name="shape">Shape of a new Tensor.</param>
        public TensorBase(int[] shape)
        {
            _data = new T[shape.Product()];
            _shape = shape;
            _strides = ComputeStrides(_shape);
        }

        /// <summary>
        /// Creating new Tensor.
        /// </summary>
        /// <param name="shape">Shape of a new Tensor.</param>
        /// <param name="data">Data of a new Tensor.</param>
        public TensorBase(int[] shape, T[] data)
        {
            AssertShapeMatchesDataLength(shape, data);


            _data = data;
            _shape = shape;
            _strides = ComputeStrides(_shape);
        }

        /// <summary>
        /// Get full copy of shape.
        /// </summary>
        /// <returns>Shape of tensor.</returns>
        public int[] CopyShape() => (int[])_shape.Clone();

        /// <summary>
        /// Get full copy of data.
        /// </summary>
        /// <returns>Data of tensor.</returns>
        public T[] CopyData() => (T[])_data.Clone();

        /// <summary>
        /// Creates a deep copy of the tensor, including shape and data.
        /// </summary>
        /// <returns>ITensor<T></returns>
        public virtual ITensor<T> Clone()
        {
            return CreateTensor(CopyShape(), CopyData());
        }

        protected abstract ITensor<T> CreateTensor(int[] shape, T[] data);

        private int[] ComputeStrides(int[] shape)
        {
            AssertShapeIsValid(shape);

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
