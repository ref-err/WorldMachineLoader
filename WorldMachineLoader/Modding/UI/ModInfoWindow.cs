using FMOD;
using OneShotMG;
using OneShotMG.src.EngineSpecificCode;
using OneShotMG.src.TWM;
using OneShotMG.src.Util;
using System;
using System.Collections.Generic;
using WorldMachineLoader.Utils;

namespace WorldMachineLoader.Modding.UI
{
    internal class ModInfoWindow : TWMWindow
    {
        private ModItem mod;

        private TempTexture titleTexture;

        private List<TempTexture> descriptionTextures = new List<TempTexture>();

        private TempTexture authorTexture;

        private TempTexture urlLabelTexture;

        private TempTexture urlTexture;

        private TextButton okButton;
        
        public ModInfoWindow(ModItem mod)
        {
            this.mod = mod;

            base.WindowTitle = $"\"{mod.name}\" Info";
            base.WindowIcon = "info";
            base.ContentsSize = new Vec2(250, 150);

            var lines = TextUtils.WrapText(mod.description, 48);

            foreach (var line in lines)
            {
                Console.WriteLine(line);
                var tex = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.Game, line);

                descriptionTextures.Add(tex);
            }

            okButton = new TextButton("OK", new Vec2(220, 130), delegate
            {
                Game1.windowMan.RemoveWindow(this);
            }, buttonWidth: 26);

            AddButton(TWMWindowButtonType.Close);
            AddButton(TWMWindowButtonType.Minimize);
        }
        
        public override void DrawContents(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            Vec2 position = new Vec2(6, 6) + screenPos;
            GameColor gColor = theme.Background(alpha);
            GameColor gameColor = theme.Primary(alpha);
            Rect boxRect = new Rect(screenPos.X, screenPos.Y, base.ContentsSize.X, base.ContentsSize.Y);
            Game1.gMan.ColorBoxBlit(boxRect, gColor);

            Game1.gMan.MainBlit(titleTexture, position * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
            position.Y += 12;
            foreach (var descTex in descriptionTextures)
            {
                Game1.gMan.MainBlit(descTex, position * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
                position.Y += 12;
            }
            position.Y += 8;
            Game1.gMan.MainBlit(authorTexture, position * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
            position.Y += 20;
            Game1.gMan.MainBlit(urlLabelTexture, position * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
            position.Y += 12;
            Game1.gMan.MainBlit(urlTexture, position * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);

            okButton.Draw(screenPos, theme, alpha);
        }

        public override bool IsSameContent(TWMWindow window)
        {
            return window is ModListWindow;
        }

        public override bool Update(bool cursorOccluded)
        {
            if (titleTexture == null || !titleTexture.isValid)
            {
                DrawTitleTexture();
            }
            foreach (var descTex in descriptionTextures)
            {
                descTex.KeepAlive();
            }
            if (authorTexture == null || !authorTexture.isValid)
            {
                DrawAuthorTexture();
            }
            if (urlLabelTexture == null || !urlLabelTexture.isValid)
            {
                DrawURLLabelTexture();
            }
            if (urlTexture == null || !urlTexture.isValid)
            {
                DrawURLTexture();
            }
            
            titleTexture.KeepAlive();
            authorTexture.KeepAlive();
            urlLabelTexture.KeepAlive();
            urlTexture.KeepAlive();

            if (!IsModalWindowOpen())
                okButton.Update(new Vec2(Pos.X + 2, Pos.Y + 26), !cursorOccluded && !base.IsMinimized);

            return base.Update(cursorOccluded);
        }

        private void DrawTitleTexture()
        {
            titleTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, $"{mod.name} ({mod.version})");
        }

        private void DrawAuthorTexture()
        {
            authorTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, $"Author: {mod.author}");
        }

        private void DrawURLLabelTexture()
        {
            var label = "URL:";
            if (string.IsNullOrEmpty(mod.url))
                label = "";

            urlLabelTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, label);
        }

        private void DrawURLTexture()
        {
            var url = mod.url;
            if (string.IsNullOrEmpty(url))
                url = "";
            else
                url = $"{mod.url}";
            urlTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.Game, url);
        }
    }
}
