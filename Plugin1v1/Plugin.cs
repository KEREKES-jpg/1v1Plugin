using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using Exiled.CustomRoles.Events;

namespace Plugin1v1
{
    public class Plugin : Plugin<Config>
    {
        private static readonly Plugin Singleton = new Plugin();
        public static Plugin Instance => Singleton;

        public override PluginPriority Priority { get; } = PluginPriority.Medium;

        private Handlers.ServerHandler server;
        private Handlers.PlayerHandler player;

        private Plugin()
        {
        }

        public override void OnEnabled()
        {
            RegisterEvents();
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            UnregisterEvents();
            base.OnDisabled();
        }

        public void RegisterEvents()
        {
            server = new Handlers.ServerHandler();
            player = new Handlers.PlayerHandler();

            Server.RoundStarted += server.OnRoundStart;
            Player.Spawned += player.OnRoleChanged;
            Player.Dying += player.PlayerDying;
            Player.Died += player.OnPlayerKilled;
            Player.DroppingItem += player.DroppingItem;
            Player.DroppingAmmo += player.DroppingAmmo;
            Player.Left += player.OnDisconnect;
            Player.DamagingDoor += player.OnDoorDestroyed;
            Player.SpawningRagdoll += player.SpawningRagdoll;
            Player.Verified += player.OnPlayerConnect;
        }

        public void UnregisterEvents()
        {
            Server.RoundStarted -= server.OnRoundStart;
            Player.Spawned -= player.OnRoleChanged;
            Player.Dying -= player.PlayerDying;
            Player.Died -= player.OnPlayerKilled;
            Player.DroppingItem -= player.DroppingItem;
            Player.DroppingAmmo -= player.DroppingAmmo;
            Player.Left -= player.OnDisconnect;
            Player.DamagingDoor -= player.OnDoorDestroyed;
            Player.SpawningRagdoll -= player.SpawningRagdoll;
            Player.Verified -= player.OnPlayerConnect;

            server = null;
            player = null;
        }
    }
}
