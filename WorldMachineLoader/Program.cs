using System;
using System.IO;

namespace WorldMachineLoader
{
    /// <summary>The main entry point for WorldMachineLoader, responsible for hooking into and launching the game.</summary>
    internal static class Program
    {
        /// <summary>The main entry point which hooks into and launches the game.</summary>
        /// <param name="args">The provided command line arguments.</param>
        internal static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Title = "World Machine Loader";
            Console.WriteLine("The World Machine Loader");

            // Set Current Working Directory to game's folder (where's this assembly located at)
            Directory.SetCurrentDirectory(Constants.GamePath);

            // Initialize the mod loader
            ModLoader.ModLoader modLoader = new ModLoader.ModLoader(args);

            if (!modLoader.CheckGameAssembly())
                Environment.Exit(1);

            // Load the mods
            modLoader.CheckMods();

            // Launch OneShotMG with mod loader
            modLoader.Start();
        }
    }
}
