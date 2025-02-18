using Newtonsoft.Json;
using OmenTools;

namespace whook;

public struct Whitelist
{
    public string name;
    public uint id;
}

public struct Move
{
    public float Speed;
    public bool Isopen;
}

public struct Tp
{
    public bool Isopen;
}

public class Config
{
    public static Config Instance = new();
    public static string path;
    public Move move;
    public string ospath;
    public Tp tp;

    public List<Whitelist> Whitelist;

    public static void init()
    {
        Instance = new Config();
        path = Path.Combine(Glo.Glo.Pi.ConfigDirectory.ToString(), "config.json");
        var s = Glo.Glo.Pi.AssemblyLocation.ToString().Split(@"\");
        for (var i = 0; i < s.Length - 1; i++) Instance.ospath += s[i] + @"\";
        if (!File.Exists(path))
        {
            Instance.Whitelist = new List<Whitelist>();
            Instance.move = new Move();
            Instance.tp = new Tp();
            Instance.Save();
            return;
        }

        try
        {
            var j = File.ReadAllText(path);
            Instance = JsonConvert.DeserializeObject<Config>(j);
        }
        catch (Exception e)
        {
            DService.Log.Debug(e.ToString());
        }
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, JsonConvert.SerializeObject(Instance, Formatting.Indented));
        return;
    }
}
