using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using OmenTools;
using whook.Windows;

namespace whook;

public sealed class Plugin : IDalamudPlugin
{
    public readonly WindowSystem WindowSystem = new("wtool");
    public Plugin(IDalamudPluginInterface pi)
    {
        Glo.Glo.Pi = pi;
        Glo.Glo.Open = false;
        DService.Init(Glo.Glo.Pi);
        Config.init();
        MainWindow = new MainWindow();
        WindowSystem.AddWindow(MainWindow);
        pi.UiBuilder.Draw += DrawUi;
        pi.UiBuilder.OpenMainUi += ToggleMainUi;
        
    }

    private MainWindow MainWindow { get; init; }

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
