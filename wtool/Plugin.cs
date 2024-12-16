using Dalamud.Hooking;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ECommons.DalamudServices;
using whook.Windows;

namespace whook;

public sealed class Plugin : IDalamudPlugin
{
    private static Hook<te> _s;

    public readonly WindowSystem WindowSystem = new("wtool");

    public Plugin(IDalamudPluginInterface pi)
    {
        Svc.Init(pi);
        // var i = Svc.ClientState.LocalPlayer;
        // if (i.TargetObjectId == 0)
        // {
        //     return;
        // }
        // var t= Svc.Objects.SearchById(i.TargetObjectId);
        // var me = Svc.ClientState.LocalPlayer.GameObject();
        // Svc.Log.Debug("执行了");
        // TargetSystem.Instance()->OpenObjectInteraction((GameObject*)t.Address);  //与npc 右键
        // Svc.Log.Debug("q");
        // Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        // var goatImagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
        // ConfigWindow = new ConfigWindow(this);
        MainWindow = new MainWindow(this);
        WindowSystem.AddWindow(MainWindow);
        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
        ;
    }

    [PluginService]
    internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;

    [PluginService]
    internal static ITextureProvider TextureProvider { get; private set; } = null!;

    [PluginService]
    internal static IGameInteropProvider h { get; private set; } = null!;

    public Configuration Configuration { get; init; }
    private MainWindow MainWindow { get; init; }

    public void Dispose()
    {
        // _s.Dispose();
        WindowSystem.RemoveAllWindows();
        //
        // ConfigWindow.Dispose();
        MainWindow.Dispose();
        // // xwwrecvhook.dispose();
        // xwwsendhook.dispose();
        // xwwsend_lobby.dispose();
    }


    private void DrawUI()
    {
        WindowSystem.Draw();
    }

    public void ToggleMainUI()
    {
        MainWindow.Toggle();
    }


    private delegate long te(IntPtr a1, long a2, long a3);
}
