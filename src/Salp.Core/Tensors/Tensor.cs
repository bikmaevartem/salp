
using System.Numerics;

namespace Salp.Core.Tensors
{
    public class Tensor<T> : TensorBase<T> where T : INumber<T>
    {
        /// <summary>
        /// Creating new Tensor.
        /// </summary>
        /// <param name="shape">Shape of a new Tensor.</param>
        public Tensor(int[] shape) : base(shape) { }

        /// <summary>
        /// Creating new Tensor.
        /// </summary>
        /// <param name="shape">Shape of a new Tensor.</param>
        /// <param name="data">Data of a new Tensor.</param>
        public Tensor(int[] shape, T[] data) : base(shape, data) { }

        /// <summary>
        /// Returns a string representation of the tensor, including its shape and a preview of the data.
        /// </summary>
        /// <returns>A string that describes the tensor.</returns>
        public override string ToString()
        {
            return $"Tensor<{typeof(T).Name}>[{string.Join("x", _shape)}]";
        }

        protected override ITensor<T> CreateTensor(int[] shape, T[] data)
        {
            return new Tensor<T>(shape, data);
        }
    }
}
