using HarmonyLib;

namespace Jester
{
    [HarmonyPatch(typeof(IntroCutscene.Nested_0), "MoveNext")]
    public static class IntroPatch
    {
        static bool Prefix(IntroCutscene.Nested_0 __instance)
        {
            if (!PlayerController.LocalPlayer.hasComponent("Jester"))
            {
                return true;
            }

            var jesterTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
            jesterTeam.Add(PlayerControl.LocalPlayer);
            __instance.yourTeam = jesterTeam;

            return true;
        }

        public static void Postfix(IntroCutscene.Nested_0 __instance)
        {

            Jester.jesterWon = false;

            if (PlayerController.getLocalPlayer().hasComponent("Jester"))
            {
                __instance.__this.Title.Text = "Jester";
                __instance.__this.Title.Color = Jester.jesterColor;
                __instance.__this.ImpostorText.Text = "Get voted off to win";
                __instance.__this.BackgroundBar.material.color = Jester.jesterColor;
            }
        }
    }
}
