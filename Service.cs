using Dalamud.Plugin.Services;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace FriendsFX
{
    public class Service
    {
        [PluginService] public static IClientState ClientState { get; private set; } = null!;
        [PluginService] public static IObjectTable ObjectTable { get; private set; } = null!;
        [PluginService] public static IFramework Framework { get; private set; } = null!;
        [PluginService] public static IPartyList PartyList { get; private set; } = null!;
        [PluginService] public static ICommandManager CommandManager { get; private set; } = null!;
        [PluginService] public static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    }
}