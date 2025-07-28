namespace WorldMachineLoader.API.Events
{
    public class MapChangedEvent
    {
        public int MapID { get; }
        public string InternalName { get; }
        public string LocalizedName { get; }

        public MapChangedEvent(int mapId, string name, string localizedName)
        {
            MapID = mapId;
            InternalName = name;
            LocalizedName = localizedName;
        }
    }

    public class ItemAddedEvent
    {
        public int ItemID { get; }
        public string ItemName { get; }

        public ItemAddedEvent(int itemId, string itemName)
        {
            ItemID = itemId;
            ItemName = itemName;
        }
    }

    public class ItemRemovedEvent
    {
        public int ItemID { get; }
        public string ItemName { get; }

        public ItemRemovedEvent(int itemID, string itemName)
        {
            ItemID = itemID;
            ItemName = itemName;
        }
    }

    public class PlayerInteractedEvent
    {
        public int EntityID { get; }
        public string EntityName { get; }

        public PlayerInteractedEvent(int entityID, string entityName)
        {
            EntityID = entityID;
            EntityName = entityName;
        }
    }

    public class DialogStartedEvent { }
    public class DialogEndedEvent { }
}
