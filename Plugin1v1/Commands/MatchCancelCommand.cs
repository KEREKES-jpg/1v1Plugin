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
    public sealed class MatchCancelCommand : ICommand
    {
        public string Command { get; } = "cancel";
        public string[] Aliases { get; }
        public string Description { get; } = "Cancels current match or leaves party if in one";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(((PlayerCommandSender)sender).ReferenceHub);
            if (player.FightManager().IsFighting)
            {
                if (player.FightManager().Opponent != null) player.FightManager().Opponent.FightManager().ExitMatch(MatchCancelReason.Cancelled);
                player.FightManager().ExitMatch(MatchCancelReason.Cancelled);
                response = "Cancelling match";
                return true;
            }
            else
            {
                if (player.FightManager().Opponent != null)
                {
                    player.FightManager().Opponent.FightManager().CancelParty(true);
                    player.FightManager().CancelParty(true);
                    response = "Leaving party";
                    return true;
                }
                else
                {
                    response = "You are not in match";
                    return false;
                }

            }
        }
    }
}
