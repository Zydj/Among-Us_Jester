using HarmonyLib;
using UnityEngine;

namespace Jester
{
    [HarmonyPatch]
    public static class GameOptionsMenuPatch
    {
        public static ToggleOption showJesterOption;
        public static GameOptionsMenu instance;

        public static OptionBehaviour option;

        public static float defaultBounds;

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
        public static void Postfix(GameOptionsMenu __instance)
        {
            instance = __instance;

            defaultBounds = __instance.GetComponentInParent<Scroller>().YBounds.max;

            option = __instance.Children[__instance.Children.Count - 1];

            CustomPlayerMenuPatch.AddOptions();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
        public static void Postfix1(GameOptionsMenu __instance)
        {
            if (showJesterOption != null)
            {
                showJesterOption.transform.position = option.transform.position - new Vector3(0, 0.5f, 0);

                __instance.GetComponentInParent<Scroller>().YBounds.max = defaultBounds + 0.5f;
            }
        }
    }
}
