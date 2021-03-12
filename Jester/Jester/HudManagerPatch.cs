using HarmonyLib;
using InnerNet;

namespace Jester
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudManagerPatch
    {
        static void Postfix(HudManager __instance)
        {
            
            if (AmongUsClient.Instance.GameState != InnerNetClient.Nested_0.Started)
                return;

            if (MeetingHud.Instance != null)
            {
                return;
            }

            if (!PlayerController.LocalPlayer.hasComponent("Jester"))
            {
                return;
            }            

            string currentTasks = __instance.TaskText.Text;
            
            currentTasks = currentTasks.Replace("[FFA1B8FF]Get voted off during meetings to win.\n[FFFFFFFF]Fake Tasks:\n", "");
            __instance.TaskText.Text = "[FFA1B8FF]Get voted off during meetings to win.\n[FFFFFFFF]Fake Tasks:\n" + currentTasks;

        }
    }
}
