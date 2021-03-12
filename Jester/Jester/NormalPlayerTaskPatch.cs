using HarmonyLib;

namespace Jester
{
    [HarmonyPatch(typeof(NormalPlayerTask), nameof(NormalPlayerTask.Complete))]
    public static class NormalPlayerTaskPatch
    {
        public static void Prefix()
        {
            if (!PlayerController.LocalPlayer.hasComponent("Jester"))
            {
                return;
            }
            
            Jester.log.LogMessage("Do not update local task bar");
        }
    }
}
