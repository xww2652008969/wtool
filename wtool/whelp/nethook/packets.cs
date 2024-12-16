using System.Runtime.InteropServices;

namespace whook.whelp.nethook;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct FrameHeader
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] Magic; // 16 bytes

    public ulong Timestamp;         // 8 bytes
    public uint Size;               // 4 bytes
    public ushort ConnectionType;   // 2 bytes
    public ushort SegmentCount;     // 2 bytes
    public byte Unknown20;          // 1 byte
    public byte Compression;        // 1 byte
    public ushort Unknown22;        // 2 bytes
    public uint DecompressedLength; // 4 bytes
}

public struct SegmentHeader
{
    public uint Size;
    public uint SourceActor;
    public uint TargetActor;
    public ushort SegmentType;
    public ushort Padding;
}

public struct Segment
{
    public SegmentHeader Header;
    public byte[] Data;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct IpcHeader
{
    public ushort reserved; // 2 bytes
    public ushort type;     // 2 bytes
    public ushort padding;  // 2 bytes
    public ushort serverId; // 2 bytes
    public uint timestamp;  // 4 bytes
    public uint padding1;   // 4 bytes
}

public struct Packet
{
    public List<byte[]> Ipc;
    public List<byte[]> Other;
}
