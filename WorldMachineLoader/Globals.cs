using Microsoft.Xna.Framework;
using System.Collections.Generic;
using WorldMachineLoader.Modding;

namespace WorldMachineLoader
{
    /// <summary>Contains global methods/variables</summary>
    internal class Globals
    {
        /// <summary>Contains all loaded mods</summary>
        public static List<ModItem> mods = new List<ModItem>();

        public static List<ModItem> disabledMods = new List<ModItem>();

        public static bool restartPending = false;
        public static bool restartWillEnable;

        public static Game monoGame;
    }
}
