using System.Collections.Generic;

namespace Jester
{
    public static class PlayerController
    {
        public static List<Player> players;
        public static Player LocalPlayer;

        public static void InitPlayers()
        {
            players = new List<Player>();

            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                Player p = new Player(player);
                if (player.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                {
                    LocalPlayer = p;
                }
                players.Add(p);
            }
        }

        public static Player getPlayerById(byte id)
        {
            if (players == null) return null;

            foreach (Player player in players)
            {
                if (player.PlayerId == id)
                {
                    return player;
                }
            }

            return null;
        }

        public static Player getLocalPlayer()
        {
            if (players == null) return null;
            foreach (Player player in players)
            {
                if (player.playerdata == PlayerControl.LocalPlayer)
                {
                    return player;
                }
            }
            return null;
        }

        public static Player getPlayerByRole(string role)
        {
            foreach (Player player in players)
            {
                if (player.hasComponent(role))
                {
                    return player;
                }
            }
            return null;
        }

        public static PlayerControl getPlayerControlByRole(string role)
        {
            Player player = getPlayerByRole(role);
            foreach (PlayerControl playerCon in PlayerControl.AllPlayerControls)
            {
                if (playerCon.PlayerId == player.PlayerId)
                {
                    return playerCon;
                }
            }

            return null;
        }

        public static PlayerControl getPlayerControlById(byte id)
        {
            foreach (PlayerControl playerCon in PlayerControl.AllPlayerControls)
            {
                if (playerCon.PlayerId == id)
                {
                    return playerCon;
                }
            }
            return null;
        }
    }
}
