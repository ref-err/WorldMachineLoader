using OneShotMG.src.TWM;

namespace WorldMachineLoader.API.Events.Environment
{
    /// <summary>Event that is triggered when a game window is opened.</summary>
    public class TWMWindowAddedEvent
    {
        /// <summary>An instance of a window.</summary>
        public TWMWindow WindowInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TWMWindowAddedEvent"/> class.
        /// </summary>
        /// <param name="windowInstance">An instance of a window.</param>
        public TWMWindowAddedEvent(TWMWindow windowInstance) { WindowInstance = windowInstance; }
    }

    /// <summary>Event that is triggered when a game window is closed.</summary>
    public class TWMWindowRemovedEvent
    {
        /// <summary>An instance of a window.</summary>
        public TWMWindow WindowInstance { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TWMWindowRemovedEvent"/> class.
        /// </summary>
        /// <param name="windowInstance">An instance of a window.</param>
        public TWMWindowRemovedEvent(TWMWindow windowInstance) { WindowInstance = windowInstance; }
    }

    /// <summary>Event that is triggered when the desktop finishes loading.</summary>
    public class TWMDesktopLoadedEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TWMDesktopLoadedEvent"/> class.
        /// </summary>
        public TWMDesktopLoadedEvent() { }
    }

    /// <summary>Event that is triggered when window manager finishes initialization.</summary>
    public class WindowManagerInitializedEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowManagerInitializedEvent"/> class.
        /// </summary>
        public WindowManagerInitializedEvent() { }
    }
}
