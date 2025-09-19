using HarmonyLib;
using OneShotMG;
using OneShotMG.src;
using OneShotMG.src.TWM;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Events.Lifecycle;
using WorldMachineLoader.API.Events.Environment;

namespace WorldMachineLoader.Patches
{
    [HarmonyPatch(typeof(Game1))]
    class GameInitPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("Initialize")]
        static void InitEvent()
        {
            EventBus.Invoke<Game1InitializeEvent>(new Game1InitializeEvent());
        }
    }

    [HarmonyPatch(typeof(WindowManager))]
    class TWMWindowManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("AddWindow")]
        static void AddWindowEvent(TWMWindow window)
        {
            EventBus.Invoke<TWMWindowAddedEvent>(new TWMWindowAddedEvent(window));
        }

        [HarmonyPostfix]
        [HarmonyPatch("RemoveWindow")]
        static void RemoveWindowEvent(TWMWindow window)
        {
            EventBus.Invoke<TWMWindowRemovedEvent>(new TWMWindowRemovedEvent(window));
        }

        [HarmonyPostfix]
        [HarmonyPatch("InitDesktop")]
        static void LoadDesktopEvent()
        {
            EventBus.Invoke<TWMDesktopLoadedEvent>(new TWMDesktopLoadedEvent());
        }
    }

    [HarmonyPatch(typeof(MasterSaveManager))]
    class MasterSaveManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("WriteFile")]
        static void SaveEvent()
        {
            EventBus.Invoke<SaveWrittenEvent>(new SaveWrittenEvent());
        }
    }
}
