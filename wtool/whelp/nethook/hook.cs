using System.Runtime.InteropServices;

namespace whook.whelp.nethook;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct IPC
{
    // 前2字节是opcode，使用ushort类型
    public ushort Opcode;

    // 3到8字节填充，使用byte数组
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] Padding1;

    // 9到12字节是时间戳，使用uint类型
    public uint Timestamp;

    // 13到32字节填充，使用byte数组
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
    public byte[] Padding2;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct 进本
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
    public byte[] Padding1;

    public byte flag1;
    public byte flag2;
    public byte flag3;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] Padding2;

    public byte flag4;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
    public byte[] Padding3;

    public Int16 id;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] Padding4;

    public 进本()
    {
        Padding3 = new byte[9] { 0xEA, 0x7A, 0x07, 0xF7, 0x7F, 0x00, 0x00, 0x00, 0x00 };
    }
}
