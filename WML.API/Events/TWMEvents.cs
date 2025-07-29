namespace WorldMachineLoader.API.Events
{
    /// <summary>Event that is triggered when a game window is opened.</summary>
    public class TWMWindowAddedEvent
    {
        /// <summary>An instance of a window.</summary>
        public object WindowInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TWMWindowAddedEvent"/> class.
        /// </summary>
        /// <param name="windowInstance">An instance of a window.</param>
        public TWMWindowAddedEvent(object windowInstance) { WindowInstance = windowInstance; }
    }

    /// <summary>Event that is triggered when a game window is closed.</summary>
    public class TWMWindowRemovedEvent
    {
        /// <summary>An instance of a window.</summary>
        public object WindowInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TWMWindowRemovedEvent"/> class.
        /// </summary>
        /// <param name="windowInstance">An instance of a window.</param>
        public TWMWindowRemovedEvent(object windowInstance) { WindowInstance = windowInstance; }
    }

    /// <summary>Event that is triggered when the desktop finishes loading.</summary>
    public class TWMDesktopLoadedEvent
    {
        /// <summary>An instance of the desktop.</summary>
        public object DesktopInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TWMDesktopLoadedEvent"/> class.
        /// </summary>
        /// <param name="desktopInstance">An instance of the desktop.</param>
        public TWMDesktopLoadedEvent(object desktopInstance) { DesktopInstance = desktopInstance; }
    }
}
