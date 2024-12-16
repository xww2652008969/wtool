using Dalamud.Game.ClientState.Objects.SubKinds;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using FFXIVClientStructs.FFXIV.Client.Game.Object;

namespace whook.whelp;

public static class Whelp
{
    public static IPlayerCharacter Me = Svc.ClientState.LocalPlayer;

    public static unsafe GameObject* ToGameObject()
    {
        if (Me == null) return null;
        return Me.GameObject();
    }

    public static unsafe GameObject* ToGameObject(IPlayerCharacter lp)
    {
        return lp.GameObject();
    }
}
