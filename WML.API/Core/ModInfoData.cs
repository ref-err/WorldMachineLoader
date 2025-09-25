namespace WorldMachineLoader.API.Core
{
    /// <summary>
    /// Provides mod information for WorldMachineLoader.
    /// </summary>
    public class ModInfoData
    {
        /// <summary>
        /// Gets the name of the mod.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the unique identifier of the mod.
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// Gets the version of the mod.
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the name of the mod author.
        /// </summary>
        public string Author { get; }

        /// <summary>
        /// Initializes a new instance of class <see cref="ModInfoData"/>.
        /// </summary>
        /// <param name="name">Mod name.</param>
        /// <param name="id">Unique mod identifier.</param>
        /// <param name="author">Mod author's name.</param>
        /// <param name="version">Mod version.</param>
        public ModInfoData(string name, string id, string author, string version)
        {
            Name = name;
            ID = id;
            Author = author;
            Version = version;
        }
    }
}
