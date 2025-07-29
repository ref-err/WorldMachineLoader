namespace WorldMachineLoader.API.Events
{
    /// <summary>
    /// Event that is triggered when the player moves to another map/location.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This event is invoked regardless of how the map transfer occurs:
    /// </para>
    /// <para>
    /// - When the player walks to another location by themself.
    /// </para>
    /// <para>
    /// - When the player uses "Fast Travel" feature.
    /// </para>
    /// </remarks>
    public class MapChangedEvent
    {
        /// <summary>The ID of the map.</summary>
        public int MapID { get; }

        /// <summary>The internal name of the map.</summary>
        public string InternalName { get; }

        /// <summary>The display name of the map.</summary>
        /// <remarks>
        /// This localized name is based on the language set in The World Machine settings.
        /// </remarks>
        public string LocalizedName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChangedEvent"/> class.
        /// </summary>
        /// <param name="mapId">The ID of the map.</param>
        /// <param name="name">The internal name of the map.</param>
        /// <param name="localizedName">The display name of the map.</param>
        public MapChangedEvent(int mapId, string name, string localizedName)
        {
            MapID = mapId;
            InternalName = name;
            LocalizedName = localizedName;
        }
    }

    /// <summary>Event that is triggered when a new item gets added to the player's inventory.</summary>
    public class ItemAddedEvent
    {
        /// <summary>The ID of an item.</summary>
        public int ItemID { get; }

        /// <summary>The name of an item.</summary>
        public string ItemName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemAddedEvent"/> class.
        /// </summary>
        /// <param name="itemId">The ID of an item.</param>
        /// <param name="itemName">The name of an item.</param>
        public ItemAddedEvent(int itemId, string itemName)
        {
            ItemID = itemId;
            ItemName = itemName;
        }
    }

    /// <summary>Event that is triggered when an item gets removed from the player's inventory.</summary>
    public class ItemRemovedEvent
    {
        /// <summary>The ID of an item.</summary>
        public int ItemID { get; }

        /// <summary>The name of an item.</summary>
        public string ItemName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemRemovedEvent"/> class.
        /// </summary>
        /// <param name="itemID">The ID of an item.</param>
        /// <param name="itemName">The name of an item.</param>
        public ItemRemovedEvent(int itemID, string itemName)
        {
            ItemID = itemID;
            ItemName = itemName;
        }
    }

    /// <summary>Event that is triggered when the player interacts with an entity (interactable object).</summary>
    public class PlayerInteractedEvent
    {
        /// <summary>The ID on an entity.</summary>
        public int EntityID { get; }

        /// <summary>The name of an entity.</summary>
        public string EntityName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerInteractedEvent"/> class.
        /// </summary>
        /// <param name="entityID">The ID of an entity.</param>
        /// <param name="entityName">The name of an entity.</param>
        public PlayerInteractedEvent(int entityID, string entityName)
        {
            EntityID = entityID;
            EntityName = entityName;
        }
    }

    /// <summary>Event that is triggered when a dialog starts.</summary>
    public class DialogStartedEvent { }

    /// <summary>Event that is triggered when a dialog ends.</summary>
    public class DialogEndedEvent { }
}
