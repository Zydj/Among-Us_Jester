using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using System.Collections.Generic;
using UnityEngine;

namespace Jester
{
    /*
     * TODO:
     *      - Jester tasks not counted towards total tasks    
     *      - Jester menu options       
     */



    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class Jester : BasePlugin
    {
        public static bool jesterWon = false;
        public const string Id = "org.bepinex.plugins.Jester";
        public static BepInEx.Logging.ManualLogSource log;

        public static PlayerControl localPlayer = null;
        public static List<PlayerControl> localPlayers = new List<PlayerControl>();
        public static bool introDone = false;
        public static bool jesterEnabled = true;


        public static Color jesterColor = new Color(1, (float)(63.0 / 100), (float)(72.0 / 100));

        public Harmony Harmony { get; } = new Harmony(Id);       

        public ConfigEntry<string> Name { get; private set; }

        public override void Load()
        {
            Name = Config.Bind("Fake", "Name", ":>");

            log = Log;
            log.LogMessage("Jester Mod Loaded");

            Harmony.PatchAll();
        }
    }
}
