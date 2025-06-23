using System.IO;
using System.Reflection;

namespace WorldMachineLoader
{
    /// <summary>Contains constants for defining game details such as game path and assembly name.</summary>
    internal static class Constants
    {
        /// <summary>The path to the game folder.</summary>
        public static string GamePath { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>The game's assembly name.</summary>
        internal static string GameAssemblyName { get; } = "OneShotMG";

        /// <summary>The path to the all mods folder.</summary>
        public static string ModsPath { get; } = Path.Combine(GamePath, "mods");

        /// <summary>The path to loader's config file</summary>
        public static string SettingsPath { get; } = Path.Combine(ModsPath, "settings.json");
    }
}
