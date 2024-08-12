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
    public sealed class MatchInviteCommand : ICommand
    {
        public string Command { get; } = "invite";
        public string[] Aliases { get; }
        public string Description { get; } = "Invites player to party!";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1)
            {
                response = "Incorrect format, please use: .invite [player name]";
                return false;
            }
            var player = Player.Get(((PlayerCommandSender)sender).ReferenceHub);
            Player targetPly = Player.Get(arguments.At(0));
            if (targetPly == null)
            {
                response = $"Failed to find player {targetPly.Nickname}!";
                return false;
            }
            else if (targetPly.FightManager().InParty)
            {
                response = $"{targetPly.Nickname} is already in party!";
                return false;
            }
            else if (targetPly.FightManager().IsFighting)
            {
                response = $"{targetPly.Nickname} is already in match!";
                return false;
            }
            else if (player.FightManager().selectedKit == null)
            {
                response = $"Please select kit first using .select [kit name] command!";
                return false;
            }
            response = $"You sent invite to {targetPly.Nickname}!";
            targetPly.FightManager().ReceiveInvite(player);
            return true;
        }
    }
}
