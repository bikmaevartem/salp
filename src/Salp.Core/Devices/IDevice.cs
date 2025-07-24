using System.Numerics;
using Salp.Core.Tensors;

namespace Salp.Core.Devices
{
    public interface IDevice<T> where T : unmanaged, INumber<T>
    {
        ITensor<T> Zip(ITensor<T> a, ITensor<T> b, Func<T, T, T> func);

        ITensor<T> ZipInPlace(ITensor<T> a, ITensor<T> b, Func<T, T, T> func);

        ITensor<T> Map(ITensor<T> a, Func<T, T> func);
        
        ITensor<T> MapInPlace(ITensor<T> a, Func<T, T> func);
    }
}
