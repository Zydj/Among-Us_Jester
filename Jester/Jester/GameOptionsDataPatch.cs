using HarmonyLib;

namespace Jester
{
    [HarmonyPatch(typeof(GameOptionsData))]
    public static class GameOptionsDataPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("NHJLMAAHKJF")]
        public static void Postfix(GameOptionsData __instance, ref string __result, int MKGPLPMAKLO)
        {
            /*
            Jester.log.LogMessage("ShortTasks: " + __instance.NumShortTasks);
            Jester.log.LogMessage("LongTasks: " + __instance.NumLongTasks);
            Jester.log.LogMessage("CommonTasks: " + __instance.NumCommonTasks);     
            */      
            
        }
    }
}