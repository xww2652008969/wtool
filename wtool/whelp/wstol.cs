namespace whook.whelp;

public static class wstol
{
    public static WebSocketServer s;

    public static void Init()
    {
        s = new WebSocketServer("http://127.0.0.1:6001/");
        s.StartAsync();
    }
}
