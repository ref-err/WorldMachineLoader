using OneShotMG.src.TWM;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Events.Lifecycle;
using WorldMachineLoader.API.Events.Environment;
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
            EventBus.Subscribe<TWMWindowAddedEvent>(OnWindowAdded);
            EventBus.Subscribe<TWMWindowRemovedEvent>(OnWindowRemoved);
            EventBus.Subscribe<SaveWrittenEvent>(OnSaveWritten);
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

        private void OnWindowAdded(TWMWindowAddedEvent e)
        {
            TWMWindow window = e.WindowInstance;
            context.Logger.Log(window.WindowTitle);
        }

        private void OnWindowRemoved(TWMWindowRemovedEvent e)
        {
            TWMWindow window = e.WindowInstance;
            context.Logger.Log(window.WindowTitle);
        }

        private void OnSaveWritten(SaveWrittenEvent e)
        {
            context.Logger.Log("Save written..?");
        }
    }
}
