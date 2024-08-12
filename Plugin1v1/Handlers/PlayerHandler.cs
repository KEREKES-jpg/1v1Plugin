using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin1v1.Handlers
{
    public class PlayerHandler
    {
        public void OnRoleChanged(SpawnedEventArgs ev)
        {
            if (ev.Player.Role == RoleTypeId.Overwatch) return;

            if (ev.Player.FightManager().IsFighting == false)
            {
                if (ev.Player.FightManager().IsSpectating == false & ev.Player.Role != RoleTypeId.Tutorial)
                {
                    ev.Player.Role.Set(RoleTypeId.Tutorial);
                }
            }


        }

        public void OnPlayerConnect(VerifiedEventArgs ev)
        {
            ev.Player.Broadcast(6, String.Format(Plugin.Instance.Config.PlayerJoinBroadcast, ev.Player.Nickname));
        }

        public void OnDisconnect(LeftEventArgs ev)
        {
            if (ev.Player.FightManager().IsFighting)
            {
                ev.Player.FightManager().Opponent.FightManager().ExitMatch(MatchCancelReason.Disconnect);
            }

        }

        public void OnDoorDestroyed(DamagingDoorEventArgs ev)
        {
            ev.IsAllowed = false;
        }

        public void PlayerDying(DyingEventArgs ev)
        {
            ev.Player.ClearInventory();

        }

        public void SpawningRagdoll(SpawningRagdollEventArgs ev)
        {
            ev.IsAllowed = false;
        }

        public void OnPlayerKilled(DiedEventArgs ev)
        {
            if (ev.Player == null) return;
            if (ev.Player.FightManager().IsFighting)
            {
                ev.Player.FightManager().LostRound();
                ev.Attacker.FightManager().WonRound();
            }
        }

        public void DroppingItem(DroppingItemEventArgs ev)
        {
            ev.IsAllowed = false;
        }

        public void DroppingAmmo(DroppingAmmoEventArgs ev)
        {
            ev.IsAllowed = false;
        }
    }
}
