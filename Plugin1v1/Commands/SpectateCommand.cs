using CommandSystem;
using Exiled.API.Features;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin1v1.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public sealed class SpectaceCommand : ICommand
    {
        public string Command { get; } = "spectate";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Toggles between tutorial and spectator!";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(((PlayerCommandSender)sender).ReferenceHub);

            if (player.FightManager().IsFighting)
            {
                response = "You are in a match!";
                return false;
            }


            if (player.FightManager().IsSpectating)
            {
                player.FightManager().IsSpectating = false;
                player.Role.Set(PlayerRoles.RoleTypeId.Tutorial);
            }
            else
            {
                player.FightManager().IsSpectating = true;
                player.Role.Set(PlayerRoles.RoleTypeId.Spectator);
            }
            response = "Toggled";
            return true;           

        }
    }
}
