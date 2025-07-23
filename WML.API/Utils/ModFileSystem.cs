using System;
using System.IO;

namespace WorldMachineLoader.API.Utils
{
    /// <summary>
    /// Environment in which mod is intended to make file operations.
    /// </summary>
    public class ModFileSystem
    {
        private readonly string rootPath;

        /// <summary>
        /// Initializes a new instance of <see cref="ModFileSystem"/> class.
        /// </summary>
        /// <param name="rootPath">The root path of the FS.</param>
        public ModFileSystem(string rootPath)
        {
            this.rootPath = rootPath;
        }

        /// <summary>
        /// Returns the absolute path for the specified path string.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns>The full path to the file.</returns>
        /// <exception cref="UnauthorizedAccessException">When mod tries to make operations outside it's environment.</exception>
        public string GetFullPath(string relativePath)
        {
            string combined = Path.GetFullPath(Path.Combine(rootPath, relativePath));
            if (!combined.StartsWith(rootPath))
                throw new UnauthorizedAccessException("Access outside mod's data directory is not allowed.");
            return combined;
        }

        /// <summary>Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
        public void WriteAllText(string path, string content) { File.WriteAllText(GetFullPath(path), content); }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <returns>A string containing all lines of the file.</returns>
        public string ReadAllText(string path) { return File.ReadAllText(GetFullPath(path)); }

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <returns>true if the caller has the required permission and path contains the name of an existing file; otherwise, false.</returns>
        public bool Exists(string path) { return File.Exists(GetFullPath(path)); }

        /// <summary>Deletes the specified file.</summary>
        public void Delete(string path) { File.Delete(GetFullPath(path)); }
    }
}
