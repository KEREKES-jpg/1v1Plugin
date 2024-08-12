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
    public sealed class MatchAcceptInviteCommand : ICommand
    {
        public string Command { get; } = "accept";
        public string[] Aliases { get; } = { "yes" };
        public string Description { get; } = "Accepts invite";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var player = Player.Get(((PlayerCommandSender)sender).ReferenceHub);

            if (arguments.Count == 1)
            {
                Player targetPly = Player.Get(arguments.At(0));
                if (player.FightManager().AcceptInvite(targetPly))
                {
                    response = "Invite accepted!";
                    return true;
                }
                else
                {
                    response = "Invite could not be accepted.";
                    return false;
                }
            }
            else
            {
                response = "Incorrect format! Use: .accept [name]";
                return false;
            }
        }
    }
}
