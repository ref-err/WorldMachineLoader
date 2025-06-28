using Newtonsoft.Json;

namespace WorldMachineLoader.Modding
{
    /// <summary>The metadata for a mod from JSON file.</summary>
    internal class ModMetadata
    {
        /// <summary>The mod's display name.</summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>The mod's description text.</summary>
        [JsonProperty(PropertyName = "description", Required = Required.DisallowNull)]
        public string Description { get; set; }

        /// <summary>The mod's author display name.</summary>
        [JsonProperty(PropertyName = "author", Required = Required.Always)]
        public string Author { get; set; }

        /// <summary>The mod's version string.</summary>
        [JsonProperty(PropertyName = "version", Required = Required.Always)]
        public string Version { get; set; }

        /// <summary>The mod's home URL address string.</summary>
        [JsonProperty(PropertyName = "url", Required = Required.Default)]
        public string URL { get; set; }

        /// <summary>The mod's icon</summary>
        [JsonProperty(PropertyName = "icon", Required = Required.DisallowNull)]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "experimental", Required = Required.DisallowNull)]
        public bool Experimental { get; set; }

        /// <summary>The mod's assembly filename to load.</summary>
        [JsonProperty(PropertyName = "assembly_name", Required = Required.DisallowNull)]
        public string AssemblyName { get; set; }
    }
}
