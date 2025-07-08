using System;
using System.Collections.Generic;
using System.Linq;
using WorldMachineLoader.API.Utils;

namespace WorldMachineLoader.API.Core
{
    public static class EventBus
    {
        private static Logger Logger = new Logger("EventBus");

        private static readonly Dictionary<Type, List<Delegate>> _handlers = new Dictionary<Type, List<Delegate>>();

        public static void Subscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (!_handlers.TryGetValue(type, out var list))
            {
                list = new List<Delegate>();
                _handlers[type] = list;
            }
            list.Add(callback);
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (_handlers.TryGetValue(type, out var list))
                list.Remove(callback);
        }

        public static void Invoke<T>(T evt)
        {
            var type = typeof(T);
            if (_handlers.TryGetValue(type, out var list))
            {
                foreach (var cb in list.Cast<Action<T>>())
                {
                    try
                    {
                        cb(evt);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Error when calling handler: {ex}", Logger.LogLevel.Error);
                    }
                }
            }
        }
    }
}
