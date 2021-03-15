using HarmonyLib;
using UnityEngine;
using System.Linq;
using UnhollowerBaseLib;

namespace Jester
{
    [HarmonyPatch]
    public static class GameOptionsMenuPatch
    {
        public static ToggleOption showJesterOption;
        public static GameOptionsMenu instance;

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
        public static void Postfix(GameOptionsMenu __instance)
        {
            instance = __instance;
            CustomPlayerMenuPatch.AddOptions();
        }

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
        public static void Postfix1(GameOptionsMenu __instance)
        {
            OptionBehaviour option = __instance.Children[__instance.Children.Count - 2];
            if (showJesterOption != null)
            {
                showJesterOption.transform.position = option.transform.position - new Vector3(0, 0.5f, 0);
            }
        }
    }
}
