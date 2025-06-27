using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;
using HarmonyLib;
using WorldMachineLoader.Modding;

namespace WorldMachineLoader.ModLoader
{
    /// <summary>The core mod loader class.</summary>
    internal class ModLoader
    {
        private readonly Harmony harmony;

        private Assembly gameAssembly;

        private readonly DirectoryInfo modsDirectory;

        private readonly List<Mod> mods = new List<Mod>();

        /// <summary>Creates mod loader instance.</summary>
        /// <param name="args">The list of provided command line arguments.</param>
        public ModLoader(string[] args)
        {
            // Create Harmony instance
            harmony = new Harmony("io.github.referr.oswmeloader");
            
            // Get the mods directory
            modsDirectory = new DirectoryInfo(Constants.ModsPath);
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
                Console.WriteLine($"[WML ERROR] Could not load \"{ex.FileName}.exe\"!");
                Console.WriteLine($"[WML ERROR] Bad Image Format Exception: {ex.Message}");

                if (!Environment.Is64BitProcess)
                    Console.WriteLine("[WML ERROR] It seems we are running in 32-bit mode. Consider to use 64-bit instead.");
            }
            catch (Exception ex)
            {
                if (!File.Exists(Path.Combine(Constants.GamePath, $"{Constants.GameAssemblyName}.exe")))
                {
                    Console.WriteLine("[WML ERROR] Could not find the game executable file. Please check if it's running inside game folder.");
                }
                else
                {
                    Console.WriteLine($"[WML ERROR] Exception while trying to get game assembly:\n{ex}");
                }
            }

            return false;
        }

        /// <summary>Checks all mods in the directory to parse them for further loading it.</summary>
        public void CheckMods()
        {
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
                Console.WriteLine($"[WML] Skipping mod \"{modDirName}\" as it does not have mod.json file.");
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

                if (!ModSettings.IsEnabled(mod.Name))
                {
                    Console.WriteLine($"[WML] Skipping mod \"{mod.Name}\" because it is disabled in config file.");
                    Globals.disabledMods.Add(new ModItem(mod.Author, mod.Name, mod.Version, mod.Description, mod.URL, Path.Combine(modPath, iconName), false));
                    return false;
                }

                Console.WriteLine($"[WML] Loading mod \"{mod.Name}\"...");

                mods.Add(mod);
                Globals.mods.Add(new ModItem(mod.Author, mod.Name, mod.Version, mod.Description, mod.URL, Path.Combine(modPath, iconName), true));

                return true;
            }
            catch (JsonSerializationException ex)
            {
                Console.Error.WriteLine($"[WML ERROR] Couldn't parse mod \"{modDirName}\" metadata!");
                Console.Error.WriteLine($"[WML ERROR] Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[WML ERROR] Couldn't load mod \"{modDirName}\"!");
                Console.Error.WriteLine($"[WML ERROR] Exception: {ex}");
            }

            return false;
        }

        /// <summary>Loads game assembly, patches and launches the game.</summary>
        public void Start()
        {
            Console.WriteLine("[WML] Patching...");

            // Patch any Harmony annotations from this assembly before mods assemblies
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            // Load mod's assemblies to patch the game
            foreach (Mod mod in mods)
            {
                if (mod.HasAssembly)
                {
                    Assembly modAssembly = Assembly.LoadFrom(mod.AssemblyFilePath);
                    harmony.PatchAll(modAssembly);
                }
            }

            // Invoke OneShotMG entry point to run the game
            Console.WriteLine("[WML] Starting OneShotMG...");
            MethodBase gameEntrypoint = gameAssembly.ManifestModule.ResolveMethod(gameAssembly.EntryPoint.MetadataToken);
            gameEntrypoint.Invoke(null, null);
        }
    }
}
