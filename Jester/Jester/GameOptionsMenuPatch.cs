using HarmonyLib;
using UnityEngine;
using System.Linq;
using UnhollowerBaseLib;

namespace Jester
{
    [HarmonyPatch(typeof(GameOptionsMenu))]
    public static class GameOptionsMenuPatch
    {
        public static ToggleOption showJesterOption;
        public static GameOptionsMenu instance;

        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void Postfix1(GameOptionsMenu __instance)
        {
            instance = __instance;
            CustomPlayerMenuPatch.AddOptions();
        }

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        public static void Postfix2(GameOptionsMenu __instance)
        {
            OptionBehaviour option = __instance.Children[__instance.Children.Count - 2];
            if (showJesterOption != null)
            {
                showJesterOption.transform.position = option.transform.position - new Vector3(0, 0.5f, 0);
            }
        }
    }

    [HarmonyPatch]
    public static class ToggleButtonPatch
    {
        [HarmonyPatch(typeof(ToggleOption), "Toggle")]
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

    [HarmonyPatch(typeof(CustomPlayerMenu))]
    public class CustomPlayerMenuPatch
    {

        public static void deleteOptions(bool destroy)
        {
            if (GameOptionsMenuPatch.showJesterOption != null)
            {
                GameOptionsMenuPatch.showJesterOption.gameObject.SetActive(false);

                if (destroy)
                {
                    GameObject.Destroy(GameOptionsMenuPatch.showJesterOption);
                    GameOptionsMenuPatch.showJesterOption = null;
                }
            }
        }

        public static void AddOptions()
        {
            if (GameOptionsMenuPatch.showJesterOption == null)
            {
                ToggleOption showAnonymousVotes = GameObject.FindObjectsOfType<ToggleOption>().ToList().Where(x => x.TitleText.Text == "Anonymous Votes").First();
                GameOptionsMenuPatch.showJesterOption = GameObject.Instantiate(showAnonymousVotes);

                Jester.log.LogMessage(GameOptionsMenuPatch.instance.Children.Count);

                OptionBehaviour[] options = new OptionBehaviour[GameOptionsMenuPatch.instance.Children.Count + 1];
                GameOptionsMenuPatch.instance.Children.ToArray().CopyTo(options, 0);
                options[options.Length - 1] = GameOptionsMenuPatch.showJesterOption;
                GameOptionsMenuPatch.instance.Children = new Il2CppReferenceArray<OptionBehaviour>(options);
            }
            else
            {
                GameOptionsMenuPatch.showJesterOption.gameObject.SetActive(true);
            }
            GameOptionsMenuPatch.showJesterOption.TitleText.Text = "Jester Role";
            GameOptionsMenuPatch.showJesterOption.oldValue = Jester.jesterEnabled;
            GameOptionsMenuPatch.showJesterOption.CheckMark.enabled = Jester.jesterEnabled;

        }

        [HarmonyPostfix]
        [HarmonyPatch("Close")]
        public static void Postfix1(CustomPlayerMenu __instance, bool JMNGFPKKPDF)
        {
            deleteOptions(true);
        }

        [HarmonyPrefix]
        [HarmonyPatch("OpenTab")]
        public static void Prefix1(GameObject CCAHNLMBCOD)
        {
            if (CCAHNLMBCOD.name == "GameGroup" && GameOptionsMenuPatch.instance != null)
            {
                AddOptions();
            }
            else
            {
                deleteOptions(false);
            }
        }
    }
}
