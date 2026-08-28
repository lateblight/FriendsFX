using ImGuiNET;
using Dalamud.Interface.Windowing;
using System.Numerics;

namespace FriendsFX
{
    public class ConfigWindow : Window
    {
        private readonly Configuration configuration;
        private string inputName = string.Empty;

        public ConfigWindow(Configuration configuration) : base("FriendsFX Settings")
        {
            this.configuration = configuration;
            Size = new Vector2(400, 350);
            SizeCondition = ImGuiCond.FirstUseEver;
        }

        public override void Draw()
        {
            ImGui.Text("Welcome to FriendsFX!");
            ImGui.Spacing();

            // Updated text to reflect global tracking anywhere in the world
            ImGui.TextWrapped("Add character names (e.g. Firstname Lastname) of your friends below. When they are nearby anywhere in the world, battle effects will automatically be shown fully!");
            ImGui.Spacing();

            ImGui.SetNextItemWidth(250);
            ImGui.InputText("##FriendInput", ref inputName, 100);
            ImGui.SameLine();

            if (ImGui.Button("Add Friend"))
            {
                if (!string.IsNullOrWhiteSpace(inputName) && !configuration.FriendNames.Contains(inputName.Trim()))
                {
                    configuration.FriendNames.Add(inputName.Trim());
                    configuration.Save();
                    inputName = string.Empty;
                }
            }

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            ImGui.Text("Current Friends List:");
            
            if (ImGui.BeginChild("FriendScrollRegion", new Vector2(0, 150), true))
            {
                for (int i = 0; i < configuration.FriendNames.Count; i++)
                {
                    var friend = configuration.FriendNames[i];
                    ImGui.Text(friend);
                    ImGui.SameLine(ImGui.GetWindowWidth() - 90);
                    if (ImGui.Button($"Remove##{i}"))
                    {
                        configuration.FriendNames.RemoveAt(i);
                        configuration.Save();
                        break;
                    }
                }
                ImGui.EndChild();
            }
        }
    }
}