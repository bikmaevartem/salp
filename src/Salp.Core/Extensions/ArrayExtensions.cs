using System.Numerics;

namespace Salp.Core.Extensions
{
    public static class ArrayExtensions
    {
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
    }
}
