using HarmonyLib;
using System.Collections.Generic;
using Hazel;
using UnhollowerBaseLib;
using System.Linq;
using InnerNet;

namespace Jester
{
    enum RPC
    {
        CompleteTask = 1
    }
    enum CustomRPC
    {

        SetJester = 40,
        JesterWin = 50,
        SetLocalPlayers = 41,
        SyncCustomSettings = 42,
        SetLastPlayerTask = 43

    }

    [HarmonyPatch]
    public static class PlayerControlPatch
    {
        public static List<Player> getCrewMates(Il2CppReferenceArray<GameData.Nested_1> infection)
        {
            List<Player> Crewmates = new List<Player>();
            foreach (Player player in PlayerController.players)
            {

                bool isInfected = false;
                foreach (GameData.Nested_1 infected in infection)
                {

                    if (player.playerdata.PlayerId == infected.IBJBIALCEKB.PlayerId)
                    {
                        isInfected = true;
                        break;
                    }
                }

                if (!isInfected)
                {
                    Crewmates.Add(player);
                }
            }
            return Crewmates;
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        static void Postfix(byte ACCJCEHMKLN, MessageReader HFPCBBHJIPJ)
        {            
            Jester.log.LogMessage("RPC is:" + ACCJCEHMKLN);
            switch (ACCJCEHMKLN)
            {
                case (byte)CustomRPC.SetJester:
                    {
                        Jester.log.LogMessage("Setting Jester");
                        PlayerController.InitPlayers();
                        Player p = PlayerController.getPlayerById(HFPCBBHJIPJ.ReadByte());
                        p.components.Add("Jester");                        
                        break;
                    }

                case (byte)CustomRPC.JesterWin:
                    {
                        Jester.jesterWon = true;
                        Jester.log.LogMessage("Handling Jester Victory");
                        Player player = PlayerController.getPlayerByRole("Jester");

                        foreach (PlayerControl playerCon in PlayerControl.AllPlayerControls)
                        {
                            if (playerCon.PlayerId == player.PlayerId)
                            {
                                playerCon.Data.AKOHOAJIHBE = true;
                                playerCon.Data.LGEGJEHCFOG = false;
                            }
                            else
                            {
                                Jester.log.LogMessage(playerCon.nameText.Text + " is impostor: " + playerCon.Data.LGEGJEHCFOG);
                                playerCon.RemoveInfected();
                                playerCon.Die(DeathReason.Exile);
                                playerCon.Data.AKOHOAJIHBE = true;
                                playerCon.Data.LGEGJEHCFOG = false;
                            }
                        }
                        break;
                    }

                case (byte)CustomRPC.SetLocalPlayers:
                    {
                        Jester.localPlayers.Clear();
                        Jester.localPlayer = PlayerControl.LocalPlayer;

                        var localPlayerBytes = HFPCBBHJIPJ.ReadBytesAndSize();

                        foreach (var id in localPlayerBytes)
                        {
                            foreach (var player in PlayerControl.AllPlayerControls)
                            {
                                if (player.PlayerId == id)
                                {
                                    Jester.localPlayers.Add(player);
                                }
                            }
                        }
                        Jester.log.LogMessage(Jester.localPlayer.nameText.Text);                       
                        break;
                    }               

                case (byte)CustomRPC.SyncCustomSettings:
                    {
                        Jester.jesterEnabled = HFPCBBHJIPJ.ReadBoolean();
                        break;
                    }

                case (byte)CustomRPC.SetLastPlayerTask:
                    {
                        Jester.log.LogMessage("Setting last player task");
                        Jester.lastPlayerTask = PlayerController.getPlayerById(HFPCBBHJIPJ.ReadByte());
                        break;
                    }
            }            
        }

        [HarmonyPatch(typeof(PlayerControl), "CompleteTask")]
        public static bool Prefix(uint CBAHIKLHCAO, PlayerControl __instance)
        {
            if (!Jester.jesterEnabled)
            {
                return true;
            }
            
            Jester.log.LogMessage(__instance.nameText.Text + " Completed a task");

            

            foreach (PlayerTask task in __instance.myTasks)
            {
                if (task.Id == CBAHIKLHCAO)
                {
                    Jester.log.LogMessage("Sending last player task RPC");
                    Jester.lastPlayerTask = PlayerController.getPlayerById(__instance.PlayerId);
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetLastPlayerTask, Hazel.SendOption.Reliable);
                    writer.Write(__instance.PlayerId);
                    writer.EndMessage();
                }
            }

            Jester.log.LogMessage("Checking local taskbar");

            if (!Jester.lastPlayerTask.hasComponent("Jester"))
            {
                return true;
            }

            Jester.log.LogMessage("Do not update task bar");
            return false;
        }

        [HarmonyPatch(typeof(PlayerControl), "RpcSetInfected")]
        public static void Postfix(Il2CppReferenceArray<GameData.Nested_1> FMAOEJEHPAO)
        {
            if (!Jester.jesterEnabled)
            {
                return;
            }

            PlayerController.InitPlayers();           

            List<Player> crewmates = getCrewMates(FMAOEJEHPAO);
            var jesterid = new System.Random().Next(0, crewmates.Count);

            Player jester = crewmates[jesterid];
            MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJester, Hazel.SendOption.Reliable);
            writer.Write(jester.playerdata.PlayerId);
            writer.EndMessage();
            jester.components.Add("Jester");

            Jester.localPlayers.Clear();
            Jester.localPlayer = PlayerControl.LocalPlayer;

            foreach (var player in PlayerControl.AllPlayerControls)
            {
                Jester.localPlayers.Add(player);
            }

            writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetLocalPlayers, Hazel.SendOption.Reliable);
            writer.WriteBytesAndSize(Jester.localPlayers.Select(player => player.PlayerId).ToArray());
            writer.EndMessage();

        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
        public static void Postfix(PlayerControl __instance)
        {
            if (!Jester.jesterEnabled)
            {
                return;
            }

            if (AmongUsClient.Instance.GameState != InnerNetClient.Nested_0.Started)
                return;

            if (__instance == null)
            {
                return;
            }

            if (!Jester.introDone)
            {
                return;
            }

            if (PlayerController.getLocalPlayer().hasComponent("Jester"))
            {
                PlayerControl.LocalPlayer.nameText.Color = Jester.jesterColor;
            }
        }

        [HarmonyPatch(typeof(PlayerControl), "RpcSyncSettings")]
        public static void Postfix(GameOptionsData OMFKMPLOPPM)
        {
            if (PlayerControl.AllPlayerControls.Count > 1)
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SyncCustomSettings, Hazel.SendOption.Reliable);
                writer.Write(Jester.jesterEnabled);
                writer.EndMessage();
            }
        }
    }


}
