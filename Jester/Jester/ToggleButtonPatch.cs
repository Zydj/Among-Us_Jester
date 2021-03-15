using HarmonyLib;

namespace Jester
{
    [HarmonyPatch]
    public static class ToggleButtonPatch
    {
        [HarmonyPatch(typeof(ToggleOption), nameof(ToggleOption.Toggle))]
        public static bool Prefix(ToggleOption __instance)
        {
            if (__instance.TitleText.Text == "Jester Role")
            {
                Jester.jesterEnabled = !Jester.jesterEnabled;
                PlayerControl.LocalPlayer.RpcSyncSettings(PlayerControl.GameOptions);

                __instance.oldValue = Jester.jesterEnabled;
                __instance.CheckMark.enabled = Jester.jesterEnabled;

                return false;
            }
            return true;
        }
    }
}
