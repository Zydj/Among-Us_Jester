using HarmonyLib;

namespace Jester
{
    [HarmonyPatch]
    public static class EndGameManagerPatch
    {
        [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
        public static void Prefix()
        {
            if (!Jester.jesterEnabled)
            {
                return;
            }

            if (TempData.winners.Count <= 1 || !TempData.DidHumansWin(TempData.EndReason))
            {
                return;
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
        }
        [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
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
