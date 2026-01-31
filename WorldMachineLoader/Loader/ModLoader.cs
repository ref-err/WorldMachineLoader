using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Interfaces;
using WorldMachineLoader.API.Utils;
using WorldMachineLoader.Modding;
using WorldMachineLoader.Utils;

namespace WorldMachineLoader.Loader
{
    /// <summary>The core mod loader class.</summary>
    internal class ModLoader
    {
        public Logger Logger = new Logger("WML/ModLoader");

        private readonly Harmony harmony = new Harmony("io.github.referr.oswmeloader");

        private Assembly gameAssembly;

        private readonly DirectoryInfo modsDirectory = new DirectoryInfo(Constants.ModsPath);

        public static readonly HashSet<Mod> loadedMods = new HashSet<Mod>();

        /// <summary>Creates mod loader instance.</summary>
        /// <param name="args">The list of provided command line arguments.</param>
        public ModLoader(string[] args)
        {
            APIServices.ModInfoProvider = new ModInfoProvider();
        }

        /// <summary>Check that the game assembly is available.</summary>
        /// <returns>Returns a boolean value whether the game assembly was successfully checked and loaded.</returns>
        public bool CheckGameAssembly()
        {
            try
            {
                gameAssembly = Assembly.LoadFrom($"{Constants.GameAssemblyName}.exe");

                return true;
            }
            catch (BadImageFormatException ex)
            {
                Logger.Log($"Could not load \"{ex.FileName}.exe\"!", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                Logger.Log($"Bad Image Format Exception: {ex.Message}", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);

                if (!Environment.Is64BitProcess)
                    Logger.Log("It seems we are running in 32-bit mode. Consider to use 64-bit instead.", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
            }
            catch (Exception ex)
            {
                if (!File.Exists(Path.Combine(Constants.GamePath, $"{Constants.GameAssemblyName}.exe")))
                {
                    var msg = "Could not find the game executable file. Please check if it's running inside game folder.";
                    Logger.Log(msg, Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Logger.Log($"Exception while trying to get game assembly:\n{ex}", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                }
            }

            return false;
        }

        /// <summary>Checks all mods in the directory to parse them for further loading it.</summary>
        public void CheckMods()
        {
            if (Globals.isSafeModEnabled)
            {
                Logger.Log("Safe mod is enabled, not loading any mods.", Logger.LogLevel.Warn, Logger.VerbosityLevel.Minimal);
                return;
            }

            // List all mods subdirectories
            string[] modsSubdirs = Directory.GetDirectories(modsDirectory.FullName);

            foreach (string modDir in modsSubdirs)
            {
                LoadModFromPath(modDir);
            }
        }

        /// <summary>Loads the mod from specified directory path.</summary>
        /// <param name="modPath">The mod's directory path.</param>
        /// <returns>Returns a boolean value whether the mod was successfully loaded.</returns>
        private bool LoadModFromPath(string modPath)
        {
            string modDirName = Path.GetFileName(modPath);

            if (!File.Exists(Path.Combine(modPath, "mod.json")))
            {
                Logger.Log($"Skipping mod \"{modDirName}\" as it does not have mod.json file.", Logger.LogLevel.Warn);
                return false;
            }

            try
            {
                Mod mod = Mod.FromPath(modPath);
                string iconName = mod.Icon;

                if (string.IsNullOrEmpty(iconName))
                {
                    iconName = "";
                }

                if (!ModSettings.IsEnabled(mod.ID))
                {
                    Logger.Log($"Skipping mod \"{mod.Name}/{mod.ID}\" because it is disabled in config file.");
                    Globals.disabledMods.Add(new ModItem(mod, modPath, false));
                    return false;
                }

                if (loadedMods.Any(m => mod.ID == m.ID))
                {
                    Logger.Log($"Duplicate mod ID \"{mod.ID}\" detected for mod \"{mod.Name}\". Skipping load.", Logger.LogLevel.Warn, Logger.VerbosityLevel.Minimal);
                    return false;
                }

                if (!mod.HasAssembly) return false;

                var assembly = Assembly.LoadFrom(mod.AssemblyFilePath);

                Logger.Log($"[{mod.ID}] Loading assembly: {mod.AssemblyFilePath}", Logger.LogLevel.Info, Logger.VerbosityLevel.Detailed);
                Logger.Log($"[{mod.ID}] Assembly.FullName: {assembly.FullName}", Logger.LogLevel.Info, Logger.VerbosityLevel.Detailed);

                mod.Assembly = assembly;

                if (loadedMods.Any(m => m.Assembly.FullName == assembly.FullName))
                {
                    Logger.Log($"Mod \"{mod.Name}/{mod.ID}\" has duplicate Assembly.FullName: {assembly.FullName}. Skipping...", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                    Logger.Log("If you are a developer, please avoid using same Assembly.FullName's. If you aren't, report this error to mod's developer.", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                    return false;
                }

                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IMod).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        Logger.Log($"Loading mod \"{mod.Name}/{mod.ID}\"...");

                        ModContext context = new ModContext(
                            mod.Name,
                            mod.ID,
                            mod.Author,
                            mod.Version,
                            Path.Combine(modPath, "data")
                        );

                        var modInstance = (IMod)Activator.CreateInstance(type);

                        try
                        {
                            modInstance.OnLoad(context);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($"Exception while calling \"{mod.Name}/{mod.ID}\" OnLoad: {ex}", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                            Logger.Log($"Not loading.", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                            return false;
                        }

                        Globals.mods.Add(new ModItem(mod, modPath, true));
                        mod.Instance = modInstance;
                        mod.ModContext = context;
                        loadedMods.Add(mod);

                        if (mod.Experimental)
                        {
                            Logger.Log($"\"{mod.Name}/{mod.ID}\" is marked as experimental. Be careful!", Logger.LogLevel.Warn);
                        }

                        return true;
                    }
                }
                Logger.Log($"Couldn't load mod \"{mod.Name}/{mod.ID}\": no class implementing IMod was found in the assembly.", Logger.LogLevel.Warn, Logger.VerbosityLevel.Minimal);
                return false;
            }
            catch (JsonSerializationException ex)
            {
                Logger.Log($"Couldn't parse mod \"{modDirName}\" metadata!", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                Logger.Log($"Exception: {ex.Message}", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
            }
            catch (Exception ex)
            {
                Logger.Log($"Couldn't load mod \"{modDirName}\"!", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                Logger.Log($"Exception: {ex}", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
            }

            return false;
        }

        /// <summary>Loads game assembly, patches and launches the game.</summary>
        public void Start()
        {
            Logger.Log("Patching...");

            // Patch any Harmony annotations from this assembly before mods assemblies
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            PatchManager patchManager = new PatchManager();

            // Load mod's assemblies to patch the game
            foreach (Mod mod in loadedMods)
            {
                patchManager.ApplyAllPatches(mod.Assembly, mod.ID);
            }

            // Invoke OneShotMG entry point to run the game
            Logger.Log("Starting OneShotMG...", Logger.LogLevel.Info, Logger.VerbosityLevel.Minimal);
            MethodBase gameEntrypoint = gameAssembly.ManifestModule.ResolveMethod(gameAssembly.EntryPoint.MetadataToken);
            gameEntrypoint.Invoke(null, null);
        }
    }
}
