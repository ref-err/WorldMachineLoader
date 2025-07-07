using WorldMachineLoader.API.Utils;

namespace WorldMachineLoader.API.Core
{
    public class ModContext
    {
        public string Name { get; }
        public string ModID { get; }
        public string Author { get; }
        public string Version { get; }
        public string ModDirectory { get; }

        public Logger Logger { get; }

        public ModContext(string name, string modId, string author, string version, string modDir)
        {
            Name = name;
            ModID = modId;
            Author = author;
            Version = version;
            ModDirectory = modDir;
            Logger = new Logger(Name);
        }
    }
}
