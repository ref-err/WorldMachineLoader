using Microsoft.Xna.Framework.Graphics;
using OneShotMG;
using OneShotMG.src.EngineSpecificCode;
using OneShotMG.src.TWM;
using OneShotMG.src.Util;
using System.Collections.Generic;
using System.IO;
using WorldMachineLoader.Loader;
using WorldMachineLoader.Utils;

// i swear i'll add comments to this later
namespace WorldMachineLoader.Modding.UI
{
    internal class ModInfoWindow : TWMWindow
    {
        private readonly ModItem mod;

        private TempTexture titleTexture;

        private List<TempTexture> descriptionTextures = new List<TempTexture>();

        private TempTexture authorTexture;

        private TempTexture stateTexture;

        private TempTexture urlLabelTexture;

        private TempTexture urlTexture;

        private TempTexture experimentalTexture;

        private TextButton okButton;

        private TextButton enableButton;

        private TextButton disableButton;

        private Texture2D icon;

        private bool previousState;
        
        public ModInfoWindow(ModItem mod)
        {
            this.mod = mod;
            previousState = mod.isEnabled;

            DrawModIconTexture();

            base.WindowTitle = $"\"{mod.name}\" Info";
            base.WindowIcon = "info";
            base.ContentsSize = new Vec2(250, 150);

            var lines = TextUtils.WrapText(mod.description, 48);

            foreach (var line in lines)
            {
                var tex = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.Game, line);

                descriptionTextures.Add(tex);
                ContentsSize += new Vec2(0, 12);
            }

            okButton = new TextButton("OK", new Vec2(220, ContentsSize.Y - 20), delegate
            {
                Game1.windowMan.RemoveWindow(this);
                OnWindowClose();
            }, buttonWidth: 26);

            enableButton = new TextButton("Enable", new Vec2(4, ContentsSize.Y - 20), delegate
            {
                ModSettings.EnableMod(mod.modId);
                mod.isEnabled = true;
            });

            disableButton = new TextButton("Disable", new Vec2(4, ContentsSize.Y - 20), delegate
            {
                ModSettings.DisableMod(mod.modId);
                mod.isEnabled = false;
            });
            onClose = delegate
            {
                OnWindowClose();
            };
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
            if (icon != null)
            {
                float xScale = 64f / icon.Width;
                float yScale = 64f / icon.Height;
                Game1.gMan.MainBlit(icon, position * 2, new Rect(0, 0, icon.Width, icon.Height), xScale, yScale);
            }
            else
            {
                Vec2 size = Game1.gMan.TextureSize("the_world_machine/achievements/unknown");
                Game1.gMan.MainBlit("the_world_machine/achievements/unknown", position * 2, new Rect(0, 0, size.X, size.Y), 2f, 2f, 1f);
            }
            Game1.gMan.MainBlit(authorTexture, (position + new Vec2(35, 0)) * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
            position.Y += 20;
            Game1.gMan.MainBlit(stateTexture, (position + new Vec2(35, 0)) * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
            if (string.IsNullOrEmpty(mod.url))
            {
                position.Y += 20;
                Game1.gMan.MainBlit(urlLabelTexture, position * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
                position.Y += 12;
                Game1.gMan.MainBlit(urlTexture, position * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
            }
            if (mod.experimental && mod.isEnabled)
            {
                position.Y += 20;
                var size = Game1.gMan.TextureSize("the_world_machine/window_icons/error");
                var scale = 20f / size.X;
                Game1.gMan.MainBlit("the_world_machine/window_icons/error", position * 2, new Rect(0, 0, size.X, size.Y), scale, scale, red: gameColor.r / 255f, green: gameColor.g / 255f, blue: gameColor.b / 255f);
                position.X += 12;
                Game1.gMan.MainBlit(experimentalTexture, position * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
            }
            if (mod.isEnabled)
            {
                disableButton.Draw(screenPos, theme, alpha);
            }
            else
            {
                enableButton.Draw(screenPos, theme, alpha);
            }

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
            if (stateTexture == null || !stateTexture.isValid)
            {
                DrawStateTexture();
            }
            if (urlLabelTexture == null || !urlLabelTexture.isValid)
            {
                DrawURLLabelTexture();
            }
            if (urlTexture == null || !urlTexture.isValid)
            {
                DrawURLTexture();
            }
            if (experimentalTexture == null || !experimentalTexture.isValid)
            {
                DrawExperimentalTexture();
            }
            
            titleTexture.KeepAlive();
            authorTexture.KeepAlive();
            stateTexture.KeepAlive();
            urlLabelTexture.KeepAlive();
            urlTexture.KeepAlive();
            experimentalTexture.KeepAlive();

            if (!IsModalWindowOpen())
                if (mod.isEnabled)
                {
                    disableButton.Update(new Vec2(Pos.X + 2, Pos.Y + 26), !cursorOccluded && !base.IsMinimized);
                } else
                {
                    enableButton.Update(new Vec2(Pos.X + 2, Pos.Y + 26), !cursorOccluded && !base.IsMinimized);
                }
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

        private void DrawStateTexture()
        {
            stateTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, $"State: {(mod.isEnabled ? "Enabled" : "Disabled")}");
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

        private void OnWindowClose()
        {
            if (mod.isEnabled == previousState || Game1.windowMan.IsOneshotWindowOpen()) return;

            Globals.restartPending = true;
            Globals.restartWillEnable = mod.isEnabled;
        }

        private void DrawModIconTexture()
        {
            if (!string.IsNullOrEmpty(mod.iconPath) && File.Exists(mod.iconPath))
            {
                using (var stream = File.OpenRead(mod.iconPath))
                {
                    icon = Texture2D.FromStream(Globals.monoGame.GraphicsDevice, stream);
                }
            }
        }

        private void DrawExperimentalTexture()
        {
            experimentalTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.Game, "This mod is marked as experimental! Be careful!");
        }
    }
}
