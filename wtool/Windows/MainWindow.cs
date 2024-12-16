using System.Numerics;
using System.Runtime.InteropServices;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Hooking;
using Dalamud.Interface.Windowing;
using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using ImGuiNET;
using whook.whelp.nethook;

namespace whook.Windows;

public unsafe class MainWindow : Window, IDisposable
{
    public delegate int set(int a1, int a2, int a3, int a4, int a5);

    private static bool b = false;

    public static IntPtr up =
        Svc.SigScanner.ScanText("48 89 5C 24 ?? 48 89 74 24 ?? 4C 89 64 24 ?? 55 41 56 41 57 48 8B EC 48 83 EC 70");


    public static IntPtr setflag =
        Svc.SigScanner.ScanText(
            "48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 8B E9 41 8B D9 48 8B 0D ?? ?? ?? ?? 41 8B F8 8B F2");

    public readonly Hook<set> _setHook;

    private readonly Hook<ProcessZonePacketUpDelegate> _up;
    private CancellationTokenSource cts;
    public MarkingController* flag;
    private Plugin p;

    public MainWindow(Plugin plugin) : base("My Amazing Window##With a hidden ID",
                                            ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        _up = Svc.Hook.HookFromAddress<ProcessZonePacketUpDelegate>(up, upevent);
        _setHook = Svc.Hook.HookFromAddress<set>(setflag, s);
        p = plugin;
        _setHook.Enable();
        flag = MarkingController.Instance();
    }


    public void Dispose()
    {
        cts?.Cancel();
        _up.Dispose();
        _setHook.Dispose();
    }


    private int s(int a1, int a2, int a3, int a4, int a5)
    {
        Svc.Log.Debug(a1.ToString());
        Svc.Log.Debug(a2.ToString());
        Svc.Log.Debug(a3.ToString());
        Svc.Log.Debug(a4.ToString());
        Svc.Log.Debug(a5.ToString());
        return _setHook.Original(a1, a2, a3, a4, a5);
    }

    private byte upevent(IntPtr a1, IntPtr dataPtr, IntPtr a3, byte a4)
    {
        // if (Marshal.ReadInt32(dataPtr) == 123)
        // {
        //     var da = new byte[32];
        //     Marshal.Copy(dataPtr, da, 0, da.Length);
        //     var d = BitConverter.ToString(da).Replace("-", " ");
        //     Svc.Log.Debug($"opcode：{d}");   
        // }
        if (Marshal.ReadInt32(dataPtr) == 172)
        {
            var h = new IPC();
            h.Opcode = 172;
            h.Timestamp = 3670016;
            var d = new 进本();
            d.flag1 = 32;
            d.flag4 = 1;
            d.id = 88;
            var hh = tool.tool.StructToBytes(h);
            var dd = tool.tool.StructToBytes(d);
            var data = hh.Concat(dd).ToArray();
            var ptr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, ptr, data.Length);
            Svc.Log.Debug($"a1{a1.ToString()}");
            _up.Original(a1, ptr, a3, a4);
            return 1;
            var s = BitConverter.ToString(data).Replace("-", " ");
            Svc.Log.Debug(s);
            var da = new byte[64];
            Marshal.Copy(dataPtr, da, 0, da.Length);
            var ss = BitConverter.ToString(da).Replace("-", " ");
            Svc.Log.Debug(ss);


            // var hea = new byte[32];
            // Marshal.Copy(dataPtr, hea, 0, hea.Length);
            // var dd=tool.tool.ByteArrayToStruct<whelp.nethook.IPC>(hea);
            // Svc.Log.Debug(dd.Timestamp.ToString());
            // var da = new byte[32];
            // Marshal.Copy(dataPtr + 32, da, 0, da.Length);
            // var d = BitConverter.ToString(da).Replace("-", " ");
            // Svc.Log.Debug(d);
            // Svc.Log.Debug(a1.ToString());
            // Svc.Log.Debug(a3.ToString());
            // Svc.Log.Debug(a4.ToString());
        }

        return _up.Original(a1, dataPtr, a3, a4);
    }

    public override void Draw()
    {
        var o = Svc.Objects.Where(v => v.ObjectKind == ObjectKind.Player).ToArray();
        var obj = new string[0];
        foreach (var v in o)
        {
            Array.Resize(ref obj, obj.Length + 1);
            obj[obj.Length - 1] = v.Name.ToString();
        }

        var set = 0;
        if (ImGui.ListBox("对象列表", ref set, obj, o.Length))
        {
            if (o[set] != null)
            {
                if (cts != null && cts.IsCancellationRequested == false) cts.Cancel();
                cts = new CancellationTokenSource();
                Task.Run(() => zh(o[set], cts.Token));
            }
        }

        // if (ImGui.Button("Close"))
        // {
        //     _setHook.Original(202, 8, 0, 0, 0); //202是传送
        // }
        if (ImGui.Button("停止")) cts.Cancel();

        if (ImGui.Button("清除"))
        {
            if (cts != null && cts.IsCancellationRequested == false) cts.Cancel();

            for (var i = 0; i < 8; i++) flag->FieldMarkers[i].Active = false;
        }
    }

    private async Task zh(IGameObject m, CancellationToken token)
    {
        var i = 0;
        double kkm;
        for (var j = 0; j < 8; j++)
        {
            kkm = (m.Rotation + (2 * Math.PI)) * (j + (1 / 8));
            var x = MathF.Sin((float)kkm); //x
            var z = MathF.Cos((float)kkm); //y
            var b = new Vector3(x, 0, z);
            flag->FieldMarkers[j].Position = m.Position + (5 * b);
            flag->FieldMarkers[j].Active = true;
        }

        while (true)
        {
            token.ThrowIfCancellationRequested();
            for (var j = 0; j < 8; j++)
            {
                if (m.ObjectKind != ObjectKind.Player) return;
                var k = flag->FieldMarkers[j].Position -= m.Position;
                var f = Math.Atan(k.Z / k.X) * (180 / Math.PI);
                f = f + (Math.PI / 16);
                var x = MathF.Sin((float)f);
                var z = MathF.Cos((float)f);
                var b = new Vector3(x, 0, z);
                flag->FieldMarkers[j].Position = m.Position + (1 * b);
                flag->FieldMarkers[j].Active = true;
            }
        }
    }

    private delegate byte ProcessZonePacketUpDelegate(IntPtr a1, IntPtr dataPtr, IntPtr a3, byte a4);
}
