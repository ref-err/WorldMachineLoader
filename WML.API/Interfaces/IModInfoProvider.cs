using System.Collections.Generic;
using WorldMachineLoader.API.Core;

namespace WorldMachineLoader.API.Interfaces
{
    public interface IModInfoProvider
    {
        IReadOnlyList<ModInfoData> GetLoadedMods();
        ModInfoData FindModByID(string id);
        List<ModInfoData> FindModsByName(string name);
    }
}
