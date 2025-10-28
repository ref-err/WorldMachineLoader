using System.Collections.Generic;
using System.Linq;
using WorldMachineLoader.Loader;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Interfaces;

namespace WorldMachineLoader.Modding
{
    public class ModInfoProvider : IModInfoProvider
    {
        public IReadOnlyList<ModInfoData> GetLoadedMods()
        {
            return ModLoader.mods.Select(m =>
            {
                return new ModInfoData(m.Name, m.ID, m.Description, m.Author, m.Version, m.URL, m.Icon, m.Experimental, m.AssemblyName);
            }).ToList();
        }

        public ModInfoData FindModByID(string id)
        {
            var mod = ModLoader.mods.FirstOrDefault(m => m.ID == id);
            if (mod == null) return null;

            return new ModInfoData(mod.Name, mod.ID, mod.Description, mod.Author, mod.Version, mod.URL, mod.Icon, mod.Experimental, mod.AssemblyName);
        }

        public List<ModInfoData> FindModsByName(string name)
        {
            var mods = ModLoader.mods.Where(m => m.Name == name).ToList();
            if (mods.Any()) return null;

            return mods.Select(mod => new ModInfoData(
                mod.Name,
                mod.ID,
                mod.Description, 
                mod.Author,
                mod.Version,
                mod.URL,
                mod.Icon,
                mod.Experimental,
                mod.AssemblyName)).ToList();
        }
    }
}
