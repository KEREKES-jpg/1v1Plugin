using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin1v1
{
    public static class Extensions
    {
        public static BoxFightManager FightManager(this Player _player)
        {
            if (BoxFightManager.Players.ContainsKey(_player))
            {
                return BoxFightManager.Players[_player];
            }

            if (_player.GameObject.TryGetComponent(out BoxFightManager component))
            {
                BoxFightManager.Players.Add(_player, component);
                return component;
            }

            var obj = _player.GameObject.AddComponent<BoxFightManager>();
            BoxFightManager.Players.Add(_player, obj);
            return obj;
        }
    }
}
