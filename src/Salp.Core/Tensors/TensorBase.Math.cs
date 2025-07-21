using System.Numerics;

namespace Salp.Core.Tensors
{
    public abstract partial class TensorBase<T> : ITensor<T> where T : INumber<T>
    {
        /// <summary>
        /// Applies the specified binary function element-wise to this tensor and the given tensor.
        /// </summary>
        /// <param name="tensor">The second tensor to combine with.</param>
        /// <param name="func">A function that takes two elements (from this and the other tensor) and returns a combined value.</param>
        /// <returns>A new tensor with the function applied to each pair of elements.</returns>
        public virtual ITensor<T> Zip(ITensor<T> tensor, Func<T, T, T> func)
        {
            int[] otherShape = tensor.CopyShape();
            T[] otherData = tensor.CopyData();

            AssertShapesAreEqual(_shape, otherShape);
            AssertDataLengthsAreEqual(_data, otherData);

            for (int i = 0; i < _data.Length; i++)
            {
                otherData[i] = func(_data[i], otherData[i]);
            }

            return CreateTensor(otherShape, otherData);
        }

        /// <summary>
        /// Applies the specified function to each element of the tensor.
        /// </summary>
        /// <param name="func">A function that takes a single element and returns a transformed value.</param>
        /// <returns>A new tensor containing the result of the operation.</returns>
        public virtual ITensor<T> Map(Func<T, T> func)
        {
            int[] shape = CopyShape();
            T[] data = CopyData();

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = func(data[i]);
            }

            return CreateTensor(shape, data);
        }

        /// <summary>
        /// Performs element-wise addition with the given tensor.
        /// </summary>
        /// <param name="tensor">The tensor to add.</param>
        /// <returns>A new tensor containing the result of the operation.</returns>
        public virtual ITensor<T> Add(ITensor<T> tensor)
        {
            return Zip(tensor, (a, b) => a + b);
        }

        /// <summary>
        /// Performs element-wise subtraction with the given tensor.
        /// </summary>
        /// <param name="tensor">The tensor to subtract.</param>
        /// <returns>A new tensor containing the result of the operation.</returns>
        public virtual ITensor<T> Sub(ITensor<T> tensor)
        {
            return Zip(tensor, (a, b) => a - b);
        }

        /// <summary>
        /// Performs element-wise multiplication with the given tensor.
        /// </summary>
        /// <param name="tensor">The tensor to multiply with.</param>
        /// <returns>A new tensor containing the result of the operation.</returns>
        public virtual ITensor<T> Mul(ITensor<T> tensor)
        {
            return Zip(tensor, (a, b) => a * b);
        }

        /// <summary>
        /// Performs element-wise division by the given tensor.
        /// </summary>
        /// <param name="tensor">The tensor to divide by.</param>
        /// <returns>A new tensor containing the result of the operation.</returns>
        public virtual ITensor<T> Div(ITensor<T> tensor)
        {
            return Zip(tensor, (a, b) => a / b);
        }

        /// <summary>
        ///  Applies element-wise negation to the tensor.
        /// </summary>
        /// <returns>A new tensor containing the result of the operation.</returns>
        public virtual ITensor<T> Negate()
        {
            return Map(a => -a);
        }
    }
}
