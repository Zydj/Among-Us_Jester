using System;
using System.Collections.Generic;

namespace Jester
{
    public class Player
    {
        public PlayerControl playerdata;
        public List<string> components;
        public int PlayerId;

        public Player(PlayerControl playerdata)
        {
            this.playerdata = playerdata;
            components = new List<string>();
            this.PlayerId = playerdata.PlayerId;
        }

        public bool hasComponent(String name)
        {
            if (components.Contains(name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
