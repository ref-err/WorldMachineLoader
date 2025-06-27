using Newtonsoft.Json;
using OneShotMG.src.EngineSpecificCode;
using System;
using System.IO;

namespace WorldMachineLoader.Modding
{
    public class ModItem
    {
        public TempTexture titleTexture;

        public TempTexture descriptionTexture;

        public string title;

        public string description;

        public string author;

        public string name;

        public string version;

        public string url;

        public string iconPath;

        public bool isEnabled;

        public ModItem(string author, string name, string version, string description, string url, string iconPath, bool isEnabled)
        {
            this.author = author;
            this.name = name;
            this.version = version;
            this.url = url;
            this.iconPath = iconPath;
            this.isEnabled = isEnabled;
            this.title = $"{(isEnabled ? "" : "[DISABLED] ")}{this.author} - {this.name} ({this.version})";
            this.description = description;
        }
    }

    /// <summary>Represents a mod object and metadata.</summary>
    internal class Mod
    {
        private readonly DirectoryInfo modDir;

        private readonly ModMetadata modMetadata;

        /// <summary>Loads a mod from path.</summary>
        /// <param name="basePath">The mod's directory path.</param>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="JsonSerializationException"/>
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

            modMetadata = JsonConvert.DeserializeObject<ModMetadata>(File.ReadAllText(modMetadataPath));

            if (!string.IsNullOrEmpty(modMetadata.AssemblyName) && !HasAssembly)
            {
                Console.WriteLine($"[WML] Mod \"{Name}\" has assembly name in metadata but does not have it.");
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

        /// <summary>The mod's display name.</summary>
        public string Name { get => modMetadata.Name; }

        /// <summary>The mod's description text.</summary>
        public string Description { get => modMetadata.Description; }

        /// <summary>The mod's author display name.</summary>
        public string Author { get => modMetadata.Author; }

        /// <summary>The mod's version string.</summary>
        public string Version { get => modMetadata.Version; }

        /// <summary>The mod's home URL address string.</summary>
        public string URL { get => modMetadata.URL; }

        /// <summary>The mod's icon</summary>
        public string Icon { get => modMetadata.Icon; }

        /// <summary>The mod's assembly filename to load.</summary>
        public string AssemblyName { get => modMetadata.AssemblyName; }

        /// <summary>The mod's directory path.</summary>
        public string DirectoryPath { get => modDir.FullName; }

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
}
