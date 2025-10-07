using System;
using System.Collections.Generic;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Utils;

namespace WorldMachineLoader.API.UI
{
    public static class WindowRegistry
    {
        private static readonly Logger logger = new Logger("API/WindowRegistry");

        private static readonly Dictionary<string, WindowInfo> _windows = new Dictionary<string, WindowInfo>();

        public static IReadOnlyDictionary<string, WindowInfo> All => _windows;

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

        public static WindowInfo Get(string key)
        {
            return _windows.TryGetValue(key, out var window) ? window : null;
        }

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

        public static ModWindow Create(string key)
        {
            if (_windows.TryGetValue(key, out var info))
            {
                logger.Log($"Trying to create window of type {info.WindowType.GetType().FullName}", Logger.LogLevel.Info, Logger.VerbosityLevel.Detailed);
                return Activator.CreateInstance(info.WindowType) as ModWindow;
            }
            return null;
        }
    }

    public class WindowInfo
    {
        public Type WindowType { get; }

        public string DisplayName { get; }

        public WindowInfo(Type type, string displayName)
        {
            DisplayName = displayName;
            WindowType = type;
        }
    }
}
