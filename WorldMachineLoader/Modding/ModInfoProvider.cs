using System.Collections.Generic;
using System.Linq;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Interfaces;

namespace WorldMachineLoader.Modding
{
    public class ModInfoProvider : IModInfoProvider
    {
        public IReadOnlyList<ModInfoData> GetLoadedMods()
        {
            return ModLoader.ModLoader.mods.Select(m =>
            {
                return new ModInfoData(m.Name, m.ID, m.Author, m.Version);
            }).ToList();
        }

        public ModInfoData FindModByID(string id)
        {
            var mod = ModLoader.ModLoader.mods.FirstOrDefault(m => m.ID == id);
            if (mod == null) return null;

            return new ModInfoData(mod.Name, mod.ID, mod.Author, mod.Version);
        }

        public ModInfoData FindModByName(string name)
        {
            var mod = ModLoader.ModLoader.mods.FirstOrDefault(m => m.Name == name);
            if (mod == null) return null;

            return new ModInfoData(mod.Name, mod.ID, mod.Author, mod.Version);
        }
    }
}
