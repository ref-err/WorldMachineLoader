using HarmonyLib;
using OneShotMG;
using OneShotMG.src;
using OneShotMG.src.TWM;
using OneShotMG.src.TWM.Filesystem;
using WorldMachineLoader.Modding;

namespace WorldMachineLoader.Patches
{
    [HarmonyPatch(typeof(Game1))]
    class Game1Patch
    {
        // patching initialize to change versionstring and window title.
        [HarmonyPrefix]
        [HarmonyPatch("Initialize")]
        static void Initialize_PrefixPatch(Game1 __instance)
        {
            Game1.logMan.Log(LogManager.LogLevel.Info, "Hello from World Machine Loader!");
            Game1.VersionString = $"{Game1.VersionString} + WorldMachineLoader";

            __instance.Window.Title = $"OneShot [{Game1.VersionString}]";
        }
    }

    [HarmonyPatch(typeof(WindowManager))]
    class WindowManagerPatch
    {
        // patching RunFile to open Mod List window. idk why this window type exists,
        // because in oneshot's source this window type literally does nothing at all.
        [HarmonyPrefix]
        [HarmonyPatch("RunFile")]
        public static bool InterceptRunFile(object node, WindowManager __instance)
        {
            if (node is TWMFile file && file.program == LaunchableWindowType.DUMMY_FILE_FOR_TUTORIALS)
            {
                __instance.AddWindow(new ModListWindow());
            }
            return true;
        }

        // patching LoadDesktop method to create an icon with LaunchableWindowType of DUMMY_FILE_FOR_TUTORIALS.
        [HarmonyPostfix]
        [HarmonyPatch("LoadDesktop")]
        static void LoadDesktop_Postfix(TWMDesktopManager desktop, WindowManager __instance)
        {
            TWMFile tWMFile = new TWMFile("oneshot", "Mod List", LaunchableWindowType.DUMMY_FILE_FOR_TUTORIALS);
            __instance.FileSystem.Delete("/Mod List");
            __instance.FileSystem.WriteFile("/", tWMFile);
        }
    }
}
