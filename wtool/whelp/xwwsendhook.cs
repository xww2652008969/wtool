using System.Runtime.InteropServices;
using Dalamud.Hooking;
using ECommons.DalamudServices;
using whook.whelp.nethook;

namespace whook.whelp;

public class xwwsendhook
{
    private static Hook<setflag> _s1;
    private static Hook<setflag> _s2;

    public static void inithook()
    {
        var s1 = Memoryhelp.getbase() + 27949008;
        _s1 = Svc.Hook.HookFromAddress<setflag>(s1, hook1);
        var s2 = Memoryhelp.getbase() + 28237408;
        _s2 = Svc.Hook.HookFromAddress<setflag>(s2, hook2);
        _s1.Enable();
        _s2.Enable();
    }


    public static void hook1(IntPtr a1, long a2)
    {
        Svc.Log.Debug("触发了send1");
        Svc.Log.Debug(a1.ToString());
        Svc.Log.Debug(a2.ToString());
        Svc.Log.Debug("处理函数");
        get(a1);
        _s1.Original(a1, a2);
    }

    public static void hook2(IntPtr a1, long a2)
    {
        Svc.Log.Debug("触发了send2");
        Svc.Log.Debug(a1.ToString());
        Svc.Log.Debug(a2.ToString());
        Svc.Log.Debug("处理函数");
        get(a1);
        _s2.Original(a1, a2);
    }


    public static void get(IntPtr a1, bool flag1)
    {
        //服务器下发
        var b = a1 + 16;
        var c = Marshal.ReadInt64(b);
        var d = (IntPtr)c;
        var hand = new byte[40];
        for (var i = 0; i < 40; i++) hand[i] = Marshal.ReadByte(d + i);
        var datah = ByteArrayToDataPacket(hand); //提取包hnd
        if (datah.Compression != 0) return;
        var num_segments = datah.SegmentCount;
        var frame_size = datah.Size;
        var frame_hand_size = Marshal.SizeOf(typeof(FrameHeader));
        if (frame_size > 0x10000 || frame_size < frame_hand_size) return;
        var frame_data = new byte[frame_size - frame_hand_size];
        var frame_data_len = frame_data.Length;
        for (var i = 0; i < frame_size - frame_hand_size; i++)
            frame_data[i] = Marshal.ReadByte(d + frame_hand_size + i);
        Svc.Log.Debug("接受到了数据");
        var bb = new byte[1] { 1 };
        if (wstol.s == null) return;
        wstol.s.SendMessageAsync(bb.Concat(hand).Concat(frame_data).ToArray()); //发包给其他程序
        return;
        var frame_data_offset = (uint)0; //解析包
        var pack = new Packet();         //创建一个packet
        for (var i = 0; i < num_segments; i++)
        {
            var segment_size = frame_data_len - frame_data_offset;
            var segment_header_size = Marshal.SizeOf(typeof(SegmentHeader));
            if (segment_size > 0x10000 || segment_size < segment_header_size) return;
            var segmentheader_bytes = new byte[segment_header_size];
            if (frame_data.Length < segment_header_size) return;
            Array.Copy(frame_data, frame_data_offset, segmentheader_bytes, 0, segment_header_size);
            var header = ByteArrayToSegmentHeader(segmentheader_bytes);
            Svc.Log.Debug(header.Size.ToString());
            Svc.Log.Debug(num_segments.ToString());
            var databyte = new byte[header.Size - segment_header_size];
            Array.Copy(frame_data, frame_data_offset, databyte, 0, header.Size - segment_header_size);
            Segment segment;
            segment.Header = header;
            segment.Data = databyte;
            frame_data_offset += header.Size;
        }
    }


    public static void get(IntPtr a1)
    {
        var b = a1 + 32;
        var c = Marshal.ReadInt64(b);
        var d = (IntPtr)c;
        var hand = new byte[40];
        for (var i = 0; i < 40; i++) hand[i] = Marshal.ReadByte(d + i);
        var datah = ByteArrayToDataPacket(hand); //提取包hnd
        if (datah.Compression != 0) return;
        var num_segments = datah.SegmentCount;
        var frame_size = datah.Size;
        var frame_hand_size = Marshal.SizeOf(typeof(FrameHeader));
        if (frame_size > 0x10000 || frame_size < frame_hand_size) return;
        var frame_data = new byte[frame_size - frame_hand_size];
        var frame_data_len = frame_data.Length;
        for (var i = 0; i < frame_size - frame_hand_size; i++)
            frame_data[i] = Marshal.ReadByte(d + frame_hand_size + i);
        Svc.Log.Debug("接受到了数据");
        var bb = new byte[1] { 2 };
        if (wstol.s == null) return;
        wstol.s.SendMessageAsync(bb.Concat(hand).Concat(frame_data).ToArray());
    }


    public static FrameHeader ByteArrayToDataPacket(byte[] data)
    {
        if (data.Length != Marshal.SizeOf(typeof(FrameHeader))) throw new ArgumentException("数据长度不匹配");

        FrameHeader packet;
        // 将字节数组转换为结构体
        using (var ms = new MemoryStream(data))
        {
            using (var br = new BinaryReader(ms))
            {
                packet.Magic = br.ReadBytes(16);
                packet.Timestamp = br.ReadUInt64();
                packet.Size = br.ReadUInt32();
                packet.ConnectionType = br.ReadUInt16();
                packet.SegmentCount = br.ReadUInt16();
                packet.Unknown20 = br.ReadByte();
                packet.Compression = br.ReadByte();
                packet.Unknown22 = br.ReadUInt16();
                packet.DecompressedLength = br.ReadUInt32();
            }
        }

        return packet;
    }

    public static SegmentHeader ByteArrayToSegmentHeader(byte[] data)
    {
        SegmentHeader header;
        using (var ms = new MemoryStream(data))
        {
            using (var br = new BinaryReader(ms))
            {
                header.Size = br.ReadUInt32();
                header.SourceActor = br.ReadUInt32();
                header.TargetActor = br.ReadUInt32();
                header.SegmentType = br.ReadUInt16();
                header.Padding = br.ReadUInt16();
            }
        }

        return header;
    }

    public static IpcHeader ByteArrayToIpcHeader(byte[] data)
    {
        IpcHeader ipcheader;
        using (var ms = new MemoryStream(data))
        {
            using (var br = new BinaryReader(ms))
            {
                ipcheader.reserved = br.ReadUInt16();
                ipcheader.type = br.ReadUInt16();
                ipcheader.padding = br.ReadUInt16();
                ipcheader.serverId = br.ReadUInt16();
                ipcheader.timestamp = br.ReadUInt32();
                ipcheader.padding1 = br.ReadUInt16();
            }
        }

        return ipcheader;
    }

    public static void dispose()
    {
        if (_s1 != null)
        {
            _s1.Dispose();
            _s1.Dispose();
        }
    }

    private delegate void setflag(IntPtr a1, long a2);
}
