using WorldMachineLoader.API.Core;

namespace WorldMachineLoader.API.Interfaces
{
    /// <summary>The main interface that every mod should implement.</summary>
    public interface IMod
    {
        /// <summary>Called when the mod is being loaded.</summary>
        /// <param name="modContext">The context in which the mod is loaded</param>
        void OnLoad(ModContext modContext);

        /// <summary>Called when the game is shutting down.</summary>
        void OnShutdown();
    }
}
