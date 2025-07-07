using System;
using System.Windows.Forms;
using System.IO;
using WorldMachineLoader.Utils;

namespace WorldMachineLoader
{
    /// <summary>The main entry point for WorldMachineLoader, responsible for hooking into and launching the game.</summary>
    internal static class Program
    {
        /// <summary>The main entry point which hooks into and launches the game.</summary>
        /// <param name="args">The provided command line arguments.</param>
        internal static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            string logsDir = Path.Combine(Constants.GamePath, "logs");

            if (File.Exists(Path.Combine(logsDir, "crashed")))
            {
                DialogResult result = MessageBox.Show(
                    "WorldMachineLoader (or the game/one of the installed mods) has crashed. " +
                    "Would you like to enable Safe Mode?\nThis will stop mod loader from loading mods.",
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes) Globals.isSafeModEnabled = true;
                File.Delete(Path.Combine(logsDir, "crashed"));
            }

            if (!Directory.Exists(logsDir)) Directory.CreateDirectory(logsDir);

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var fileStream = new StreamWriter(Path.Combine(logsDir, $"log-{timestamp}.log")) { AutoFlush = true };
            DualWritter writer = new DualWritter(Console.Out, fileStream);

            Console.SetOut(writer);
            Console.SetError(writer);

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

        public static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            try
            {
                File.Create(Path.Combine(Constants.GamePath, "logs", "crashed"));
                string crashFile = Path.Combine(Constants.GamePath, "logs", $"crash-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log");
                string content = "Oops! Loader crashed!\n" +
                                $"Crashed at {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\n" +
                                $"Error: {ex.Message}\n" +
                                $"StackTrace:\n{ex.StackTrace}";
                File.WriteAllText(crashFile, content);
            }
            catch { }
        }
    }
}
