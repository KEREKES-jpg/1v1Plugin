using Exiled.API.Features;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Plugin1v1
{
    public enum MatchCancelReason
    {
        Disconnect,
        End,
        Cancelled,
        Full,
        Other
    }

    [Serializable]
    public class FightMap
    {
        public Vector3 spawn1 { get; set; }
        public Vector3 spawn2 { get; set; }
        public bool inUse;

        public FightMap(Vector3 _spawn1, Vector3 _spawn2)
        {
            spawn1 = _spawn1;
            spawn2 = _spawn2;
            inUse = false;
        }

        public FightMap()
        {

        }
    }

    public class BoxFightManager : MonoBehaviour
    {
        public static Dictionary<Exiled.API.Features.Player, BoxFightManager> Players = new Dictionary<Player, BoxFightManager>();

        public bool IsFighting { get; set; } = false;
        public bool IsSpectating { get; set; } = false;

        public int Round { get; set; }
        public int MyPoints { get; set; }
        public int OpponentPoints { get; set; }

        public bool InParty { get; set; }
        public bool IsPartyHost { get; set; }

        public Player Ply { get; set; }
        public Player Opponent { get; set; }

        public List<Player> Invites { get; set; } = new List<Player>();

        public string selectedKit;

        public static FightMap[] possibleMaps = new FightMap[]
        {

        };

        public FightMap CurrentMap;

        private void Awake()
        {
            Ply = Player.Get(gameObject);
        }

        public void StartMatch(Player opponent)
        {
            Invites.Clear();
            IsSpectating = false;
            Opponent = opponent;
            IsFighting = true;
            Round = 0;
            if (CurrentMap == null)
            {
                ExitMatch(MatchCancelReason.Full);
            }
            else
            {
                NewRound();
            }
        }

        public void StartParty(Player opponent, bool isHost)
        {
            IsPartyHost = isHost;
            Opponent = opponent;
            InParty = true;
            Ply.ShowHint(String.Format(Plugin.Instance.Config.ConnectedToPartyMessage, Opponent.Nickname));
        }

        public void CancelParty(bool _showMessage = false)
        {
            if (_showMessage) Ply.ShowHint(Plugin.Instance.Config.LeftPartyMessage);

            IsPartyHost = false;
            Opponent = null;
            InParty = false;
        }


        public void ExitMatch(MatchCancelReason reason = MatchCancelReason.Other)
        {
            if (reason == MatchCancelReason.End)
            {
                if (MyPoints > OpponentPoints)
                {
                    Ply.ShowHint(Plugin.Instance.Config.PlayerWonMessage);
                }
                else
                {
                    Ply.ShowHint(Plugin.Instance.Config.PlayerLostMessage);
                }

            }
            else if (reason == MatchCancelReason.Disconnect)
            {
                Ply.ShowHint(String.Format(Plugin.Instance.Config.MatchEndDisconnectMessage, Opponent.Nickname));
            }
            else if (reason == MatchCancelReason.Other)
            {
                Ply.ShowHint(Plugin.Instance.Config.MatchEndUnknownMessage);
            }
            else if (reason == MatchCancelReason.Cancelled)
            {
                Ply.ShowHint(Plugin.Instance.Config.MatchEndCancelledMessage);
            }
            else if (reason == MatchCancelReason.Full)
            {
                Ply.ShowHint(Plugin.Instance.Config.MatchEndFullMessage);
            }
            CancelParty(false);
            IsFighting = false;
            Ply.Role.Set(PlayerRoles.RoleTypeId.Tutorial);
            CurrentMap.inUse = false;
            CurrentMap = null;
            Round = 0;
            MyPoints = 0;
            OpponentPoints = 0;
            Opponent = null;
        }

        public void NewRound()
        {
            Round++;
            if (MyPoints == Plugin.Instance.Config.RequiredWinPoints)
            {
                Opponent.FightManager().ExitMatch(MatchCancelReason.End);
                ExitMatch(MatchCancelReason.End);
                return;
            }
            Ply.ClearInventory();
            Ply.Role.Set(PlayerRoles.RoleTypeId.ClassD);
            if (IsPartyHost)
            {
                Ply.Position = CurrentMap.spawn1;
            }
            else
            {
                Ply.Position = CurrentMap.spawn2;
            }

            Ply.ShowHint(String.Format(Plugin.Instance.Config.MatchNewRoundMessage, Ply.Nickname, Opponent.Nickname, MyPoints, OpponentPoints));
            if (IsPartyHost)
            {
                GiveKit(selectedKit);
            }
            else
            {
                GiveKit(Opponent.FightManager().selectedKit);
            }
        }

        void GiveKit(string desiredKit)
        {
            foreach (var n in Plugin.Instance.Config.Loudouts[desiredKit])
            {
                Ply.AddItem(n);
            }
        }

        IEnumerator RespawnCooldown()
        {
            yield return new WaitForSeconds(1);
            NewRound();
        }

        public void WonRound()
        {
            MyPoints++;
            StartCoroutine(RespawnCooldown());
        }

        public void LostRound()
        {
            OpponentPoints++;
            StartCoroutine(RespawnCooldown());
        }

        public void ReceiveInvite(Player ply)
        {
            Invites.Add(ply);
            Ply.ShowHint(string.Format(Plugin.Instance.Config.PlayerPartyInviteBroadcast, ply.Nickname));
        }

        public bool AcceptInvite(Player ply)
        {
            if (InParty)
            {
                Ply.ShowHint("You are already in party!");
                return false;
            }

            List<Player> playerToRemove = new List<Player>();

            foreach (var player in Invites)
            {
                if (player == null)
                {
                    playerToRemove.Add(player);
                    continue;
                }
                if (player == ply)
                {
                    if (player.FightManager().IsFighting)
                    {
                        Ply.ShowHint("Player is already in match!");
                        return false;
                    }
                    Ply.FightManager().StartParty(ply, false);
                    ply.FightManager().StartParty(Ply, true);

                    foreach (var player2 in playerToRemove)
                    {
                        Invites.Remove(player2);
                    }
                    return true;
                }
            }

            foreach (var player in playerToRemove)
            {
                Invites.Remove(player);
            }

            Ply.ShowHint("Could not find player!");
            return false;

        }

        public override string ToString()
        {
            string response = "";

            response += $"Retreiving {Ply.Nickname}'s info";

            if (Opponent == null) response += $"\n Opponent: null";
            else response += $"\n Opponent: {Opponent.Nickname}";

            response += $"\n IsFighting: {IsFighting}";
            response += $"\n Round: {Round}";
            response += $"\n Player's points: {MyPoints}";
            response += $"\n Opponents's points: {OpponentPoints}";
            response += $"\n Is in party: {InParty}";
            response += $"\n Is party host: {IsPartyHost}";
            response += $"\n Selected kit: {selectedKit}";
            response += $"\n Current map: {Ply.CurrentRoom}";

            return response;
        }

        public FightMap GetFreeMap()
        {
            List<FightMap> maps = new List<FightMap>();
            foreach (var map in possibleMaps)
            {
                if (!map.inUse)
                {
                    maps.Add(map);
                }
            }
            var selectedMapka = maps[UnityEngine.Random.Range(0, maps.Count)];
            selectedMapka.inUse = true;
            return selectedMapka;
        }
    }
}
