using System;
using System.Numerics;
using Salp.Core.Extensions;
using Salp.Core.Tensors;
using Salp.Core.Tensors.Memory;

namespace Salp.Core.Devices
{
    public class DeviceCPU<T> : IDevice<T> where T : unmanaged, INumber<T>
    {
        private const int MIN_LENGTH_FOR_PARALLELISM = 10000;

        #region Common function

        /// <summary>
        /// Applies a unary element-wise function to the input tensor <paramref name="a"/> and returns a new tensor
        /// containing the results. The input tensor <paramref name="a"/> remains unchanged.
        /// </summary>
        /// <param name="a">The input tensor to apply the function on.</param>
        /// <param name="func">A unary function to apply to each element of <paramref name="a"/>.</param>
        /// <returns>A new tensor containing the results of applying <paramref name="func"/> to each element of <paramref name="a"/>.</returns>
        public ITensor<T> Map(ITensor<T> a, Func<T, T> func)
        {
            return Map(isInPlace: false, a, func);
        }

        /// <summary>
        /// Applies a unary element-wise function to the input tensor <paramref name="a"/> and modifies it in place.
        /// The input tensor <paramref name="a"/> is updated with the results.
        /// </summary>
        /// <param name="a">The tensor to modify in place.</param>
        /// <param name="func">A unary function to apply to each element of <paramref name="a"/>.</param>
        /// <returns>The modified tensor <paramref name="a"/> after applying the function.</returns>
        public ITensor<T> MapInPlace(ITensor<T> a, Func<T, T> func)
        {
            return Map(isInPlace: true, a, func);
        }

