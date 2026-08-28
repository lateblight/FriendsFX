using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;

namespace FriendsFX
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "FriendsFX";
        
        private readonly Configuration configuration;
        private readonly FriendsEffectController effectController;
        private readonly WindowSystem windowSystem;
        private readonly ConfigWindow configWindow;

        public Plugin(IDalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<Service>();

            configuration = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            configuration.Initialize(pluginInterface);

            effectController = new FriendsEffectController(
                Service.ClientState, 
                Service.ObjectTable, 
                Service.Framework, 
                Service.CommandManager,
                configuration
            );

            windowSystem = new WindowSystem("FriendsFX");
            configWindow = new ConfigWindow(configuration);
            windowSystem.AddWindow(configWindow);

            pluginInterface.UiBuilder.Draw += windowSystem.Draw;
            pluginInterface.UiBuilder.OpenConfigUi += OnOpenConfigUi;

            Service.CommandManager.AddHandler("/friendsfx", new CommandInfo(OnCommand)
            {
                HelpMessage = "Opens the FriendsFX configuration window."
            });
        }

        private void OnCommand(string command, string args)
        {
            configWindow.Toggle();
        }

        private void OnOpenConfigUi()
        {
            configWindow.Toggle();
        }

        public void Dispose()
        {
            Service.CommandManager.RemoveHandler("/friendsfx");
            
            Service.PluginInterface.UiBuilder.Draw -= windowSystem.Draw;
            Service.PluginInterface.UiBuilder.OpenConfigUi -= OnOpenConfigUi;

            effectController?.Dispose();
        }
    }
}