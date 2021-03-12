using HarmonyLib;

namespace Jester
{
    [HarmonyPatch(typeof(NormalPlayerTask), nameof(NormalPlayerTask.Complete))]
    public static class NormalPlayerTaskPatch
    {
        public static void Prefix()
        {
            if (!Jester.jesterEnabled)
            {
                return;
            }

            if (PlayerController.LocalPlayer.hasComponent("Jester"))
            {
                Jester.log.LogMessage("Do not update local task bar"); 
            }            
        }
    }
}
