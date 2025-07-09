using System;
using System.IO;

namespace WorldMachineLoader.API.Utils
{
    public class ModFileSystem
    {
        private readonly string rootPath;

        public ModFileSystem(string rootPath)
        {
            this.rootPath = rootPath;
        }

        public string GetFullPath(string relativePath)
        {
            string combined = Path.GetFullPath(Path.Combine(rootPath, relativePath));
            if (!combined.StartsWith(rootPath))
                throw new UnauthorizedAccessException("Access outside mod's data directory is not allowed.");
            return combined;
        }

        public void WriteAllText(string path, string content) { File.WriteAllText(GetFullPath(path), content); }

        public string ReadAllText(string path) { return File.ReadAllText(GetFullPath(path)); }

        public bool Exists(string path) { return File.Exists(GetFullPath(path)); }

        public void Delete(string path) { File.Delete(GetFullPath(path)); }
    }
}