        protected virtual ITensor<T> Map(bool isInPlace, ITensor<T> a, Func<T, T> func)
        {
            int fromInclusive = 0;
            int toExclusive = a.Memory.HostLength;
            ITensor<T> result = isInPlace ? a : a.Clone();

            if (a.Memory.HostLength >= MIN_LENGTH_FOR_PARALLELISM)
            {
                Parallel.For(fromInclusive, toExclusive, (i) =>
                {
                    result.Memory.HostData[i] = func(result.Memory.HostData[i]);
                });
            }
            else
            {
                for (int i = 0; i < a.Memory.HostLength; i++)
                {
                    result.Memory.HostData[i] = func(result.Memory.HostData[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Applies a binary element-wise function to the input tensors <paramref name="a"/> and <paramref name="b"/>
        /// and returns a new tensor containing the results. The input tensors remain unchanged.
        /// </summary>
        /// <param name="a">The first input tensor.</param>
        /// <param name="b">The second input tensor. Must have the same shape as <paramref name="a"/> or be broadcastable.</param>
        /// <param name="func">A binary function to apply to each pair of elements from <paramref name="a"/> and <paramref name="b"/>.</param>
        /// <returns>A new tensor containing the results of applying <paramref name="func"/> element-wise to <paramref name="a"/> and <paramref name="b"/>.</returns>
        public ITensor<T> Zip(ITensor<T> a, ITensor<T> b, Func<T, T, T> func)
        {
            return Zip(isInPlace: false, a, b, func);
        }

        /// <summary>
        /// Applies a binary element-wise function to the input tensors <paramref name="a"/> and <paramref name="b"/>
        /// and modifies tensor <paramref name="a"/> in place with the results. The tensor <paramref name="b"/> remains unchanged.
        /// </summary>
        /// <param name="a">The tensor to modify in place. Must have the same shape as <paramref name="b"/> or be broadcastable.</param>
        /// <param name="b">The second input tensor, used as a read-only operand.</param>
        /// <param name="func">A binary function to apply to each pair of elements from <paramref name="a"/> and <paramref name="b"/>.</param>
        /// <returns>The modified tensor <paramref name="a"/> after applying the function element-wise.</returns>
        public ITensor<T> ZipInPlace(ITensor<T> a, ITensor<T> b, Func<T, T, T> func)
        {
            return Zip(isInPlace: true, a, b, func);
        }

        private ITensor<T> Zip(bool isInPlace, ITensor<T> a, ITensor<T> b, Func<T, T, T> func)
        {
            int fromInclusive = 0;
            int toExclusive = a.Memory.HostLength;
            ITensor<T> result = isInPlace ? a : a.Clone();

            if (a.Memory.HostLength >= MIN_LENGTH_FOR_PARALLELISM)
            {
                Parallel.For(fromInclusive, toExclusive, (i) =>
                {
                    result.Memory.HostData[i] = func(result.Memory.HostData[i], b.Memory.HostData[i]);
                });
            }
            else
            {
                for (int i = 0; i < a.Memory.HostLength; i++)
                {
                    result.Memory.HostData[i] = func(result.Memory.HostData[i], b.Memory.HostData[i]);
                }
            }

            return result;
        }


        #endregion

        #region Element by Element

        /// <summary>
        /// Returns a new tensor that is the element-wise sum of tensors <paramref name="a"/> and <paramref name="b"/>.
        /// The input tensors are not modified.
        /// </summary>
        /// <param name="a">The first input tensor.</param>
        /// <param name="b">The second input tensor. Must have the same shape or be broadcastable to the shape of <paramref name="a"/>.</param>
        /// <returns>A new tensor containing the element-wise sum of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public ITensor<T> Add(ITensor<T> a, ITensor<T> b)
        {
            return Zip(a, b, (x, y) => x + y);
        }

        /// <summary>
        /// Adds tensor <paramref name="b"/> to tensor <paramref name="a"/> element-wise, modifying <paramref name="a"/> in place.
        /// </summary>
        /// <param name="a">The tensor to be modified. Must have the same shape or be broadcastable to <paramref name="b"/>.</param>
        /// <param name="b">The tensor to add. Remains unchanged.</param>
        /// <returns>The modified tensor <paramref name="a"/> after addition.</returns>
        public ITensor<T> AddInPlace(ITensor<T> a, ITensor<T> b)
        {
            return ZipInPlace(a, b, (x, y) => x + y);
        }

        /// <summary>
        /// Returns a new tensor that is the element-wise difference of tensors <paramref name="a"/> and <paramref name="b"/>.
        /// The input tensors are not modified.
        /// </summary>
        /// <param name="a">The tensor to subtract from.</param>
        /// <param name="b">The tensor to subtract. Must have the same shape or be broadcastable to <paramref name="a"/>.</param>
        /// <returns>A new tensor containing the element-wise difference of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public ITensor<T> Subtract(ITensor<T> a, ITensor<T> b)
        {
            return Zip(a, b, (x, y) => x - y);
        }

        /// <summary>
        /// Subtracts tensor <paramref name="b"/> from tensor <paramref name="a"/> element-wise, modifying <paramref name="a"/> in place.
        /// </summary>
        /// <param name="a">The tensor to be modified. Must have the same shape or be broadcastable to <paramref name="b"/>.</param>
        /// <param name="b">The tensor to subtract. Remains unchanged.</param>
        /// <returns>The modified tensor <paramref name="a"/> after subtraction.</returns>
        public ITensor<T> SubtractInPlace(ITensor<T> a, ITensor<T> b)
        {
            return ZipInPlace(a, b, (x, y) => x - y);
        }

        /// <summary>
        /// Returns a new tensor that is the element-wise product of tensors <paramref name="a"/> and <paramref name="b"/>.
        /// The input tensors are not modified.
        /// </summary>
        /// <param name="a">The first input tensor.</param>
        /// <param name="b">The second input tensor. Must have the same shape or be broadcastable.</param>
        /// <returns>A new tensor containing the element-wise product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        public ITensor<T> Multiply(ITensor<T> a, ITensor<T> b)
        {
            return Zip(a, b, (x, y) => x * y);
        }

        /// <summary>
        /// Multiplies tensor <paramref name="a"/> by tensor <paramref name="b"/> element-wise, modifying <paramref name="a"/> in place.
        /// </summary>
        /// <param name="a">The tensor to be modified. Must have the same shape or be broadcastable to <paramref name="b"/>.</param>
        /// <param name="b">The tensor to multiply by. Remains unchanged.</param>
        /// <returns>The modified tensor <paramref name="a"/> after multiplication.</returns>
        public ITensor<T> MultiplyInPlace(ITensor<T> a, ITensor<T> b)
        {
            return ZipInPlace(a, b, (x, y) => x * y);
        }

        /// <summary>
        /// Returns a new tensor that is the element-wise division of tensors <paramref name="a"/> by <paramref name="b"/>.
        /// The input tensors are not modified.
        /// </summary>
        /// <param name="a">The dividend tensor.</param>
        /// <param name="b">The divisor tensor. Must have the same shape or be broadcastable.</param>
        /// <returns>A new tensor containing the element-wise division of <paramref name="a"/> by <paramref name="b"/>.</returns>
        public ITensor<T> Divide(ITensor<T> a, ITensor<T> b)
        {
            return Zip(a, b, (x, y) => x / y);
        }

        /// <summary>
        /// Divides tensor <paramref name="a"/> by tensor <paramref name="b"/> element-wise, modifying <paramref name="a"/> in place.
        /// </summary>
        /// <param name="a">The tensor to be modified. Must have the same shape or be broadcastable to <paramref name="b"/>.</param>
        /// <param name="b">The divisor tensor. Remains unchanged.</param>
        /// <returns>The modified tensor <paramref name="a"/> after division.</returns>
        public ITensor<T> DivideInPlace(ITensor<T> a, ITensor<T> b)
        {
            return ZipInPlace(a, b, (x, y) => x / y);
        }

        #endregion

        #region Unary operations

        /// <summary>
        /// Returns a new tensor where each element is the arithmetic negation of the corresponding element in <paramref name="a"/>.
        /// </summary>
        /// <param name="a">The input tensor.</param>
        /// <returns>A new tensor containing the negated values of <paramref name="a"/>.</returns>
        public ITensor<T> Negate(ITensor<T> a) // -a
        {
            return Map(a, x => -x);
        }

        /// <summary>
        /// Performs in-place arithmetic negation of each element in <paramref name="a"/>.
        /// </summary>
        /// <param name="a">The input tensor to modify.</param>
        /// <returns>The same tensor instance with negated values.</returns>
        public ITensor<T> NegateInPlace(ITensor<T> a) // -a
        {
            return MapInPlace(a, x => -x);
        }

        /// <summary>
        /// Returns a new tensor where each element is the absolute value of the corresponding element in <paramref name="a"/>.
        /// </summary>
        /// <param name="a">The input tensor.</param>
        /// <returns>A new tensor containing the absolute values of <paramref name="a"/>.</returns>
        public ITensor<T> Abs(ITensor<T> a)
        {
            return Map(a, x => T.Abs(x));
        }

        /// <summary>
        /// Performs in-place absolute value operation on each element in <paramref name="a"/>.
        /// </summary>
        /// <param name="a">The input tensor to modify.</param>
        /// <returns>The same tensor instance with absolute values.</returns>
        public ITensor<T> AbsInPlace(ITensor<T> a) // |a|
        {
            return MapInPlace(a, x => T.Abs(x));
        }

        #endregion

        #region Reductions

        /// <summary>
        /// Calculates the sum of all elements in the tensor.
        /// </summary>
        /// <param name="a">Input tensor.</param>
        /// <returns>The total sum of all elements.</returns>
        public T Sum(ITensor<T> a)
        {
            return a.Memory.HostData.Sum();
        }

        /// <summary>
        /// Calculates the arithmetic mean of all elements in the tensor.
        /// </summary>
        /// <param name="a">Input tensor.</param>
        /// <returns>The average value of all elements.</returns>
        public T Mean(ITensor<T> a)
        {
            return  a.Memory.HostData.Sum() / T.CreateChecked(a.Memory.HostLength);
        }

        /// <summary>
        /// Finds the maximum element in the tensor.
        /// </summary>
        /// <param name="a">Input tensor.</param>
        /// <returns>The maximum value among all elements.</returns>
        public T Max(ITensor<T> a)// Максимальный элемент
        {
            return a.Memory.HostData.Max();
        }

        /// <summary>
        /// Finds the minimum element in the tensor.
        /// </summary>
        /// <param name="a">Input tensor.</param>
        /// <returns>The minimum value among all elements.</returns>
        public T Min(ITensor<T> a)// Минимальный элемент
        {
            return a.Memory.HostData.Min();
        }

        #endregion

        #region Equal

        /// <summary>
        /// Determines whether all corresponding elements of tensor <paramref name="a"/> and tensor <paramref name="b"/> are equal.
        /// </summary>
        /// <param name="a">The first tensor.</param>
        /// <param name="b">The second tensor.</param>
        /// <returns>True if all elements are equal; otherwise, false.</returns>
        public bool Equal(ITensor<T> a, ITensor<T> b)// Элементы a == b
        {
            if (Object.ReferenceEquals(a, b))
            {
                return true;
            }


            bool result = a.Memory.HostLength == b.Memory.HostLength;

            if (result)
            {
                result = a.Memory.HostShape.SequenceEqual(b.Memory.HostShape);
            }

            if (result)
            {
                // TODO Parallel?
                result = a.Memory.HostData.SequenceEqual(b.Memory.HostData);
            }


            return result;
        }

        #endregion

        #region Creating Tensors

        public ITensor<T> CreateTensor(int[] shape)
        {
            ITensorMemory<T> memory = TensorMemoryCpu<T>.Create(shape);
            return Tensor<T>.CreateTensor(memory);
        }

        #endregion
    }
}
