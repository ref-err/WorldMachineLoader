using OneShotMG;
using System;
using System.Collections.Generic;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Utils;

namespace WorldMachineLoader.API.UI
{
    /// <summary>
    /// Registry for mod-provided windows.
    /// Maintains a mapping of registered windows keyed by "modId/WindowTypeName".
    /// Provides registration, lookup, and factory creation for ModWindow types.
    /// </summary>
    public static class WindowRegistry
    {
        private static readonly Logger logger = new Logger("API/WindowRegistry");

        private static readonly Dictionary<string, WindowInfo> _windows = new Dictionary<string, WindowInfo>();

        /// <summary>
        /// Read-only view of all registered windows.
        /// </summary>
        public static IReadOnlyDictionary<string, WindowInfo> All => _windows;

        /// <summary>
        /// Register a window type for a mod.
        /// Throws ArgumentNullException if <paramref name="modContext"/> is null.
        /// </summary>
        /// <typeparam name="T">Type of the window to register. Must derive from ModWindow.</typeparam>
        /// <param name="modContext">Context of the registering mod (used to build the key).</param>
        /// <param name="windowName">Display name for the window.</param>
        public static void Register<T>(ModContext modContext, string windowName) where T : ModWindow
        {
            if (modContext == null)
                throw new ArgumentNullException(nameof(modContext));
            
            string key = $"{modContext.ModID}/{typeof(T).Name}"; // "author.modid/WindowName"

            if (_windows.ContainsKey(key))
            {
                logger.Log($"Window \"{key}\" already registered!", Logger.LogLevel.Error);
                return;
            }

            _windows[key] = new WindowInfo(typeof(T), windowName);
            logger.Log($"Registered window \"{key}\" for mod ID \"{modContext.ModID}\".", Logger.LogLevel.Info, Logger.VerbosityLevel.Detailed);
        }

        /// <summary>
        /// Get WindowInfo by its registry key. Returns null if not found.
        /// </summary>
        /// <param name="key">Registry key in the form "modId/WindowTypeName".</param>
        /// <returns>Info instance or null if not registered.</returns>
        public static WindowInfo Get(string key)
        {
            return _windows.TryGetValue(key, out var window) ? window : null;
        }

        /// <summary>
        /// Returns a dictionary of all registered windows that belong to the given mod ID.
        /// </summary>
        /// <param name="modId">Mod ID to filter by.</param>
        /// <returns>Dictionary of key => WindowInfo for the given mod id.</returns>
        public static IReadOnlyDictionary<string, WindowInfo> GetByMod(string modId)
        {
            var result = new Dictionary<string, WindowInfo>();
            foreach (var kv in _windows)
            {
                if (kv.Key.StartsWith(modId + "/", StringComparison.OrdinalIgnoreCase))
                    result[kv.Key] = kv.Value;
            }
            return result;
        }

        /// <summary>
        /// Attempts to create a new instance of the registered ModWindow type for the given key.
        /// On exception, logs an error and shows an error message in game.
        /// </summary>
        /// <param name="key">Registry key in the form "modId/WindowTypeName".</param>
        /// <returns>New ModWindow instance or null if creation failed or key not found.</returns>
        public static ModWindow Create(string key)
        {
            try
            {
                if (_windows.TryGetValue(key, out var info))
                {
                    logger.Log($"Trying to create window of type {info.WindowType.GetType().FullName}", Logger.LogLevel.Info, Logger.VerbosityLevel.Detailed);
                    return Activator.CreateInstance(info.WindowType) as ModWindow;
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Exception while trying to create a new window instance: {ex.Message}.\nStacktrace:\n{ex.StackTrace}", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                Game1.windowMan.ShowModalWindow(OneShotMG.src.TWM.ModalWindow.ModalType.Error, "An error occured while trying to create a new window instance. See console for more info.");
            }
            return null;
        }
    }

    /// <summary>
    /// Class that holds information about a registered window type.
    /// </summary>
    public class WindowInfo
    {
        /// <summary>
        /// The window Type (derived from ModWindow).
        /// </summary>
        public Type WindowType { get; }

        /// <summary>
        /// Display name for the window.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Construct a WindowInfo describing a window Type and its display name.
        /// </summary>
        /// <param name="type">The Type of the window.</param>
        /// <param name="displayName">Display name for this window.</param>
        public WindowInfo(Type type, string displayName)
        {
            DisplayName = displayName;
            WindowType = type;
        }
    }
}
