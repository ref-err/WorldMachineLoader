namespace WorldMachineLoader.API.Events
{
    /// <summary>Event that is triggered when a new achievement gets unlocked.</summary>
    public class AchievementUnlockedEvent
    {
        /// <summary>The unique ID of the unlocked achievement.</summary>
        public string AchievementID { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AchievementUnlockedEvent"/> class.
        /// </summary>
        /// <param name="achievementId">The unique ID of the unlocked achievement.</param>
        public AchievementUnlockedEvent(string achievementId) { AchievementID = achievementId; }
    }

    /// <summary>Event that is triggered when a new friend profile gets unlocked.</summary>
    public class FriendProfileUnlockedEvent
    {
        /// <summary>The unique ID of the unlocked friend profile.</summary>
        public string ProfileID { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendProfileUnlockedEvent"/> class.
        /// </summary>
        /// <param name="profileId">The unique ID of the unlocked friend profile.</param>
        public FriendProfileUnlockedEvent(string profileId) { ProfileID = profileId; }
    }

    /// <summary>Event that is triggered when a new theme gets unlocked.</summary>
    public class ThemeUnlockedEvent
    {
        /// <summary>The unique ID of the unlocked theme.</summary>
        public string ThemeID { get; }

        /// <summary>The name of the unlocked theme.</summary>
        public string ThemeName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeUnlockedEvent"/> class.
        /// </summary>
        /// <param name="themeId">The unique ID of the unlocked theme.</param>
        /// <param name="themeName">The name of the unlocked theme.</param>
        public ThemeUnlockedEvent(string themeId, string themeName)
        {
            ThemeID = themeId;
            ThemeName = themeName;
        }
    }

    /// <summary>Event that is triggered when a new wallpaper gets unlocked.</summary>
    public class WallpaperUnlockedEvent
    {
        /// <summary>The unique ID of the unlocked wallpaper.</summary>
        public string WallpaperID { get; }

        /// <summary>The name of the unlocked wallpaper.</summary>
        public string WallpaperName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WallpaperUnlockedEvent"/> class.
        /// </summary>
        /// <param name="wallpaperId">The unique ID of the unlocked wallpaper.</param>
        /// <param name="wallpaperName">The name of the unlocked wallpaper.</param>
        public WallpaperUnlockedEvent(string wallpaperId, string wallpaperName)
        {
            WallpaperID = wallpaperId;
            WallpaperName = wallpaperName;
        }
    }
}
