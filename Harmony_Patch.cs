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
        /* https://github.com/pardeike/Harmony/issues/186#issuecomment-491525920
        
        static class InjectedClass
        {
            public static void Start()
            {
                HarmonyInstance harmony = HarmonyInstance.Create("com.blah.somepatch");
                harmony.Patch(
                    original: AccessTools.Method(typeof(TargetClass), "GetRandomInteger"),
                    prefix: new HarmonyMethod(typeof(InjectedClass), nameof(InjectedClass.GetFixedInteger))
                );
            }

            public static bool GetFixedInteger(int seed, ref int __result)
            {
                __result = 42; // set return value
                return false; // don't run original method
                // if this method returned true, then the original method would also run
            }
        }


        */
        public static bool TYCAP_CheckTimeStopBlocked(bool isRelease, ref bool __result) { __result = false; return false; }
        public static bool TYCAP_CheckTimeMultiplierBlocked(ref bool __result) { __result = false; return false; }
        public static bool TYCAP_CheckEscapeBlocked(ref bool __result) { __result = false; return false; }
        public static bool TYCAP_CheckManaulBlocked(ref bool __result) { __result = false; return false; }
    }
}
