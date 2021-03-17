using HarmonyLib;

namespace Jester
{
    [HarmonyPatch]
    public static class IntroCutscenePatch
    {
        [HarmonyPatch(typeof(IntroCutscene.Nested_0), nameof(IntroCutscene.Nested_0.MoveNext))]
        static void Prefix(IntroCutscene.Nested_0 __instance)
        {
            if (!Jester.jesterEnabled)
            {
                return;
            }

            if (PlayerController.LocalPlayer.hasComponent("Jester"))
            {
                var jesterTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                jesterTeam.Add(PlayerControl.LocalPlayer);
                __instance.yourTeam = jesterTeam;
            }            
        }

        [HarmonyPatch(typeof(IntroCutscene.Nested_0), nameof(IntroCutscene.Nested_0.MoveNext))]
        public static void Postfix(IntroCutscene.Nested_0 __instance)
        {
            if (!Jester.jesterEnabled)
            {
                return;
            }

            Jester.jesterWon = false;

            if (PlayerController.getLocalPlayer().hasComponent("Jester"))
            {
                __instance.__this.Title.Text = "Jester";
                __instance.__this.Title.Color = Jester.jesterColor;
                __instance.__this.ImpostorText.Text = "Get voted off to win";
                __instance.__this.BackgroundBar.material.color = Jester.jesterColor;
            }

            Jester.introDone = true;
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
        public static void Postfix()
        {
            Jester.introDone = true;
        }
    }
}
