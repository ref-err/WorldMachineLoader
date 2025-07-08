using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Events;
using WorldMachineLoader.API.Interfaces;
using static WorldMachineLoader.API.Core.APIServices;

namespace SampleMod
{
    internal class SampleMod : IMod
    {
        private ModContext context;

        public void OnLoad(ModContext modContext)
        {
            context = modContext;
            EventBus.Subscribe<Game1InitializeEvent>(OnGame1Initialize);
            context.Logger.Log("Mod loading!");
        }

        public void OnShutdown()
        {
            context.Logger.Log("Mod shutdown!");
        }

        private void OnGame1Initialize(Game1InitializeEvent e)
        {
            context.Logger.Log("my favorite part of Logger is when Logger said \"its logging time\" and started logging all over the place.");
            foreach (var modMetadata in ModInfoProvider.GetLoadedMods())
            {
                context.Logger.Log(modMetadata.ID);
            }
            var modByName = ModInfoProvider.FindModByName("Sample Mod");
            var modByID = ModInfoProvider.FindModByID("net.referr.samplemod");
            
            context.Logger.Log(modByName.ID);
            context.Logger.Log(modByID.Name);
        }
    }
}
