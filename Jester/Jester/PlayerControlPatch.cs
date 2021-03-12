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
        SetLocalPlayers = 41

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

                        break;
                    }

                case (byte)RPC.CompleteTask:
                    {
                        Player p = PlayerController.getPlayerById(HFPCBBHJIPJ.ReadByte());

                        PlayerControl player = PlayerController.getPlayerControlById(HFPCBBHJIPJ.ReadByte());
                        Jester.log.LogMessage(player.nameText.Text + " completed a task");

                        if (!p.hasComponent("Jester"))
                        {
                            return;                       
                        }

                        Jester.log.LogMessage("Do not update all taskbars");                        

                        break;
                    }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), "RpcSetInfected")]
        public static void Postfix(Il2CppReferenceArray<GameData.Nested_1> FMAOEJEHPAO)
        {
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
    }
}
