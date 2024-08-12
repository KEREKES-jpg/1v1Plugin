using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin1v1.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public sealed class MatchDebugCommand : ICommand
    {
        public string Command { get; } = "get_data";
        public string[] Aliases { get; }
        public string Description { get; } = "Return playes match data";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1)
            {
                response = "Nespravny format, pis komand takto: get_data [meno hraca]";
                return false;
            }

            Player targetPly = Player.Get(arguments.At(0));
            if (targetPly == null)
            {
                response = "Could not find player";
                return false;
            }

            response = $"{targetPly.FightManager().ToString()}";
            return true;
        }
    }
}
