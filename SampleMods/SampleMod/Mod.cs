using OneShotMG;
using System;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Interfaces;

namespace SampleMod
{
    internal class SampleMod : IMod
    {
        private ModContext context;

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
            Console.WriteLine("my favorite part of Console is when Console said \"its writingline time\" and started writingline all over the place.");
            //context.Logger.Log("my favorite part of Game1 is when Game1 said \"its initializing time\" and started initializing all over the place.");
        }
    }
}
