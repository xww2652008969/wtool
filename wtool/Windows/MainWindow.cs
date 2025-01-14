using System.Numerics;
using System.Runtime.CompilerServices;
using Dalamud.Game.ClientState.Objects.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.Gui.ContextMenu;
using Dalamud.Interface.ImGuiNotification;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ECommons.GameHelpers;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using OmenTools;
using OmenTools.ImGuiOm;

namespace whook.Windows;

public unsafe class MainWindow : Window, IDisposable
{

    private static float a;
    private static bool b;
    public static Dictionary<uint, World> Worlds;
    public MainWindow( ) : base("Wtool", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        hookManager.Initialize();
        DService.ContextMenu.OnMenuOpened +=  OnMenuOpen;
        DService.Framework.Update += Frameworkupdate;
        DService.Framework.Update += hookManager.check;
    }

    private void Frameworkupdate( IFramework iFramework )
    {
        var objectlist = DService.ObjectTable;
        foreach (var o in objectlist)
        {
            if (o == DService.ClientState.LocalPlayer)
            {
                continue ;
            }
            if (o.ObjectKind == ObjectKind.Player)
            {
                var player = (Character*)o.Address;
                var l = new Whitelist();
                l.id=player->HomeWorld;
                l.name=player->NameString;
                if (!Config.Instance.Whitelist.Contains(l))
                {
                    Glo.Glo.Open = false;
                    //DService.Chat.Print("危险");
                    return;
                }
                
            }
        }

        Glo.Glo.Open = true;
        //DService.Chat.Print("安全");
    }
    private void OnMenuOpen(IMenuOpenedArgs args)
    {
        if (args.AddonName == null) //疑 玩家右键
        {
            var me=DService.ClientState.LocalPlayer;
            if (me != null)
            {
                var Target = me.TargetObject;
                if (Target != null&& Target.ObjectKind==ObjectKind.Player)
                {
                    var t = (Character*)Target.Address;
                    var li = new Whitelist();
                     li.id = t->HomeWorld;
                     li.name=t->NameString;
                     if (Config.Instance.Whitelist.Contains(li))
                     {
                         return;
                     }
                    var m = new MenuItem();
                    m.Name = "加入Wtool白名单";
                    m.PrefixChar = 'W';
                    m.PrefixColor = 706;
                    m.OnClicked += clickedArgs=>addwi(li);
                    args.AddMenuItem(m);
                }
                return;
            }
            return;
        }
    }

    private void addwi(Whitelist w)
    {
        Config.Instance.Whitelist.Add(w);
        Config.Instance.Save();
    }
    public void Dispose()
    {
        DService.ContextMenu.OnMenuOpened += OnMenuOpen;
        hookManager.uninit();
    }


    public override void Draw()
    {
        if (ImGui.CollapsingHeader("白名单"))
        {
            if (ImGui.BeginTable("白名单", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable))
            {
                // 表头
                ImGui.TableSetupColumn("服务器");
                ImGui.TableSetupColumn("name");
                ImGui.TableSetupColumn("操作");
                ImGui.TableHeadersRow();
                for (int i = 0; i < Config.Instance.Whitelist.Count; i++)
                {
                    ImGui.TableNextRow();
                    
                    ImGui.TableNextColumn();
                    ImGui.Text(ffxivdata.Getwordname(Config.Instance.Whitelist[i].id));
                    
                    ImGui.TableNextColumn();
                    ImGui.Text(Config.Instance.Whitelist[i].name);
                    ImGui.TableNextColumn();
                    if (ImGui.Button("删除"))
                    {
                        Config.Instance.Whitelist.Remove(Config.Instance.Whitelist[i]);
                        Config.Instance.Save();
                    }
                }
                ImGui.EndTable();
            }
        }
        // 自定义移速
        if (!Config.Instance.move.Isopen)
        {
            ImGui.Checkbox("自定义移速", ref Config.Instance.move.Isopen);
            ImGui.SetNextItemWidth(300f *1);
        }

        if (Config.Instance.move.Isopen)
        {
            ImGui.Checkbox("",ref Config.Instance.move.Isopen);
            ImGui.SetNextItemWidth(300f *1);
            ImGui.SameLine();
            if (ImGui.TreeNode("自定义移速"))
            {
                ImGui.SetNextItemWidth(300f *1);
                ImGui.SliderFloat("移动速率",ref Config.Instance.move.Speed,0.1f, 10.0f);
            }
        }
        
        ImGui.Checkbox("鼠标tp",ref Config.Instance.tp.Isopen);
        
        
        
        Config.Instance.Save();
    }
    
}
