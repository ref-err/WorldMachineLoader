using HarmonyLib;
using OneShotMG;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Events;

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
}
