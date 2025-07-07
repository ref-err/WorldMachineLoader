using System.Collections.Generic;
using WorldMachineLoader.API.Core;

namespace WorldMachineLoader.API.Interfaces
{
    public interface IModInfoProvider
    {
        IReadOnlyList<ModInfoData> GetLoadedMods();
        ModInfoData FindModByName(string name);
        ModInfoData FindModByID(string id);
    }
}
