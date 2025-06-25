using OneShotMG;
using OneShotMG.src.EngineSpecificCode;
using OneShotMG.src.TWM;
using OneShotMG.src.Util;
using System.Collections.Generic;
using System.Diagnostics;
using WorldMachineLoader.Modding.UI;

namespace WorldMachineLoader.Modding
{
    public class ModListWindow : TWMWindow
    {
        private List<ModItem> mods;

        private List<TextButton> infoButtons = new List<TextButton>();

        private TextButton restartGameButton;

        private TempTexture noModsTexture;

        public ModListWindow()
        {
            mods = Globals.mods;
            base.WindowIcon = "oneshot";
            base.WindowTitle = $"Mod List (Loaded {mods.Count} mods.)";
            base.ContentsSize = new Vec2(300, 200);

            CreateInfoButtons();

            restartGameButton = new TextButton("Restart", new Vec2(240, 180), delegate
            {
                RestartGame();
            });

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

            restartGameButton.Draw(screenPos, theme, alpha);

            foreach (var btn in infoButtons)
                btn.Draw(screenPos, theme, alpha);
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
            if (!IsModalWindowOpen())
                restartGameButton.Update(new Vec2(Pos.X + 2, Pos.Y + 26), !cursorOccluded && !base.IsMinimized);
                foreach (var btn in infoButtons)
                    btn.Update(new Vec2(Pos.X + 2, Pos.Y + 26), !cursorOccluded && !base.IsMinimized);

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
                var desc = mod.description;
                if (string.IsNullOrEmpty(desc))
                    desc = "No description provided";
                else if (desc.Length < 55)
                    desc = mod.description;
                else
                    desc = mod.description.Substring(0, 40) + "...";
                mod.descriptionTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.Game, desc);
            }
        }

        private void DrawNoModsTexture()
        {
            noModsTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, "You don't have any mods loaded!");
        }

        private void OnInfoButtonClicked(ModItem mod)
        {
            Game1.windowMan.AddWindow(new ModInfoWindow(mod));
            
        }

        private void CreateInfoButtons()
        {
            int y = 8;
            foreach (var mod in mods)
            {
                var btn = new TextButton("Info", new Vec2(252, y), delegate { OnInfoButtonClicked(mod); }, buttonWidth: 40);
                infoButtons.Add(btn);
                y += 32;
            }
        }

        private void RestartGame()
        {
            if (Game1.windowMan.IsOneshotWindowOpen())
            {
                ShowModalWindow(ModalWindow.ModalType.Error, "cant_shutdown_while_oneshot_running");
                return;
            }
            ShowModalWindow(ModalWindow.ModalType.YesNo, "Do you really want to restart?", delegate (ModalWindow.ModalResponse res)
            {
                if (res == ModalWindow.ModalResponse.Yes)
                {
                    Game1.windowMan.SaveDesktopAndFileSystem();

                    Process currentProcess = Process.GetCurrentProcess();
                    Process.Start(currentProcess.MainModule.FileName);

                    Game1.ShutDown();
                }
            });
        }
    }
}
