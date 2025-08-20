using System;
using System.IO;
using System.Windows.Forms;
using System.Security.Principal;
using WorldMachineLoader.Utils;
using WorldMachineLoader.ModLoader;
using WorldMachineLoader.API.Utils;

namespace WorldMachineLoader
{
    /// <summary>The main entry point for WorldMachineLoader, responsible for hooking into and launching the game.</summary>
    internal static class Program
    {
        /// <summary>Loader's logger.</summary>
        public static Logger Logger = new Logger("WML/Program");

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
            DualWriter writer = new DualWriter(Console.Out, fileStream);

            Console.SetOut(writer);
            Console.SetError(writer);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Title = "World Machine Loader";
            Console.WriteLine("The World Machine Loader");

            if (IsRunningAsAdmin() && !ModSettings.Instance.IgnoreAdminCheck)
            {
                Logger.Log("WorldMachineLoader cannot be run with administrator privileges.", Logger.LogLevel.Error);
                Logger.Log("Please restart the program without administrator privileges.", Logger.LogLevel.Error);
                Logger.Log("If this is intended, set \"ignore_admin_check\" to true in mods\\settings.json.", Logger.LogLevel.Error);
                MessageBox.Show("WorldMachineLoader cannot be run with administrator privileges." +
                                "Please restart the program without administrator privileges." +
                                "If this is intended, set \"ignore_admin_check\" to true in mods\\settings.json.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (IsRunningAsAdmin() && ModSettings.Instance.IgnoreAdminCheck)
            {
                Logger.Log("WorldMachineLoader is running with administrator privileges, and \"ignore_admin_check\" is enabled in config.", Logger.LogLevel.Warn);
                Logger.Log("Proceeding as requested. Be careful!", Logger.LogLevel.Warn);
            }

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

        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
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

        static bool IsRunningAsAdmin()
        {
            using (WindowsIdentity id = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(id);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }
}
