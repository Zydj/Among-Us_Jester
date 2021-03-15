using System;
using System.Linq;
using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;

namespace Jester
{
    [HarmonyPatch]
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

        [HarmonyPatch(typeof(CustomPlayerMenu), nameof(CustomPlayerMenu.Close))]
        public static void Postfix()
        {
            deleteOptions(true);
        }

        [HarmonyPatch(typeof(CustomPlayerMenu), nameof(CustomPlayerMenu.OpenTab))]
        public static void Prefix(GameObject CCAHNLMBCOD)
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
