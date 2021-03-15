using HarmonyLib;

namespace Jester
{
    [HarmonyPatch]
    public class GameOptionDataPatch
    {
        [HarmonyPatch(typeof(GameOptionsData), nameof(GameOptionsData.Method_5))]
        public static void Postfix(ref string __result)
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
