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
                Service.PartyList,
                Service.CommandManager,
                configuration
            );

            // Initialize Window System and UI
            windowSystem = new WindowSystem("FriendsFX");
            configWindow = new ConfigWindow(configuration);
            windowSystem.AddWindow(configWindow);

            // Register drawing hook to Dalamud's UI renderer
            pluginInterface.UiBuilder.Draw += windowSystem.Draw;
            pluginInterface.UiBuilder.OpenConfigUi += OnOpenConfigUi;

            // Register chat command to toggle the window
            Service.CommandManager.AddHandler("/friendsfx", new CommandInfo(OnCommand)
            {
                HelpMessage = "Opens the FriendsFX configuration window."
            });
        }

        private void OnCommand(string command, string args)
        {
            // Toggle window visibility when typing /friendsfx
            configWindow.Toggle();
        }

        private void OnOpenConfigUi()
        {
            configWindow.Toggle();
        }

        public void Dispose()
        {
            Service.CommandManager.RemoveHandler("/friendsfx");
            
            // Clean up UI hooks
            Service.PluginInterface.UiBuilder.Draw -= windowSystem.Draw;
            Service.PluginInterface.UiBuilder.OpenConfigUi -= OnOpenConfigUi;

            effectController?.Dispose();
        }
    }
}