using System.Numerics;
using Dalamud.Game.Command;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using ImGuiNET;
using OmenTools;

namespace whook.whook;

public static unsafe class Tp
{
    public static void Enable()
    {
        if (DService.Command.Commands.ContainsKey("/xwwtp")) return;
        DService.Command.AddHandler("/xwwtp", new CommandInfo(NewCommandHandler)
        {
            HelpMessage = "鼠标tp"
        });
    }

    public static void Uninit()
    {
        if (DService.Command.Commands.ContainsKey("/xwwtp") == false) return;
        DService.Command.RemoveHandler("/xwwtp");
    }

    private static void NewCommandHandler(string command, string args)
    {
        if (Glo.Glo.Open && DService.ClientState.LocalPlayer != null)
        {
            var me = (GameObject*)DService.ClientState.LocalPlayer.Address;
            me->SetPosition(getmousepos().X, getmousepos().Y, getmousepos().Z);
        }
        else
            DService.Chat.Print("Tp毛线不想活了(周围有人");
    }

    private static Vector3 getmousepos()
    {
        var v = ImGui.GetIO().MousePos;
        Vector3 v2;
        DService.Gui.ScreenToWorld(v, out v2);
        return v2;
    }
}
