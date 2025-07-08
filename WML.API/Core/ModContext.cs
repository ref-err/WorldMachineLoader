using System.IO;
using WorldMachineLoader.API.Utils;

namespace WorldMachineLoader.API.Core
{
    public class ModContext
    {
        public string Name { get; }
        public string ModID { get; }
        public string Author { get; }
        public string Version { get; }
        public string DataDirectory { get; }

        public Logger Logger { get; }

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
        }
    }
}
