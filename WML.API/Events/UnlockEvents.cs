namespace WorldMachineLoader.API.Events
{
    public class AchievementUnlockedEvent
    {
        public string AchievementID { get; }

        public AchievementUnlockedEvent(string achievementId) { AchievementID = achievementId; }
    }

    public class FriendProfileUnlockedEvent
    {
        public string ProfileID { get; }

        public FriendProfileUnlockedEvent(string profileId) { ProfileID = profileId; }
    }

    public class ThemeUnlockedEvent
    {
        public string ThemeID { get; }
        public string ThemeName { get; }

        public ThemeUnlockedEvent(string themeId, string themeName)
        {
            ThemeID = themeId;
            ThemeName = themeName;
        }
    }

    public class WallpaperUnlockedEvent
    {
        public string WallpaperID { get; }
        public string WallpaperName { get; }

        public WallpaperUnlockedEvent(string wallpaperId, string wallpaperName)
        {
            WallpaperID = wallpaperId;
            WallpaperName = wallpaperName;
        }
    }
}
