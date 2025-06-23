using HarmonyLib;
using OneShotMG;
using OneShotMG.src;
using OneShotMG.src.TWM;
using WorldMachineLoader.Modding;

namespace WorldMachineLoader.Patches
{
    [HarmonyPatch(typeof(Game1))]
    class Game1Patch
    {
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
        // patching loaddesktop method to open a window after desktop has loaded.
        // i'll try to create a desktop icon to open mod list window instead.
        [HarmonyPostfix]
        [HarmonyPatch("LoadDesktop")]
        static void LoadDesktop_Postfix(TWMDesktopManager desktop, WindowManager __instance)
        {
            __instance.AddWindow(new ModListWindow("Mod List"));
        }
    }
}
