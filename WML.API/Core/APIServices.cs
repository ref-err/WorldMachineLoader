using WorldMachineLoader.API.Interfaces;

namespace WorldMachineLoader.API.Core
{
    /// <summary>
    /// Provides access to the WorldMachineLoader API core services.
    /// </summary>
    public class APIServices
    {
        /// <summary>
        /// Gets or sets the mod information provider.
        /// Used to provide mod information via API.
        /// </summary>
        public static IModInfoProvider ModInfoProvider { get; internal set; }
    }
}
