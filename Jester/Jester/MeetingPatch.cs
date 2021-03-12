using HarmonyLib;
using Hazel;

namespace Jester
{
    [HarmonyPatch]
    public static class MeetingPatch
    {
        [HarmonyPatch(typeof(UnityEngine.Object), nameof(UnityEngine.Object.Destroy),
        new[] { typeof(UnityEngine.Object) })]
        static void Prefix(UnityEngine.Object obj)
        {
            if (!Jester.jesterEnabled)
            {
                return;
            }

            if (ExileController.Instance == null || obj != ExileController.Instance.gameObject)
            {
                return;
            }

            if (ExileController.Instance.exiled != null)
            {
                PlayerControl exiled = ExileController.Instance.exiled.CBEJMNMADDB;

                Player player = PlayerController.getPlayerById(exiled.PlayerId);

                if (player.hasComponent("Jester"))
                {
                    Jester.log.LogMessage("Jester Wins");
                    MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.JesterWin, Hazel.SendOption.Reliable);
                    writer.EndMessage();
                }

            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
        public static void Postfix(MeetingHud __instance)
        {
            if (!Jester.jesterEnabled)
            {
                return;
            }

            PlayerControl jester = PlayerController.getPlayerControlByRole("Jester");            
            if (jester == null)
            {
                return;
            }

            foreach (PlayerVoteArea playerArea in __instance.playerStates)
            {                
                if (jester.nameText.Text.Equals(playerArea.NameText.Text) && PlayerControl.LocalPlayer.PlayerId == jester.PlayerId)
                {                    
                    playerArea.NameText.Color = Jester.jesterColor;
                }
            }
        }
    }
}
