using Microsoft.Xna.Framework;
using System.Collections.Generic;
using WorldMachineLoader.Modding;

namespace WorldMachineLoader
{
    /// <summary>Contains global methods/variables.</summary>
    internal class Globals
    {
        /// <summary>Contains all loaded mods.</summary>
        public static HashSet<ModItem> Mods = new HashSet<ModItem>();

        /// <summary>Contains all disabled mods.</summary>
        public static HashSet<ModItem> DisabledMods = new HashSet<ModItem>();

        // These 2 booleans are needed to ask the user to restart the game if the mod state changes.
        public static bool RestartPending = false;
        public static bool RestartWillEnable;

        /// <summary>Determines if safe mod is enabled.</summary>
        public static bool IsSafeModEnabled = false;

        /// <summary>Game instance that we get from OneShotMG by patching.</summary>
        public static Game monoGame;
    }
}
