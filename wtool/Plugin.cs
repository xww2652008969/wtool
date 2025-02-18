using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using OmenTools;
using whook.Windows;

namespace whook;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    public readonly WindowSystem WindowSystem = new("wtool");
    private MainWindow MainWindow { get; init; }
    public Plugin()
    {
        Glo.Glo.Pi = PluginInterface;
        Glo.Glo.Open = false;
        DService.Init(Glo.Glo.Pi);
        Config.init();
        MainWindow = new MainWindow();
        WindowSystem.AddWindow(MainWindow);
        PluginInterface.UiBuilder.Draw += DrawUi;
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUi;
    }
    

    public void Dispose()
    {
        DService.Uninit();
        WindowSystem.RemoveAllWindows();
        MainWindow.Dispose();
    }


    private void DrawUi()
    {
        WindowSystem.Draw();
    }

    public void ToggleMainUi()
    {
        MainWindow.Toggle();
    }
}
