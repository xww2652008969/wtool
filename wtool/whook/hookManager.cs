using Dalamud.Plugin.Services;
using OmenTools;

namespace whook;

public static class hookManager
{
    public static void check(IFramework iFramework)
    {
        // 移动
        if (Config.Instance.move.Isopen)
            whook.Move.Enable();
        else
            whook.Move.Disable();
        //tp
        if (Config.Instance.tp.Isopen)
            whook.Tp.Enable();
        else
            whook.Tp.Uninit();
    }

    public static void Initialize()
    {
        whook.Move.Init();
        DService.Chat.Print("初始化了");
    }

    public static void uninit()
    {
        whook.Move.Dispose(); //删除
        whook.Tp.Uninit();
    }
}
