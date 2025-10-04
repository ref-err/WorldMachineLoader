using Newtonsoft.Json;

namespace WorldMachineLoader.API.Core
{
    /// <summary>
    /// Provides mod information for WorldMachineLoader.
    /// </summary>
    public class ModInfoData
    {
        public ModInfoData() { }

        [JsonConstructor]
        public ModInfoData(string name, string id, string description, string author, string version, string url, string icon, bool experimental, string assemblyName)
        {
            Name = name;
            ID = id;
            Description = description;
            Author = author;
            Version = version;
            URL = url;
            Icon = icon;
            Experimental = experimental;
            AssemblyName = assemblyName;
        }

        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; }


        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public string ID { get; set; }


        [JsonProperty(PropertyName = "description", Required = Required.DisallowNull)]
        public string Description { get; set; }


        [JsonProperty(PropertyName = "author", Required = Required.Always)]
        public string Author { get; set; }


        [JsonProperty(PropertyName = "version", Required = Required.Always)]
        public string Version { get; set; }


        [JsonProperty(PropertyName = "url", Required = Required.Default)]
        public string URL { get; set; }


        [JsonProperty(PropertyName = "icon", Required = Required.DisallowNull)]
        public string Icon { get; set; }


        [JsonProperty(PropertyName = "experimental", Required = Required.DisallowNull)]
        public bool Experimental { get; set; }


        [JsonProperty(PropertyName = "assembly_name", Required = Required.Always)]
        public string AssemblyName { get; set; }
    }
}
