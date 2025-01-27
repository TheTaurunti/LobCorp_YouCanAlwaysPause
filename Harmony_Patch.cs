using System;
using System.Collections.Generic;
using System.IO;
using Harmony;
using UnityEngine;

namespace Taurunti_YouCanAlwaysPause
{
    public class Harmony_Patch
    {
        public Harmony_Patch()
        {
            try
            {
                HarmonyInstance harmonyInstance = HarmonyInstance.Create("Taurunti_YouCanAlwaysPause");

                harmonyInstance.Patch(
                    original: typeof(BossBird).GetMethod("EscapeInit", AccessTools.all), 
                    prefix: null, 
                    postfix: new HarmonyMethod(typeof(Harmony_Patch).GetMethod("TYCAP_ApocalypseBird_EscapeInit"))
                );

                var methodsToReplace = new List<string>
                {
                    "CheckTimeStopBlocked",
                    "CheckTimeMultiplierBlocked",
                    "CheckEscapeBlocked",
                    "CheckManaulBlocked"
                };
                foreach (var oldMethodName in methodsToReplace)
                {
                    string newMethodName = "TYCAP_" + oldMethodName;

                    harmonyInstance.Patch(
                        original: typeof(PlaySpeedSettingUI).GetMethod(oldMethodName, AccessTools.all), 
                        prefix: new HarmonyMethod(typeof(Harmony_Patch).GetMethod(newMethodName)),
                        postfix: null
                    );
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText(Application.dataPath + "/BaseMods/Taurunti_YouCanAlwaysPause_ERROR.txt", ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        // How to do method replacement
        // https://github.com/pardeike/Harmony/issues/186#issuecomment-491525920

        // Removes apoc bird from "things which interfere with pausing" immediately after it is (presumably) added
        public static void TYCAP_ApocalypseBird_EscapeInit(BossBird __instance)
        {
            PlaySpeedSettingUI.instance.ReleaseSetting(__instance);
        }

        public static bool TYCAP_CheckTimeStopBlocked(bool isRelease, ref bool __result) { __result = false; return false; }
        public static bool TYCAP_CheckTimeMultiplierBlocked(ref bool __result) { __result = false; return false; }
        public static bool TYCAP_CheckEscapeBlocked(ref bool __result) { __result = false; return false; }
        public static bool TYCAP_CheckManaulBlocked(ref bool __result) { __result = false; return false; }
    }
}
