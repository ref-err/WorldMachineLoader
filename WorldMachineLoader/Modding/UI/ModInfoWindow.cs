using Microsoft.Xna.Framework.Graphics;
using OneShotMG;
using OneShotMG.src.EngineSpecificCode;
using OneShotMG.src.TWM;
using OneShotMG.src.Util;
using System.Collections.Generic;
using System.IO;
using WorldMachineLoader.ModLoader;
using WorldMachineLoader.Utils;

// i swear i'll add comments to this later
namespace WorldMachineLoader.Modding.UI
{
    internal class ModInfoWindow : TWMWindow
    {
        private readonly ModItem mod;
        private bool previousState;

        private TempTexture titleTexture;
        private List<TempTexture> descriptionTextures = new();
        private TempTexture authorTexture;
        private TempTexture stateTexture;
        private TempTexture urlLabelTexture;
        private TempTexture urlTexture;
        private TempTexture experimentalTexture;

        private TextButton okButton;
        private TextButton enableButton;
        private TextButton disableButton;

        private Texture2D icon;

        public ModInfoWindow(ModItem mod)
        {
            this.mod = mod;
            previousState = mod.isEnabled;

            DrawModIconTexture();

            WindowTitle = $"\"{mod.name}\" Info";
            WindowIcon = "info";
            ContentsSize = new Vec2(250, 150);
			//реферр чем ты думал когда писал это
            foreach (var line in TextUtils.WrapText(mod.description, 48))
            {
                descriptionTextures.Add(Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.Game, line));
                ContentsSize += new Vec2(0, 12);
            }

            okButton = new TextButton("OK", new Vec2(220, ContentsSize.Y - 20), () =>
            {
                Game1.windowMan.RemoveWindow(this);
                OnWindowClose();
            }, buttonWidth: 26);

            enableButton = new TextButton("Enable", new Vec2(4, ContentsSize.Y - 20), () =>
            {
                ModSettings.EnableMod(mod.modId);
                mod.isEnabled = true;
            });

            disableButton = new TextButton("Disable", new Vec2(4, ContentsSize.Y - 20), () =>
            {
                ModSettings.DisableMod(mod.modId);
                mod.isEnabled = false;
            });

            onClose = OnWindowClose;

            AddButton(TWMWindowButtonType.Close);
            AddButton(TWMWindowButtonType.Minimize);
        }

        public override void DrawContents(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            Vec2 pos = screenPos + new Vec2(6, 6);
            GameColor bgColor = theme.Background(alpha);
            GameColor textColor = theme.Primary(alpha);
            Rect box = new(screenPos.X, screenPos.Y, ContentsSize.X, ContentsSize.Y);
            Game1.gMan.ColorBoxBlit(box, bgColor);

            Game1.gMan.MainBlit(titleTexture, pos * 2, textColor);
            pos.Y += 12;

            foreach (var tex in descriptionTextures)
            {
                Game1.gMan.MainBlit(tex, pos * 2, textColor);
                pos.Y += 12;
            }

            pos.Y += 8;
            if (icon != null)
            {
                float xScale = 64f / icon.Width;
                float yScale = 64f / icon.Height;
                Game1.gMan.MainBlit(icon, pos * 2, new Rect(0, 0, icon.Width, icon.Height), xScale, yScale);
            }
            else
            {
                Vec2 size = Game1.gMan.TextureSize("the_world_machine/achievements/unknown");
                Game1.gMan.MainBlit("the_world_machine/achievements/unknown", pos * 2, new Rect(0, 0, size.X, size.Y), 2f, 2f);
            }

            Game1.gMan.MainBlit(authorTexture, (pos + new Vec2(35, 0)) * 2, textColor);
            pos.Y += 20;

            Game1.gMan.MainBlit(stateTexture, (pos + new Vec2(35, 0)) * 2, textColor);

            if (!string.IsNullOrEmpty(mod.url))
            {
                pos.Y += 20;
                Game1.gMan.MainBlit(urlLabelTexture, pos * 2, textColor);
                pos.Y += 12;
                Game1.gMan.MainBlit(urlTexture, pos * 2, textColor);
            }

            if (mod.experimental && mod.isEnabled)
            {
                pos.Y += 20;
                Vec2 size = Game1.gMan.TextureSize("the_world_machine/window_icons/error");
                float scale = 20f / size.X;
                Game1.gMan.MainBlit("the_world_machine/window_icons/error", pos * 2, new Rect(0, 0, size.X, size.Y), scale, scale, textColor.r / 255f, textColor.g / 255f, textColor.b / 255f);
                pos.X += 12;
                Game1.gMan.MainBlit(experimentalTexture, pos * 2, textColor);
            }

            if (mod.isEnabled)
                disableButton.Draw(screenPos, theme, alpha);
            else
                enableButton.Draw(screenPos, theme, alpha);

            okButton.Draw(screenPos, theme, alpha);
        }

        public override bool IsSameContent(TWMWindow window) => window is ModListWindow;

        public override bool Update(bool cursorOccluded)
        {
            titleTexture ??= Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, $"{mod.name} ({mod.version})");
            authorTexture ??= Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, $"Author: {mod.author}");
            stateTexture ??= Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, $"State: {(mod.isEnabled ? "Enabled" : "Disabled")}");
            urlLabelTexture ??= Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, string.IsNullOrEmpty(mod.url) ? "" : "URL:");
            urlTexture ??= Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.Game, mod.url ?? "");
            experimentalTexture ??= Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.Game, "This mod is marked as experimental! Be careful!");

            titleTexture.KeepAlive();
            authorTexture.KeepAlive();
            stateTexture.KeepAlive();
            urlLabelTexture.KeepAlive();
            urlTexture.KeepAlive();
            experimentalTexture.KeepAlive();
            descriptionTextures.ForEach(tex => tex.KeepAlive());

            if (!IsModalWindowOpen())
            {
                var pos = new Vec2(Pos.X + 2, Pos.Y + 26);
                if (mod.isEnabled)
                    disableButton.Update(pos, !cursorOccluded && !IsMinimized);
                else
                    enableButton.Update(pos, !cursorOccluded && !IsMinimized);

                okButton.Update(pos, !cursorOccluded && !IsMinimized);
            }

            return base.Update(cursorOccluded);
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
                using var stream = File.OpenRead(mod.iconPath);
                icon = Texture2D.FromStream(Globals.monoGame.GraphicsDevice, stream);
            }
        }
    }
}