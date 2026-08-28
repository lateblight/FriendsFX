using System;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Plugin.Services;

namespace FriendsFX
{
    public class FriendsEffectController : IDisposable
    {
        private readonly IClientState clientState;
        private readonly IObjectTable objectTable;
        private readonly IFramework framework;
        private readonly ICommandManager commandManager;
        private readonly Configuration configuration;

        private DateTime lastCheck = DateTime.MinValue;
        private bool lastKnownFriendState = false;

        public FriendsEffectController(
            IClientState clientState, 
            IObjectTable objectTable, 
            IFramework framework, 
            ICommandManager commandManager,
            Configuration configuration)
        {
            this.clientState = clientState;
            this.objectTable = objectTable;
            this.framework = framework;
            this.commandManager = commandManager;
            this.configuration = configuration;

            this.framework.Update += OnFrameworkUpdate;
        }

        private void OnFrameworkUpdate(IFramework sf)
        {
            if ((DateTime.Now - lastCheck).TotalSeconds < 2.0) return;
            lastCheck = DateTime.Now;

            if (!clientState.IsLoggedIn) return;

            EvaluateWorldEffects();
        }

        private void EvaluateWorldEffects()
        {
            var localPlayer = objectTable.LocalPlayer;
            if (localPlayer == null) return;

            bool friendFoundNearby = false;

            foreach (var obj in objectTable)
            {
                if (obj is IPlayerCharacter player)
                {
                    if (player.GameObjectId == localPlayer.GameObjectId) continue;

                    string playerName = player.Name.TextValue;

                    if (IsUserAFriend(playerName))
                    {
                        friendFoundNearby = true;
                        break;
                    }
                }
            }

            if (friendFoundNearby != lastKnownFriendState)
            {
                lastKnownFriendState = friendFoundNearby;

                if (lastKnownFriendState)
                {
                    commandManager.ProcessCommand("/battleeffect party all");
                }
                else
                {
                    commandManager.ProcessCommand("/battleeffect party simple");
                }
            }
        }

        private bool IsUserAFriend(string playerName)
        {
            return configuration.FriendNames.Exists(name => 
                string.Equals(name, playerName, StringComparison.CurrentCultureIgnoreCase));
        }

        public void Dispose()
        {
            framework.Update -= OnFrameworkUpdate;
        }
    }
}