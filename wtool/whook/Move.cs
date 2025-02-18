using Dalamud.Hooking;
using OmenTools;
using OmenTools.Infos;

namespace whook.whook;

public static class Move
{
    private static readonly CompSig Movesig = new("E8 ?? ?? ?? ?? 0F 28 F0 F3 41 0F 5D F0");
    private static Hook<Movede> _hook;

    public static void Init()
    {
        _hook = DService.Hook.HookFromSignature<Movede>(Movesig.Get(), h);
    }

    private static float h(IntPtr a1)
    {
        if (Glo.Glo.Open) return Config.Instance.move.Speed * _hook.Original(a1);
        return _hook.Original(a1);
    }

    public static void Enable()
    {
        if (_hook == null) return;

        if (_hook.IsEnabled) return;
        _hook.Enable();
    }

    public static void Disable()
    {
        if (_hook == null) return;

        if (!_hook.IsEnabled) return;
        _hook.Disable();
    }

    public static void Dispose()
    {
        _hook?.Dispose();
    }

    private delegate float Movede(IntPtr a1);
}
