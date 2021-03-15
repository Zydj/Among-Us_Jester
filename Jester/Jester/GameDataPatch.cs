using HarmonyLib;
using Hazel;
using UnhollowerBaseLib;

namespace Jester
{
    [HarmonyPatch]
    public static class GameDataPatch
    {        
        public enum RPC
        {
            SetTasks = 29
        }

        [HarmonyPatch(typeof(GameData), nameof(GameData.HandleRpc))]
        static void Postfix(byte ACCJCEHMKLN, MessageReader HFPCBBHJIPJ)
        {
            Jester.log.LogMessage("GameData RPC is: " + ACCJCEHMKLN);            
        }

        [HarmonyPatch(typeof(GameData), nameof(GameData.RpcSetTasks))]
        static void Postfix(byte FEFHEFFFBBI, Il2CppStructArray<byte> NMIBFBOGFCJ)
        {
            if (PlayerController.getPlayerById(FEFHEFFFBBI).hasComponent("Jester"))
            {
                GameData.Instance.GetPlayerById(FEFHEFFFBBI).DEPNCDAJFGJ= null;
                
            }           
        }        
    }
}
