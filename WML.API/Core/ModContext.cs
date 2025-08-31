using System.IO;
using WorldMachineLoader.API.Scheduling;
using WorldMachineLoader.API.Utils;

namespace WorldMachineLoader.API.Core
{
    /// <summary>
    /// Provides the runtime context for a mod.
    /// </summary>
    public class ModContext
    {
        /// <summary>The name of a mod.</summary>
        public string Name { get; }

        /// <summary>The ID of a mod.</summary>
        public string ModID { get; }

        /// <summary>The author of a mod.</summary>
        public string Author { get; }

        /// <summary>The version of a mod.</summary>
        public string Version { get; }

        /// <summary>The path to data directory.</summary>
        public string DataDirectory { get; }

        /// <summary>The mod's file system.</summary>
        public ModFileSystem FileSystem { get; }

        /// <summary>The mod's logger.</summary>
        public Logger Logger { get; }

        public Scheduler Scheduler { get; } = new Scheduler();

        public ConfigManager Config { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ModContext"/> class.
        /// </summary>
        /// <param name="name">The name of a mod.</param>
        /// <param name="modId">The ID of a mod.</param>
        /// <param name="author">The author of a mod.</param>
        /// <param name="version">The version of a mod.</param>
        /// <param name="dataDir">The path to mod's data directory.</param>
        public ModContext(string name, string modId, string author, string version, string dataDir)
        {
            Name = name;
            ModID = modId;
            Author = author;
            Version = version;
            DataDirectory = dataDir;
            Logger = new Logger(Name);

            if (!Directory.Exists(DataDirectory))
                Directory.CreateDirectory(DataDirectory);

            FileSystem = new ModFileSystem(DataDirectory);
            Config = new ConfigManager(FileSystem);
        }
    }
}
