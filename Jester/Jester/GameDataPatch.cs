using HarmonyLib;
using Hazel;
using UnhollowerBaseLib;

namespace Jester
{
    [HarmonyPatch]
    public static class GameDataPatch
    {
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
