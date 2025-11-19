using System;
using System.IO;
using System.Windows.Forms;
using System.Security.Principal;
using WorldMachineLoader.Utils;
using WorldMachineLoader.Loader;
using WorldMachineLoader.API.Utils;
using System.Net;

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
            Console.Title = "WorldMachineLoader";
            Console.WriteLine($"WorldMachineLoader {Constants.Version}");
            if (!Directory.Exists(Constants.ModsPath)) Directory.CreateDirectory(Constants.ModsPath);

            LoggerManager.CurrentLevel = ModSettings.Instance.VerbosityLevel;

            if (!ModSettings.Instance.DisableUpdateCheck)
            {
                Logger.Log("Checking for updates... (You can disable this in \"settings.json\")", Logger.LogLevel.Info, Logger.VerbosityLevel.Minimal);
                if (CheckForUpdate(out string remote, out string updateErr))
                {
                    Logger.Log($"Update available ({remote})! You can download the latest update at GitHub Releases page.", Logger.LogLevel.Info, Logger.VerbosityLevel.Minimal);
                }
                else
                {
                    if (remote != null)
                        Logger.Log("You have the latest version.", Logger.LogLevel.Info, Logger.VerbosityLevel.Minimal);
                    else
                        Logger.Log($"Error checking for updates:\n{updateErr}", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                }
            }
            else
            {
                Logger.Log("Update check disabled.", Logger.LogLevel.Info, Logger.VerbosityLevel.Minimal);
            }

            if (IsRunningAsAdmin() && !ModSettings.Instance.IgnoreAdminCheck)
            {
                Logger.Log("WorldMachineLoader cannot be run with administrator privileges.", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                Logger.Log("Please restart the program without administrator privileges.", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                Logger.Log("If this is intended, set \"ignore_admin_check\" to true in mods\\settings.json.", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                MessageBox.Show("WorldMachineLoader cannot be run with administrator privileges.\n" +
                                "Please restart the program without administrator privileges.\n" +
                                "If this is intended, set \"ignore_admin_check\" to true in mods\\settings.json.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (IsRunningAsAdmin() && ModSettings.Instance.IgnoreAdminCheck)
            {
                Logger.Log("WorldMachineLoader is running with administrator privileges, and \"ignore_admin_check\" is enabled in config.", Logger.LogLevel.Warn, Logger.VerbosityLevel.Minimal);
                Logger.Log("Proceeding as requested. Be careful!", Logger.LogLevel.Warn, Logger.VerbosityLevel.Minimal);
            }

            // Set Current Working Directory to game's folder (where's this assembly located at)
            Directory.SetCurrentDirectory(Constants.GamePath);

            // Initialize the mod loader
            ModLoader modLoader = new ModLoader(args);

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
                Logger.Log($"WorldMachineLoader has crashed! A crash log has been saved to {crashFile}", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                MessageBox.Show($"WorldMachineLoader has crashed!\nA crash log has been saved to {crashFile}.\n" +
                                $"Please include this file when reporting the issue.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        static bool CheckForUpdate(out string remoteVersion, out string error)
        {
            remoteVersion = null;
            error = null;
            try
            {
                using (var wc = new WebClient())
                {
                    wc.Headers.Add("User-Agent", "WML-UpdateChecker");

                    var request = WebRequest.Create("https://ref-err.ru/wml-latest");
                    request.Timeout = 3000;

                    using (var response = request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        string text = reader.ReadToEnd();

                        if (string.IsNullOrWhiteSpace(text))
                        {
                            error = "Server response is empty.";
                            return false;
                        }

                        remoteVersion = text.Trim();

                        int comp = VersionUtils.Compare(Constants.Version, remoteVersion);

                        if (comp == -1)
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
