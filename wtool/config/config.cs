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
    public string ospath;

    public  List<Whitelist> Whitelist;
    public Move move;
    public Tp tp;
    
    public static void init()
    {
        Instance = new Config();
        path = Path.Combine(Glo.Glo.Pi.ConfigDirectory.ToString(), "config.json");
        var s = Glo.Glo.Pi.AssemblyLocation.ToString().Split(@"\");
        for (var i = 0; i < s.Length - 1; i++) Instance.ospath += s[i] + @"\";
        if (!File.Exists(path))
        {
            Instance.Whitelist = new();
            Instance.move = new Move();
            Instance.tp = new();
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
    }
}
