using System.Numerics;
using Salp.Core.Extensions;

namespace Salp.Core.Tensors
{
    public abstract partial class TensorBase<T> : ITensor<T> where T : INumber<T>
    {
        private void AssertShapeIsValid(int[] shape)
        {
            if (shape.Any(d => d <= 0))
            {
                throw new ArgumentException("All dimensions must be greater than zero.");
            }
        }

        private void AssertShapeMatchesDataLength(int[] shape, T[] data)
        {
            AssertShapeIsValid(shape);

            int shapeProduct = shape.Product();

            if (shapeProduct != data.Length)
            {
                throw new ArgumentException($"Data length ({data.Length}) does not match shape product ({shapeProduct}) — expected data length: {string.Join(" x ", shape)} = {shapeProduct}.");
            }
        }

        private void AssertShapesAreEqual(int[] a, int[] b)
        {
            if (a.Length != b.Length || !a.SequenceEqual(b))
            {
                throw new InvalidOperationException($"Tensor shapes do not match. {string.Join("x", a)} and {string.Join("x", b)}");
            }
        }

        private void AssertDataLengthsAreEqual(T[] a, T[] b)
        {
            if (a.Length != b.Length)
            {
                throw new InvalidOperationException($"Data lengths do not match. {a.Length} and {b.Length}");
            }
        }
    }
}
