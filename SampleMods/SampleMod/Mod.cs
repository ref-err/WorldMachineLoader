using OneShotMG;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Interfaces;

namespace SampleMod
{
    internal class SampleMod : IMod
    {
        private static ModContext context;

        public void OnLoad(ModContext modContext)
        {
            context = modContext;
            context.Logger.Log("Mod loading!");
        }

        public void OnShutdown()
        {
            context.Logger.Log("Mod shutdown!");
        }

        [GamePatch(typeof(Game1), "Initialize", PatchType.Prefix)]
        private static void SomeInitPatch()
        {
            context.Logger.Log("my favorite part of Console is when Console said \"its writingline time\" and started writingline all over the place.");
            foreach (var modMetadata in APIServices.ModInfoProvider.GetLoadedMods())
            {
                context.Logger.Log(modMetadata.ID);
            }
        }
    }
}
