using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using Dalamud.Hooking;
using Dalamud.Plugin.Services;
using DotNetDetour;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using OmenTools;
using OmenTools.Infos;

namespace whook;

public unsafe class te
{
    private delegate void setposde(GameObject* a1,float x,float y,float z);
    
    private static Hook<setposde> _s;
    private static CompSig sig = new("E8 ?? ?? ?? ?? 83 4B 70 01");

    public static void init()
    {
        _s = DService.Hook.HookFromSignature<setposde>(sig.Get(), sss,IGameInteropProvider.HookBackend.MinHook);
        _s.Enable();
    }

    private static void sss(GameObject* a1,float x,float y,float z)
    {
        
        _s.Original(a1,x,y,z);
    }

    public static void dispose()
    {
        _s.Dispose();
    }

}

