using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using WorldMachineLoader.API.Utils;

namespace WorldMachineLoader.Loader
{
    /// <summary>
    /// Manages persistent settings for the mod loader.
    /// Uses JSON file to load and save settings on disk.
    /// </summary>
    internal class Settings
    {
        /// <summary>Gets or sets the currently disabled mod IDs.</summary>
        [JsonProperty(PropertyName = "disabled", Required = Required.DisallowNull)]
        public List<string> Disabled { get; set; } = new List<string>();

        /// <summary>Gets or sets value indicating whether intro sequence should be skipped.</summary>
        [JsonProperty(PropertyName = "skip_intro", Required = Required.DisallowNull)]
        public bool SkipIntro { get; set; } = false;

        /// <summary>Gets or sets value indicating whether loader should ignore if program has run as administrator.</summary>
        [JsonProperty(PropertyName = "ignore_admin_check", Required = Required.DisallowNull)]
        public bool IgnoreAdminCheck { get; set; } = false;

        [JsonProperty(PropertyName = "verbosity", Required = Required.DisallowNull)]
        public Logger.VerbosityLevel VerbosityLevel { get; set; } = Logger.VerbosityLevel.Standard;

        [JsonProperty(PropertyName = "disable_update_check", Required = Required.DisallowNull)]
        public bool DisableUpdateCheck { get; set; } = false;

        private static Settings _instance;

        /// <summary>
        /// Gets the instance of <see cref="ModSettings"/>.
        /// If there's no settings file or instance has not yet loaded,
        /// automatically calls <see cref="Load"/>
        /// </summary>
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                    Load();
                return _instance;
            }
        }

        /// <summary>
        /// Loads the settings from the JSON file specified by <see cref="Constants.SettingsPath"/>.
        /// If the file does not exist, it creates a new <see cref="ModSettings"/> instance
        /// with default values and saves immediately to disk.
        /// </summary>
        public static void Load()
        {
            if (!File.Exists(Constants.SettingsPath))
            {
                _instance = new Settings();
                Save();
                return;
            }

            string json = File.ReadAllText(Constants.SettingsPath);
            _instance = JsonConvert.DeserializeObject<Settings>(json) ?? new Settings();
        }

        /// <summary>
        /// Saves the current settings instance into the JSON file.
        /// If no instance is loaded, this method does nothing.
        /// </summary>
        public static void Save()
        {
            if (_instance == null) return;

            string json = JsonConvert.SerializeObject(_instance, Formatting.Indented);
            File.WriteAllText(Constants.SettingsPath, json);
        }

        /// <summary>Disables the specified mod by name.</summary>
        /// <param name="modId">The name of the mod to disable.</param>
        public static void DisableMod(string modId)
        {
            if (!Instance.Disabled.Contains(modId))
            {
                Instance.Disabled.Add(modId);
                Save();
            }
        }

        /// <summary>Enables the specified mod by name.</summary>
        /// <param name="modId">The name of the mod to enable.</param>
        public static void EnableMod(string modId)
        {
            if (Instance.Disabled.Remove(modId))
            {
                Save();
            }
        }

        /// <summary>Determines whether the specified mod is enabled.</summary>
        /// <param name="modId">The name of the mod to check.</param>
        /// <returns><c>true</c> if the mod is enabled; otherwise <c>false</c>.</returns>
        public static bool IsEnabled(string modId)
        {
            return !Instance.Disabled.Contains(modId);
        }
    }
}
