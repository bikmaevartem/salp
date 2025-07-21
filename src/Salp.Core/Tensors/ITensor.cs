namespace Salp.Core.Tensors
{
    /// <summary>
    /// Tensor.
    /// </summary>
    /// <typeparam name="T">Type of data in the ITensor</typeparam>
    public interface ITensor<T>
    {
        /// <summary>
        /// Get full copy of shape.
        /// </summary>
        /// <returns>Shape of tensor.</returns>
        int[] CopyShape();

        /// <summary>
        /// Get full copy of data.
        /// </summary>
        /// <returns>Data of tensor.</returns>
        T[] CopyData();

        /// <summary>
        /// Creates a deep copy of the tensor, including shape and data.
        /// </summary>
        /// <returns>ITensor</returns>
        ITensor<T> Clone();

        #region Math

        /// <summary>
        /// Applies the specified binary function element-wise to this tensor and the given tensor.
        /// </summary>
        /// <param name="tensor">The second tensor to combine with.</param>
        /// <param name="func">A function that takes two elements (from this and the other tensor) and returns a combined value.</param>
        /// <returns>A new tensor with the function applied to each pair of elements.</returns>
        ITensor<T> Zip(ITensor<T> tensor, Func<T, T, T> func);

        /// <summary>
        /// Applies the specified function to each element of the tensor.
        /// </summary>
        /// <param name="func">A function that takes a single element and returns a transformed value.</param>
        /// <returns>A new tensor containing the result of the operation.</returns>
        ITensor<T> Map(Func<T, T> func);

        /// <summary>
        /// Performs element-wise addition with the given tensor.
        /// </summary>
        /// <param name="tensor">The tensor to add.</param>
        /// <returns>A new tensor containing the result of the operation.</returns>
        ITensor<T> Add(ITensor<T> tensor);

        /// <summary>
        /// Performs element-wise subtraction with the given tensor.
        /// </summary>
        /// <param name="tensor">The tensor to subtract.</param>
        /// <returns>A new tensor containing the result of the operation.</returns>
        ITensor<T> Sub(ITensor<T> tensor);

        /// <summary>
        /// Performs element-wise multiplication with the given tensor.
        /// </summary>
        /// <param name="tensor">The tensor to multiply with.</param>
        /// <returns>A new tensor containing the result of the operation.</returns>
        ITensor<T> Mul(ITensor<T> tensor);

        /// <summary>
        /// Performs element-wise division by the given tensor.
        /// </summary>
        /// <param name="tensor">The tensor to divide by.</param>
        /// <returns>A new tensor containing the result of the operation.</returns>
        ITensor<T> Div(ITensor<T> tensor);

        /// <summary>
        ///  Applies element-wise negation to the tensor.
        /// </summary>
        /// <returns>A new tensor containing the result of the operation.</returns>
        ITensor<T> Negate();
        #endregion
    }
}
