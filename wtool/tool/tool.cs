using System.Runtime.InteropServices;

namespace whook.tool;

public class tool
{
    public static T ByteArrayToStruct<T>(byte[] bytes) where T : struct
    {
        T obj = default;

        // 分配内存
        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));
        try
        {
            // 将字节数组拷贝到分配的内存
            Marshal.Copy(bytes, 0, ptr, bytes.Length);

            // 从内存中将数据结构化为 T
            obj = Marshal.PtrToStructure<T>(ptr);
        } finally
        {
            // 释放内存
            Marshal.FreeHGlobal(ptr);
        }

        return obj;
    }

    public static byte[] StructToBytes<T>(T obj) where T : struct
    {
        var size = Marshal.SizeOf(typeof(T));
        var bytes = new byte[size];

        var ptr = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, bytes, 0, size);
        } finally
        {
            Marshal.FreeHGlobal(ptr);
        }

        return bytes;
    }
}
