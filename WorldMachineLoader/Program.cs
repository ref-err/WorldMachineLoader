using System;
using System.Windows.Forms;
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
            if (File.Exists(Path.Combine(Constants.ModsPath, "crashed")))
            {
                DialogResult result = MessageBox.Show(
                    "WorldMachineLoader (or the game/one of the installed mods) has crashed. " +
                    "Would you like to enable Safe Mode?\nThis will stop mod loader from loading mods.",
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes) Globals.isSafeModEnabled = true;
                File.Delete(Path.Combine(Constants.ModsPath, "crashed"));
            }

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
