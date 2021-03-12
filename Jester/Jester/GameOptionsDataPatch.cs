using HarmonyLib;

namespace Jester
{
    [HarmonyPatch(typeof(GameOptionsData))]
    public class GameOptionDataPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("NHJLMAAHKJF")]
        public static void Postfix1(GameOptionsData __instance, ref string __result, int MKGPLPMAKLO)
        {
            if (Jester.jesterEnabled)
            {
                __result += "Jester Role: On\n";
            }
            else
            {
                __result += "Jester Role: Off\n";
            }
        }
    }
}
