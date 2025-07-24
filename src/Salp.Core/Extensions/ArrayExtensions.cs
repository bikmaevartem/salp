using System.Numerics;

namespace Salp.Core.Extensions
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Calculates the product of all elements in the array.
        /// </summary>
        /// <typeparam name="T">The numeric type of the elements. Must implement <see cref="INumber{T}"/>.</typeparam>
        /// <param name="array">The array of numeric elements to multiply.</param>
        /// <returns>
        /// The product of all elements in the array.  
        /// Returns <c>T.Zero</c> if the array is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is <c>null</c>.</exception>
        public static T Product<T>(this T[] array) where T : INumber<T>
        {
            if (array == null)
            {  
                throw new ArgumentNullException("array");
            }
            
            if (array.Length == 0)
            {
                return T.Zero;
            }

            var result = T.One;

            foreach (var item in array)
            {
                result *= item;
            }

            return result;
        }


        #region Copy

        private const long UnsafeByteThreshold = 4096; // 4 KB

        /// <summary>
        /// Creates a new array that is an exact copy of the source array, with independent memory.
        /// </summary>
        /// <typeparam name="T">The element type of the array. Must be unmanaged.</typeparam>
        /// <param name="array">The source array to copy.</param>
        /// <returns>
        /// A new array with the same length and values as the source array. 
        /// Modifications to the returned array will not affect the original array.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the source array is null.</exception>
        public static unsafe T[] Copy<T>(this T[] array) where T: unmanaged
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length * sizeof(T) > UnsafeByteThreshold)
            {
                return array.MemoryCopy();
            }

            return array.ArrayCopy();
        }

        private static T[] ArrayCopy<T>(this T[] array)
        {
            T[] result = new T[array.Length];

            Array.Copy(array, result, array.Length);

            return result;
        }

        private static unsafe T[] MemoryCopy<T>(this T[] array)
        {
            T[] result = new T[array.Length];
            
            fixed (T* pSrc = array)
            fixed (T* pDst = result)
            {
                long byteCount = array.Length * sizeof(T);
                Buffer.MemoryCopy(pSrc, pDst, byteCount, byteCount);
            }

            return result;
        }

        #endregion
    }
}
