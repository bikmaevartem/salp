namespace Salp.Core.Tensors.Memory
{
    public interface ITensorMemory<T>
    {
        int[] HostShape { get; }

        T[] HostData { get; }

        int Hostength { get; }

        int[] HostStrides { get; }


        nint DeviceShape { get; }

        nint DeviceData { get; }

        nint DeviceLength { get; }

        nint DeviceStrides { get; }
    }
}
