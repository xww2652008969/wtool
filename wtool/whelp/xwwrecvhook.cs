using Dalamud.Hooking;
using ECommons.DalamudServices;

namespace whook.whelp;

public class xwwrecvhook
{
    private static readonly IntPtr c1 = Memoryhelp.getbase() + 27940384;
    private static readonly IntPtr c2 = Memoryhelp.getbase() + 27971408;
    private static readonly IntPtr c3 = Memoryhelp.getbase() + 28237840; //找不到签名后面再说

    private static Hook<recvhook> _r1;
    private static Hook<recvhook> _r2;
    private static Hook<recvhook> _r3;

    public static void inithook()
    {
        _r1 = Svc.Hook.HookFromAddress<recvhook>(c1, r1);
        _r1.Enable();
        _r2 = Svc.Hook.HookFromAddress<recvhook>(c2, r2);
        _r2.Enable();
        _r3 = Svc.Hook.HookFromAddress<recvhook>(c3, r3);
        _r3.Enable();
    }

    private static long r1(IntPtr a1, IntPtr a2, int a3, long a4, long a5)
    {
        xwwsendhook.get(a1, true);
        Svc.Log.Debug("触发了1");
        return _r1.Original(a1, a2, a3, a4, a5);
    }

    private static long r2(IntPtr a1, IntPtr a2, int a3, long a4, long a5)
    {
        xwwsendhook.get(a1, true);
        Svc.Log.Debug("触发了2");
        return _r2.Original(a1, a2, a3, a4, a5);
    }

    private static long r3(IntPtr a1, IntPtr a2, int a3, long a4, long a5)
    {
        xwwsendhook.get(a1, true);
        Svc.Log.Debug("触发了3");
        return _r3.Original(a1, a2, a3, a4, a5);
    }


    public static void dispose()
    {
        _r1.Dispose();
        _r2.Dispose();
        _r3.Dispose();
    }


    private delegate long recvhook(IntPtr a1, IntPtr a2, int a3, long a4, long a5);
}
