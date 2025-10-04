using Newtonsoft.Json;
using OneShotMG.src.EngineSpecificCode;
using System.IO;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Interfaces;

namespace WorldMachineLoader.Modding
{
    /// <summary>Represents a mod object and metadata.</summary>
    public class Mod
    {
        private readonly DirectoryInfo modDir;

        public ModInfoData Info { get; }

        public IMod Instance { get; set; }

        public ModContext ModContext { get; set; }

        private Mod(string basePath)
        {
            // Get the mod directory
            modDir = new DirectoryInfo(basePath);

            // Parse mod metadata
            string modMetadataPath = Path.Combine(modDir.FullName, "mod.json");

            if (!File.Exists(modMetadataPath))
            {
                throw new FileNotFoundException($"Mod \"{modDir.Name}\" does not contain metadata file \"mod.json\".");
            }

            Info = JsonConvert.DeserializeObject<ModInfoData>(File.ReadAllText(modMetadataPath));

            if (Info == null)
                throw new JsonSerializationException($"Mod \"{modDir.Name}\" has invalid metadata in file \"mod.json\".");

            if (!string.IsNullOrEmpty(Info.AssemblyName) && !HasAssembly)
            {
                Program.Logger.Log($"Mod \"{Info.Name}\" has assembly name in metadata but does not have it.");
            }
        }

        /// <summary>Loads a mod from directory path.</summary>
        /// <param name="modPath">The mod's directory path.</param>
        /// <returns>A new <see cref="Mod"/> object.</returns>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="JsonSerializationException"/>
        internal static Mod FromPath(string modPath)
        {
            return new Mod(modPath);
        }

        public string Name => Info?.Name;
        public string ID => Info?.ID;
        public string Description => Info?.Description;
        public string Author => Info?.Author;
        public string Version => Info?.Version;
        public string URL => Info?.URL;
        public string Icon => Info?.Icon;
        public bool Experimental => Info != null && Info.Experimental;
        public string AssemblyName => Info?.AssemblyName;

        /// <summary>The mod's file path to the assembly.</summary>
        public string AssemblyFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(AssemblyName)) return string.Empty;
                return Path.Combine(modDir.FullName, AssemblyName) + ".dll";
            }
        }

        /// <summary>Whether has or not assembly file in the mod directory.</summary>
        public bool HasAssembly
        {
            get
            {
                if (string.IsNullOrEmpty(AssemblyFilePath)) return false;
                return File.Exists(AssemblyFilePath);
            }
        }
    }

    public class ModItem
    {
        public TempTexture titleTexture;

        public TempTexture descriptionTexture;

        public string title;

        public string description;

        public string author;
        
        public string name;

        public string modId;

        public string version;

        public string url;

        public string iconPath;

        public bool isEnabled;

        public bool experimental;

        public ModItem(Mod mod, string modPath, bool isEnabled)
        {
            var info = mod.Info;
            author = info?.Author ?? "";
            name = info?.Name;
            modId = info?.ID;
            version = info?.Version;
            url = info?.URL;
            iconPath = string.IsNullOrEmpty(info?.Icon) ? "" : Path.Combine(modPath, info?.Icon);
            this.isEnabled = isEnabled;
            title = $"{(isEnabled ? "" : "[DISABLED] ")}{author} - {name} ({version})";
            description = info?.Description;
            experimental = info != null && info.Experimental;
        }
    }
}
