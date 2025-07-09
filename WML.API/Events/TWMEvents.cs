namespace WorldMachineLoader.API.Events
{
    public class TWMWindowAddedEvent
    {
        public object WindowInstance { get; }

        public TWMWindowAddedEvent(object windowInstance) { WindowInstance = windowInstance; }
    }

    public class TWMWindowRemovedEvent
    {
        public object WindowInstance { get; }

        public TWMWindowRemovedEvent(object windowInstance) { WindowInstance = windowInstance; }
    }

    public class TWMDesktopLoadedEvent
    {
        public object DesktopInstance { get; }

        public TWMDesktopLoadedEvent(object desktopInstance) { DesktopInstance = desktopInstance; }
    }
}
