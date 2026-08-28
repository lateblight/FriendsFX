using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;

namespace FriendsFX
{
    public class ConfigWindow : Window, IDisposable
    {
        private readonly Configuration configuration;
        private string newFriendInput = string.Empty;

        public ConfigWindow(Configuration configuration) : base("FriendsFX Settings")
        {
            this.configuration = configuration;

            Size = new Vector2(400, 300);
            SizeCondition = ImGuiCond.FirstUseEver;
        }

        public void Dispose()
        {
        }

        public override void Draw()
        {
            ImGui.Text("Welcome to FriendsFX!");
            ImGui.Separator();
            ImGui.Spacing();

            ImGui.TextWrapped("Add character names (e.g. Firstname Lastname) of your friends below. When they are in your party, battle effects will automatically be shown fully!");
            ImGui.Spacing();

            ImGui.SetNextItemWidth(250);
            ImGui.InputText("##NewFriendInput", ref newFriendInput, 50);
            ImGui.SameLine();

            if (ImGui.Button("Add Friend"))
            {
                if (!string.IsNullOrWhiteSpace(newFriendInput) && !configuration.FriendNames.Contains(newFriendInput.Trim()))
                {
                    configuration.FriendNames.Add(newFriendInput.Trim());
                    configuration.Save();
                    newFriendInput = string.Empty;
                }
            }

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Text("Current Friends List:");

            if (ImGui.BeginChild("FriendScrollRegion", new Vector2(0, 150), true, ImGuiWindowFlags.None))
            {
                for (int i = 0; i < configuration.FriendNames.Count; i++)
                {
                    ImGui.Text(configuration.FriendNames[i]);
                    ImGui.SameLine(ImGui.GetWindowWidth() - 80);
                    
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