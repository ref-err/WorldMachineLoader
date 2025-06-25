using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace WorldMachineLoader.ModLoader
{
    /// <summary>Settings JSON config</summary>
    internal class ModSettings
    {
        /// <summary>Disabled mods list</summary>
        [JsonProperty(PropertyName = "disabled", Required = Required.DisallowNull)]
        public List<string> Disabled { get; set; } = new List<string>();


        private static ModSettings _instance;

        public static ModSettings Instance
        {
            get
            {
                if (_instance == null)
                    Load();
                return _instance;
            }
        }

        public static void Load()
        {
            if (!File.Exists(Constants.SettingsPath))
            {
                _instance = new ModSettings();
                Save();
                return;
            }

            string json = File.ReadAllText(Constants.SettingsPath);
            _instance = JsonConvert.DeserializeObject<ModSettings>(json) ?? new ModSettings();
        }

        public static void Save()
        {
            if (_instance == null) return;

            string json = JsonConvert.SerializeObject(_instance, Formatting.Indented);
            File.WriteAllText(Constants.SettingsPath, json);
        }

        public static void DisableMod(string modName)
        {
            if (!Instance.Disabled.Contains(modName))
            {
                Instance.Disabled.Add(modName);
                Save();
            }
        }

        public static void EnableMod(string modName)
        {
            if (Instance.Disabled.Remove(modName))
            {
                Save();
            }
        }

        public static bool IsEnabled(string modName)
        {
            return !Instance.Disabled.Contains(modName);
        }
    }
}
