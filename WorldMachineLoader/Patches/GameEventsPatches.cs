using HarmonyLib;
using OneShotMG;
using OneShotMG.src;
using OneShotMG.src.Map;
using OneShotMG.src.Menus;
using OneShotMG.src.MessageBox;
using OneShotMG.src.TWM;
using System;
using System.Collections.Generic;
using System.Reflection;
using WorldMachineLoader.API.Core;
using WorldMachineLoader.API.Events.Gameplay;
using WorldMachineLoader.API.Events.Content;

namespace WorldMachineLoader.Patches
{
    [HarmonyPatch(typeof(TileMapManager))]
    class TileMapManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("ChangeMap", new Type[] { typeof(int), typeof(float), typeof(float), typeof(float), typeof(Entity.Direction) })]
        static void ChangeMapPatch(int mapId, float xTile, float yTile, float time, Entity.Direction direction, TileMapManager __instance)
        {
            FieldInfo fieldInfo = typeof(TileMapManager).GetField("mapNames", BindingFlags.Instance | BindingFlags.NonPublic);

            Dictionary<int, string> mapNames = (Dictionary<int, string>)fieldInfo.GetValue(__instance);

            EventBus.Invoke<MapChangedEvent>(new MapChangedEvent(mapId, mapNames[mapId], __instance.GetMapName(mapId)));
        }

        [HarmonyPostfix]
        [HarmonyPatch("CheckIfStartScript")]
        static void InteractPatch(Entity e, bool investigateButtonPressed, ref Entity interactableEntity, ref bool __result)
        {
            if (__result && investigateButtonPressed && interactableEntity != null)
            {
                EventBus.Invoke<PlayerInteractedEvent>(new PlayerInteractedEvent(interactableEntity.GetID(), interactableEntity.GetName()));
            }
        }
    }

    [HarmonyPatch(typeof(ItemManager))]
    class ItemManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("AddItem")]
        static void AddItemPatch(int itemId, ItemManager __instance)
        {
            FieldInfo fieldInfo = typeof(ItemManager).GetField("itemsData", BindingFlags.Instance | BindingFlags.NonPublic);
            Dictionary<int, ItemData> itemsData = (Dictionary<int, ItemData>)fieldInfo.GetValue(__instance);

            string itemName = itemsData[itemId].name;

            EventBus.Invoke<ItemAddedEvent>(new ItemAddedEvent(itemId, itemName));
        }

        [HarmonyPostfix]
        [HarmonyPatch("RemoveItem")]
        static void RemoveItemPatch(int itemId, ItemManager __instance)
        {
            FieldInfo fieldInfo = typeof(ItemManager).GetField("itemsData", BindingFlags.Instance | BindingFlags.NonPublic);
            Dictionary<int, ItemData> itemsData = (Dictionary<int, ItemData>)fieldInfo.GetValue(__instance);

            string itemName = itemsData[itemId].name;

            EventBus.Invoke<ItemRemovedEvent>(new ItemRemovedEvent(itemId, itemName));
        }
    }

    [HarmonyPatch(typeof(UnlockManager))]
    class UnlockManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("UnlockAchievement")]
        static void UnlockAchievementPatch(string id)
        {
            EventBus.Invoke<AchievementUnlockedEvent>(new AchievementUnlockedEvent(id));
        }

        [HarmonyPostfix]
        [HarmonyPatch("UnlockProfile")]
        static void UnlockProfile(string id)
        {
            EventBus.Invoke<FriendProfileUnlockedEvent>(new FriendProfileUnlockedEvent(id));
        }

        [HarmonyPostfix]
        [HarmonyPatch("UnlockTheme")]
        static void UnlockTheme(string id)
        {
            string name = Game1.windowMan.GetThemeById(id).displayName;
            EventBus.Invoke<ThemeUnlockedEvent>(new ThemeUnlockedEvent(id, name));
        }

        [HarmonyPostfix]
        [HarmonyPatch("UnlockWallpaper")]
        static void UnlockWallpaper(string id)
        {
            string name = Game1.windowMan.Desktop.GetWallpaperInfoSaveDataFromId(id).displayName;
            EventBus.Invoke<WallpaperUnlockedEvent>(new WallpaperUnlockedEvent(id, name));
        }
    }

    [HarmonyPatch(typeof(TextBox))]
    class DialogPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Open")]
        static void OpenPatch()
        {
            EventBus.Invoke<DialogStartedEvent>(new DialogStartedEvent());
        }

        [HarmonyPostfix]
        [HarmonyPatch("Close")]
        static void ClosePatch()
        {
            EventBus.Invoke<DialogEndedEvent>(new DialogEndedEvent());
        }
    }
}
