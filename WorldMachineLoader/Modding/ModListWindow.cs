using OneShotMG;
using OneShotMG.src;
using OneShotMG.src.EngineSpecificCode;
using OneShotMG.src.TWM;
using OneShotMG.src.TWM.Filesystem;
using OneShotMG.src.Util;
using System.Collections.Generic;

namespace WorldMachineLoader.Modding
{
    public class ModListWindow : TWMWindow
    {
        private List<ModItem> mods;

        private TempTexture noModsTexture;

        public ModListWindow(string displayName)
        {
            mods = Globals.mods;
            base.WindowIcon = "oneshot";
            base.WindowTitle = displayName;
            base.ContentsSize = new Vec2(300, 200);
            AddButton(TWMWindowButtonType.Close);
            AddButton(TWMWindowButtonType.Minimize);
        }

        // currently, im not handling the scenario when there are more than ~6 mods,
        // so it may render very strangely. i would have used SliderControl from OneShotMG.src.TWM
        // if it weren't internal. i'll try to implement pagination instead of scrolling, just because
        // i think it's easier to implement it.
        public override void DrawContents(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            Vec2 position = new Vec2(6, 6) + screenPos;

            GameColor gColor = theme.Background(alpha);
            GameColor gameColor = theme.Primary(alpha);
            Rect boxRect = new Rect(screenPos.X, screenPos.Y, base.ContentsSize.X, base.ContentsSize.Y);
            Game1.gMan.ColorBoxBlit(boxRect, gColor);

            if (mods.Count == 0)
            {
                Game1.gMan.MainBlit(noModsTexture, position * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
            }
            
            foreach (var mod in mods)
            {
                Game1.gMan.MainBlit(mod.titleTexture, position * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
                position.Y += 12;
                Game1.gMan.MainBlit(mod.descriptionTexture, position * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
                position.Y += 20;
            }
        }

        public override bool Update(bool cursorOccluded)
        {
            if ((noModsTexture == null || !noModsTexture.isValid) && mods.Count == 0)
            {
                DrawNoModsTexture();
            }

            foreach (var mod in mods)
            {
                if (mod.titleTexture == null || !mod.titleTexture.isValid)
                {
                    DrawTitleTexture();
                }
                if (mod.descriptionTexture == null || !mod.descriptionTexture.isValid)
                {
                    DrawDescriptionTexture();
                }

                mod.titleTexture.KeepAlive();
                mod.descriptionTexture.KeepAlive();
            }
            return base.Update(cursorOccluded);
        }

        public override bool IsSameContent(TWMWindow window)
        {
            return window is ModListWindow;
        }

        private void DrawTitleTexture()
        {
            foreach (var mod in mods)
            {
                mod.titleTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, mod.title);
            }
        }

        private void DrawDescriptionTexture()
        {
            foreach (var mod in mods)
            {
                mod.descriptionTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.Game, mod.description);
            }
        }

        private void DrawNoModsTexture()
        {
            noModsTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, "You don't have any mods loaded!");
        }
    }

    public class ModListIcon : FileIcon
    {
        //public delegate void DesktopIconAction();

        private string iconText1;

        private string iconText2;

        public readonly TWMFileNode file;

        public readonly string iconImagePath;

        public const GraphicsManager.FontType iconFont = GraphicsManager.FontType.OS;

        private GlitchEffect glitchEffect;

        public ModListIcon(TWMFileNode file) : base(file)
        {
            this.file = file;
            iconImagePath = "the_world_machine/desktop_icons/oneshot";
            Vec2 size = new Vec2(32, 32);
            glitchEffect = new GlitchEffect(size, 100, 900, 900, 20);
        }

        public void Draw(TWMTheme theme, Vec2 pos, bool focus = false, bool canHover = false, float alpha = 1f)
        {
            GameColor gColor = theme.Background();
            GameColor gColor2 = (focus ? theme.Primary() : theme.Variant());
            GameColor gameColor = theme.Primary();
            GameColor gameColor2 = theme.Background();

            Vec2 vec = pos + new Vec2(26, 6);

            if (Game1.windowMan.Desktop.inSolstice)
            {
                glitchEffect.Draw(iconImagePath, vec + new Vec2(1, 1), gameColor2.af, gameColor2.rf, gameColor2.gf, gameColor2.bf, GraphicsManager.BlendMode.Normal, TextureCache.CacheType.TheWorldMachine);
                glitchEffect.Draw(iconImagePath, vec, gameColor.af, gameColor.rf, gameColor.gf, gameColor.bf, GraphicsManager.BlendMode.Normal, TextureCache.CacheType.TheWorldMachine);
            }
            else
            {
                Game1.gMan.MainBlit(iconImagePath, vec + new Vec2(1, 1), gameColor2, 0, GraphicsManager.BlendMode.Normal, 2, TextureCache.CacheType.TheWorldMachine);
                Game1.gMan.MainBlit(iconImagePath, vec, gameColor, 0, GraphicsManager.BlendMode.Normal, 2, TextureCache.CacheType.TheWorldMachine);
            }
        }
    }
}
