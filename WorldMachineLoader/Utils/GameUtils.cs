using OneShotMG;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WorldMachineLoader.Utils
{
    /// <summary>
    /// This class contains utils that are used in modloader.
    /// </summary>
    internal class GameUtils
    {
        /// <summary>
        /// Saves everything and restarts the game.
        /// </summary>
        public static async Task RestartGameAsync()
        {
            Game1.windowMan.ShowModalWindow(OneShotMG.src.TWM.ModalWindow.ModalType.Info, "Waiting for save file to be written...", null, false);
            Game1.windowMan.SaveDesktopAndFileSystem();

            while (Game1.masterSaveMan.IsWritingFile()) await Task.Delay(100);

            Process currentProcess = Process.GetCurrentProcess();
            Process.Start(currentProcess.MainModule.FileName);

            Game1.ShutDown();
        }
    }
}
