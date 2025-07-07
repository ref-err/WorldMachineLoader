namespace WorldMachineLoader.API.Core
{
    public class ModInfoData
    {
        public string Name { get; }
        public string ID { get; }
        public string Version { get; }
        public string Author { get; }
        
        public ModInfoData(string name, string id, string author, string version)
        {
            Name = name;
            ID = id;
            Author = author;
            Version = version;
        }
    }
}
