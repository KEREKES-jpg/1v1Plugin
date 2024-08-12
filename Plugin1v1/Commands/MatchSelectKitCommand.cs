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
    public sealed class MatchSelectKitCommand : ICommand
    {
        public string Command { get; } = "select";
        public string[] Aliases { get; }
        public string Description { get; } = "Selects kit";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1)
            {
                response = "Incorrect format, please use: .select [kit name]";
                return false;
            }
            var player = Player.Get(((PlayerCommandSender)sender).ReferenceHub);
            string desiredKit = arguments.At(0);
            if (player.FightManager().IsFighting)
            {
                response = "Can't select kit while fighting!";
                return false;
            }

            if (Plugin.Instance.Config.Loudouts.ContainsKey(desiredKit))
            {
                player.FightManager().selectedKit = desiredKit;
                response = "Kit selected";
                return true;
            }
            else
            {
                response = "Kit does not exist please select one of the following kits:";
                foreach (var n in Plugin.Instance.Config.Loudouts)
                {
                    response += $"\n {n.Key}:";
                    foreach (var m in n.Value)
                    {
                        response += $"\n -{m}";
                    }
                }
                return false;
            }        

        }
    }
}
