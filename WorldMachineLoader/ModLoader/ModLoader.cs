using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using HarmonyLib;
using WorldMachineLoader.Modding;
using WorldMachineLoader.API.Interfaces;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.Utils;
using System.Windows.Forms;

namespace WorldMachineLoader.ModLoader
{
    /// <summary>The core mod loader class.</summary>
    internal class ModLoader
    {
        private readonly Harmony harmony;

        private Assembly gameAssembly;

        private readonly DirectoryInfo modsDirectory;

        public static readonly List<Mod> mods = new List<Mod>();

        private HashSet<string> loadedAssemblies = new HashSet<string>();

        /// <summary>Creates mod loader instance.</summary>
        /// <param name="args">The list of provided command line arguments.</param>
        public ModLoader(string[] args)
        {
            // Create Harmony instance
            harmony = new Harmony("io.github.referr.oswmeloader");
            
            // Get the mods directory
            modsDirectory = new DirectoryInfo(Constants.ModsPath);

            APIServices.ModInfoProvider = new ModInfoProvider();
        }

        /// <summary>Check that the game assembly is available.</summary>
        /// <returns>Returns a boolean value whether the game assembly was successfully checked and loaded.</returns>
        public bool CheckGameAssembly()
        {
            try
            {
                _ = Type.GetType($"OneShotMG.Game1, {Constants.GameAssemblyName}", true);

                gameAssembly = Assembly.LoadFrom($"{Constants.GameAssemblyName}.exe");

                return true;
            }
            catch (BadImageFormatException ex)
            {
                Logger.Log($"Could not load \"{ex.FileName}.exe\"!", Logger.LogLevel.Error);
                Logger.Log($"Bad Image Format Exception: {ex.Message}", Logger.LogLevel.Error);

                if (!Environment.Is64BitProcess)
                    Logger.Log("It seems we are running in 32-bit mode. Consider to use 64-bit instead.", Logger.LogLevel.Error);
            }
            catch (Exception ex)
            {
                if (!File.Exists(Path.Combine(Constants.GamePath, $"{Constants.GameAssemblyName}.exe")))
                {
                    var msg = "Could not find the game executable file. Please check if it's running inside game folder.";
                    Logger.Log(msg, Logger.LogLevel.Error);
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Logger.Log($"Exception while trying to get game assembly:\n{ex}", Logger.LogLevel.Error);
                }
            }

            return false;
        }

        /// <summary>Checks all mods in the directory to parse them for further loading it.</summary>
        public void CheckMods()
        {
            if (Globals.isSafeModEnabled)
            {
                Logger.Log("Safe mod is enabled, not loading any mods.", Logger.LogLevel.Warn);
                return;
            }
            // Create the mods directory
            if (!modsDirectory.Exists)
                modsDirectory.Create();

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
                    Logger.Log($"Skipping mod \"{mod.Name}\" because it is disabled in config file.");
                    Globals.disabledMods.Add(new ModItem(mod, modPath, false));
                    return false;
                }

                if (!mod.HasAssembly)
                {
                    return false;
                }
                var assembly = Assembly.LoadFrom(mod.AssemblyFilePath);

                if (!loadedAssemblies.Add(assembly.FullName))
                {
                    Logger.Log($"Mod \"{mod.Name}\" has duplicate Assembly.FullName: {assembly.FullName}. Skipping...", Logger.LogLevel.Error);
                    Logger.Log("If you are a developer, please avoid using same Assembly.FullName's. If you aren't, report this error to mod's developer.", Logger.LogLevel.Error);
                    return false;
                }

                Logger.Log($"[{mod.ID}] Loading assembly: {mod.AssemblyFilePath}");
                Logger.Log($"[{mod.ID}] Assembly.FullName: {assembly.FullName}");

                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IMod).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        Logger.Log($"Loading mod \"{mod.Name}\"...");

                        var dataDir = Path.Combine(modPath, "data");

                        ModContext context = new ModContext(
                            mod.Name,
                            mod.ID,
                            mod.Author,
                            mod.Version,
                            dataDir
                        );

                        var modInstance = (IMod)Activator.CreateInstance(type);
                        try
                        {
                            modInstance.OnLoad(context);
                        } catch (Exception ex)
                        {
                            Logger.Log($"Exception while calling {mod.ID} OnLoad: {ex}", Logger.LogLevel.Error);
                            return false;
                        }

                        mods.Add(mod);
                        Globals.mods.Add(new ModItem(mod, modPath, true));
                        mod.Instance = modInstance;

                        if (mod.Experimental)
                        {
                            Logger.Log($"[WML WARN] \"{mod.Name}\" is marked as experimental. Be careful!", Logger.LogLevel.Warn);
                        }
                        return true;
                    }
                    else
                    {
                        Logger.Log($"[WML WARN] Couldn't load mod \"{mod.Name}\"", Logger.LogLevel.Warn);
                        Logger.Log($"[WML WARN] Are you sure that this mod is supported by this version of loader?", Logger.LogLevel.Warn);
                        return false;
                    }
                }
            }
            catch (JsonSerializationException ex)
            {
                Logger.Log($"[WML ERROR] Couldn't parse mod \"{modDirName}\" metadata!", Logger.LogLevel.Error);
                Logger.Log($"[WML ERROR] Exception: {ex.Message}", Logger.LogLevel.Error);
            }
            catch (Exception ex)
            {
                Logger.Log($"[WML ERROR] Couldn't load mod \"{modDirName}\"!", Logger.LogLevel.Error);
                Logger.Log($"[WML ERROR] Exception: {ex}", Logger.LogLevel.Error);
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
            foreach (Mod mod in mods)
            {
                var asm = Assembly.LoadFrom(mod.AssemblyFilePath);
                patchManager.ApplyAllPatches(asm, mod.ID);
            }

            // Invoke OneShotMG entry point to run the game
            Logger.Log("Starting OneShotMG...");
            MethodBase gameEntrypoint = gameAssembly.ManifestModule.ResolveMethod(gameAssembly.EntryPoint.MetadataToken);
            gameEntrypoint.Invoke(null, null);
        }
    }
}
