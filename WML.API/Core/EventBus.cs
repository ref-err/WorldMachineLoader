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

        /// <summary>
        /// Registers a new event listener for the specified event type.
        /// </summary>
        /// <typeparam name="T">The event type to listen for.</typeparam>
        /// <param name="callback">The method to call when the event is invoked.</param>
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

        /// <summary>
        /// Unregisters an event listener from the specified event type.
        /// </summary>
        /// <typeparam name="T">The event type to unsubcribe from.</typeparam>
        /// <param name="callback">The method to unregister.</param>
        public static void Unsubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);
            if (_handlers.TryGetValue(type, out var list))
                list.Remove(callback);
        }

        /// <summary>
        /// Invokes the specified event, notifying all subscribed handlers.
        /// </summary>
        /// <typeparam name="T">The type of event being invoked.</typeparam>
        /// <param name="evt">The event instance to dispatch.</param>
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
