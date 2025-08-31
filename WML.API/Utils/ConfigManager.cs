using Newtonsoft.Json;

namespace WorldMachineLoader.API.Utils
{
    /// <summary>
    /// Provides a simple way for mods to load and save configuration objects as JSON.
    /// All files are stored inside the mod's data folder using the <see cref="ModFileSystem"/>.
    /// </summary>
    public class ConfigManager
    {
        private readonly ModFileSystem _fs;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigManager"/> class
        /// using the specified <see cref="ModFileSystem"/>.
        /// </summary>
        /// <param name="fs">The file system of the mod, used to read and write configuration files.</param>
        public ConfigManager(ModFileSystem fs)
        {
            _fs = fs;
        }

        /// <summary>
        /// Loads a configuration object of type <typeparamref name="T"/> from a JSON file.
        /// If the file does not exist, a new default instance of <typeparamref name="T"/> is created,
        /// saved to the file system, and returned.
        /// </summary>
        /// <typeparam name="T">The type of the configuration object.</typeparam>
        /// <param name="filename">The name of the configuration file (including .json extension or without).</param>
        /// <returns>The deserialized configuration object, or a new instance if the file does not exist or is invalid.</returns>
        public T Load<T>(string filename) where T : new()
        {
            if (!_fs.Exists(filename))
            {
                var defaultConfig = new T();
                Save(filename, defaultConfig);
                return defaultConfig;
            }

            string json = _fs.ReadAllText(filename);
            var result = JsonConvert.DeserializeObject<T>(json);
            if (result == null)
            {
                result = new T();
            }

            return result;
        }

        /// <summary>
        /// Saves the specified configuration object as a JSON file using indentation for readability.
        /// </summary>
        /// <typeparam name="T">The type of the configuration object.</typeparam>
        /// <param name="filename">The name of the configuration file (including .json extension or without).</param>
        /// <param name="config">The configuration object to save.</param>
        public void Save<T>(string filename, T config)
        {
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            _fs.WriteAllText(filename, json);
        }
    }
}
