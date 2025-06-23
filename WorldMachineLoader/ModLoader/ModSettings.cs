using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WorldMachineLoader.ModLoader
{
    internal class ModSettings
    {
        [JsonProperty(PropertyName = "disabled", Required = Required.Always)]
        public List<String> Disabled { get; set; } = new List<string>();
    }
}
