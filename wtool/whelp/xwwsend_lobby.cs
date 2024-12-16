using Dalamud.Hooking;
using ECommons.DalamudServices;

namespace whook.whelp;

public static class xwwsend_lobby
{
    private static Hook<send_lobby> _SendLobby;

    public static void inithook()
    {
        var s = Memoryhelp.getbase() + 27971232;
        _SendLobby = Svc.Hook.HookFromAddress<send_lobby>(s, send);
        _SendLobby.Enable();
    }


    private static void send(IntPtr a1)
    {
        Svc.Log.Debug("触发了_send_lobby");
        xwwsendhook.get(a1);
        _SendLobby.Original(a1);
    }

    public static void dispose()
    {
        _SendLobby.Dispose();
    }

    private delegate void send_lobby(IntPtr a1);
}
