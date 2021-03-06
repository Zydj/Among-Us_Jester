using HarmonyLib;
using InnerNet;

namespace Jester
{
    [HarmonyPatch]
    public static class HudManagerPatch
    {
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        static void Postfix(HudManager __instance)
        {
            if (!Jester.jesterEnabled)
            {
                return;
            }

            if (AmongUsClient.Instance.GameState != InnerNetClient.Nested_0.Started)
                return;

            if (!Jester.introDone)
            {
                return;
            }

            if (PlayerController.LocalPlayer.hasComponent("Jester"))
            {
                string currentTasks = __instance.TaskText.Text;

                currentTasks = currentTasks.Replace("[FFA1B8FF]Get voted off during meetings to win.\n[FFFFFFFF]Fake Tasks:\n", "");
                __instance.TaskText.Text = "[FFA1B8FF]Get voted off during meetings to win.\n[FFFFFFFF]Fake Tasks:\n" + currentTasks;
            }
        }
    }
}
