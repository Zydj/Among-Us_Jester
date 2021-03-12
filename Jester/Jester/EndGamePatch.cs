using HarmonyLib;

namespace Jester
{
    [HarmonyPatch(typeof(EndGameManager), "SetEverythingUp")]
    public static class EndGamePatch
    {
       
        public static bool Prefix()
        {

            if (!Jester.jesterEnabled)
            {
                return true;
            }

            if (TempData.winners.Count <= 1 || !TempData.DidHumansWin(TempData.EndReason))
            {
                return true;
            }

            if (Jester.jesterWon)
            {
                Jester.log.LogMessage("Clearing winners");
                TempData.winners.Clear();
                
                Player jester = PlayerController.getPlayerByRole("Jester");               

                foreach (PlayerControl playerCon in Jester.localPlayers)
                {
                    if (playerCon.PlayerId == jester.PlayerId)
                    {
                        Jester.log.LogMessage("Adding winners");

                        TempData.winners.Add(new WinningPlayerData(playerCon.Data));
                    }
                }
            }
                    return true;
        }

        public static void Postfix(EndGameManager __instance)
        {
            if (!Jester.jesterEnabled)
            {
                return;
            }

            if (Jester.jesterWon)
            {
                Player player = PlayerController.getPlayerById(PlayerControl.LocalPlayer.PlayerId);

                if (player.hasComponent("Jester"))
                {
                    __instance.WinText.Text = "Victory";
                    __instance.WinText.Color = Palette.Blue;
                    __instance.BackgroundBar.material.color = Palette.CrewmateBlue;
                }
                else
                {
                    __instance.WinText.Text = "Defeat";
                    __instance.WinText.Color = Jester.jesterColor;
                    __instance.BackgroundBar.material.color = Jester.jesterColor;
                }
            }
        }
    }
}
