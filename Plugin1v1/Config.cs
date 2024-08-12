using Exiled.API.Enums;
using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Plugin1v1
{
    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("Possible loudouts")]
        public Dictionary<string, List<ItemType>> Loudouts { get; set; } = new Dictionary<string, List<ItemType>>()
        {
            {"dclass", new List<ItemType>() {ItemType.ArmorLight} }
        };

        [Description("Put spawn position here in spawn1 is first spawnpoint and spawn2 the second")]
        public FightMap[] Maps { get; set; } = new FightMap[]
        {
            new FightMap(new Vector3(134.058f, 1.250f, 77.151f), new Vector3(143.753f, 1.250f, 77.284f)),
            new FightMap(new Vector3(75.003f, 1.250f, 129.613f), new Vector3(74.999f, 1.250f, 143.473f)),
        };

        [Description("Required points to win a match")]
        public int RequiredWinPoints { get; set; } = 5;

        [Description("Message that is broadcasted to player that joined [{0} = this player]")]
        public string PlayerJoinBroadcast { get; set; } = "Welcome to 1v1 server {0}! Check server info";
        [Description("Message that is broadcasted to player when they receive an invite [{0} = other player]")]
        public string PlayerPartyInviteBroadcast { get; set; } = "You received invite from {0} to join a party";


        [Description("Message showed to player that won the match")]
        public string PlayerWonMessage { get; set; } = "YOU WON";

        [Description("Message showed to player that lost the match")]
        public string PlayerLostMessage { get; set; } = "YOU LOST";
        [Description("Message showed to player when new round starts. [{0} = this player, {1} = other player, {2} = this player's points, {3} = other player's points]")]
        public string MatchNewRoundMessage { get; set; } = "<color=Red>{2}:{3}</color>";

        [Description("Message showed to player when player joins a party. [{0} = other player]")]
        public string ConnectedToPartyMessage { get; set; } = "You are now in party with {0}";
        [Description("Message showed to player when other players leaves the party")]
        public string LeftPartyMessage { get; set; } = "You are no longer in party";

        [Description("Message showed to player when opponent disconects in match. [{0} = other player]")]
        public string MatchEndDisconnectMessage { get; set; } = "Opponent disconnected";
        [Description("Message showed to player when match ended due to unknwon reason")]
        public string MatchEndUnknownMessage { get; set; } = "Match ended due to unknown reason";
        [Description("Message showed to player when one of the players cancels the match")]
        public string MatchEndCancelledMessage { get; set; } = "One of the players cancelled the match";
        [Description("Message showed to player when all maps are in use")]
        public string MatchEndFullMessage { get; set; } = "All maps are currently in use try again later";

        public bool Debug { get; set; }
    }
}
