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
    public sealed class MatchStartCommand : ICommand
    {
        public string Command { get; } = "start";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Starts the match";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(((PlayerCommandSender)sender).ReferenceHub);

            if (player.FightManager().Opponent == null || player.FightManager().IsFighting)
            {
                response = "Can not start match without opponent.";
                return false;
            }
            else
            {
                var n = player.FightManager().GetFreeMap();
                player.FightManager().CurrentMap = n;
                player.FightManager().Opponent.FightManager().CurrentMap = n;
                player.FightManager().StartMatch(player.FightManager().Opponent);
                player.FightManager().Opponent.FightManager().StartMatch(player);
                response = "Starting match...";
                return true;
            }
        }
    }
}
