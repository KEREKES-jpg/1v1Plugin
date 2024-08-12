using Exiled.API.Features;
using Exiled.API.Features.Doors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin1v1.Handlers
{
    public class ServerHandler
    {
        public void OnRoundStart()
        {
            Round.IsLocked = true;
            BoxFightManager.possibleMaps = Plugin.Instance.Config.Maps;

            foreach (Door door in Door.List)
            {
                door.ChangeLock(Exiled.API.Enums.DoorLockType.AdminCommand);
            }
        }
    }
}
