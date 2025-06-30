using Microsoft.Xna.Framework;
using System.Collections.Generic;
using WorldMachineLoader.Modding;

namespace WorldMachineLoader
{
    /// <summary>Contains global methods/variables.</summary>
    internal class Globals
    {
        /// <summary>Contains all loaded mods.</summary>
        public static List<ModItem> mods = new List<ModItem>();

        /// <summary>Contains all disabled mods.</summary>
        public static List<ModItem> disabledMods = new List<ModItem>();

        // These 2 booleans are needed to ask the user to restart the game if the mod state changes.
        public static bool restartPending = false;
        public static bool restartWillEnable;

        /// <summary>Determines if safe mod is enabled.</summary>
        public static bool isSafeModEnabled = false;

        /// <summary>Game instance that we get from OneShotMG by patching.</summary>
        public static Game monoGame;
    }
}
