using OneShotMG;
using System.Diagnostics;

namespace WorldMachineLoader.Utils
{
    internal class GameUtils
    {
        /// <summary>
        /// Saves everything and restarts the game.
        /// </summary>
        public static void RestartGame()
        {
            Game1.windowMan.SaveDesktopAndFileSystem();

            Process currentProcess = Process.GetCurrentProcess();
            Process.Start(currentProcess.MainModule.FileName);

            Game1.ShutDown();
        }
    }
}
