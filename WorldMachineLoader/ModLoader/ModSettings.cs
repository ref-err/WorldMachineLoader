using Newtonsoft.Json;
using System.Collections.Generic;

namespace WorldMachineLoader.ModLoader
{
    /// <summary>Settings JSON config</summary>
    internal class ModSettings
    {
        /// <summary>Disabled mods list</summary>
        [JsonProperty(PropertyName = "disabled", Required = Required.DisallowNull)]
        public List<string> Disabled { get; set; } = new List<string>();
    }
}
