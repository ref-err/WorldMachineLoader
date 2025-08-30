﻿using Microsoft.Xna.Framework.Graphics;
using OneShotMG;
using OneShotMG.src.EngineSpecificCode;
using OneShotMG.src.TWM;
using OneShotMG.src.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WorldMachineLoader.Modding.UI;
using WorldMachineLoader.Utils;

// i swear i'll add comments to this later
namespace WorldMachineLoader.Modding
{
    public class ModListWindow : TWMWindow
    {
        private List<ModItem> totalMods = new List<ModItem>();

        private List<ModItem> mods;

        private List<ModItem> disabledMods;

        private List<TextButton> infoButtons = new List<TextButton>();

        private TextButton restartGameButton;

        private TextButton prevPageButton;
        private TextButton nextPageButton;

        private TempTexture noModsTexture;

        private TempTexture pageCountTexture;

        private List<Texture2D> modIcons = new List<Texture2D>();

        private int currentPage = 0;
        private int lastPage = -1;
        private const int ModsPerPage = 5;

        public ModListWindow()
        {
            mods = Globals.mods;
            disabledMods = Globals.disabledMods;

            foreach (var mod in mods)
            {
                totalMods.Add(mod);
            }

            foreach (var mod in disabledMods)
            {
                totalMods.Add(mod);
            }

            CreateModIcons();

            base.WindowIcon = "oneshot";
            base.WindowTitle = $"Mod List (Loaded {mods.Count} mods.)";
            base.ContentsSize = new Vec2(300, 200);

            CreateInfoButtons();

            restartGameButton = new TextButton("Restart", new Vec2(240, 180), delegate
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
                        GameUtils.RestartGame();
                    }
                });
            });

            prevPageButton = new TextButton("<", new Vec2(125, 175), delegate
            {
                if (currentPage > 0)
                {
                    currentPage--;
                    CreateInfoButtons();
                    CreateModIcons();
                }
            }, 16);

            nextPageButton = new TextButton(">", new Vec2(159, 175), delegate
            {
                if (currentPage < GetTotalPages())
                {
                    currentPage++;
                    CreateInfoButtons();
                    CreateModIcons();
                }
            }, 16);

            AddButton(TWMWindowButtonType.Close);
            AddButton(TWMWindowButtonType.Minimize);
        }

        public override void DrawContents(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            Vec2 position = new Vec2(32, 6) + screenPos;
            Vec2 iconPos = new Vec2(6, 6) + screenPos;

            GameColor gColor = theme.Background(alpha);
            GameColor gameColor = theme.Primary(alpha);

            Rect boxRect = new Rect(screenPos.X, screenPos.Y, base.ContentsSize.X, base.ContentsSize.Y);
            Rect lineRect = new Rect(screenPos.X, screenPos.Y + 164, base.ContentsSize.X, 2);

            Game1.gMan.ColorBoxBlit(boxRect, gColor);
            Game1.gMan.ColorBoxBlit(lineRect, gameColor);

            if (totalMods.Count == 0)
            {
                Game1.gMan.MainBlit(noModsTexture, (new Vec2(6, 6) + screenPos) * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
            }

            foreach (var mod in GetModsForCurrentPage())
            {
                if (mod.experimental && mod.isEnabled)
                {
                    var size = Game1.gMan.TextureSize("the_world_machine/window_icons/error");
                    Game1.gMan.MainBlit("the_world_machine/window_icons/error", position * 2, new Rect(0, 0, size.X, size.Y), 1.5f, 1.5f, red: gameColor.r / 255f, green: gameColor.g / 255f, blue: gameColor.b / 255f);
                }
                var titlePos = mod.experimental ? ((position * 2) + new Vec2(24, 0)) : (position * 2);
                Game1.gMan.MainBlit(mod.titleTexture, titlePos, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
                position.Y += 12;
                Game1.gMan.MainBlit(mod.descriptionTexture, position * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
                position.Y += 20;
            }

            foreach (var icon in modIcons)
            {
                if (icon != null)
                {
                    float xScale = 48f / icon.Width;
                    float yScale = 48f / icon.Height;
                    Game1.gMan.MainBlit(icon, iconPos * 2, new Rect(0, 0, icon.Width, icon.Height), xScale, yScale);
                }
                else
                {
                    Vec2 size = Game1.gMan.TextureSize("the_world_machine/achievements/unknown");
                    Game1.gMan.MainBlit("the_world_machine/achievements/unknown", iconPos * 2, new Rect(0, 0, size.X, size.Y), 1.5f, 1.5f, 1f);
                }
                iconPos.Y += 32;
            }
            Game1.gMan.MainBlit(pageCountTexture, (new Vec2(146, 177) + screenPos) * 2, gameColor, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);

            restartGameButton.Draw(screenPos, theme, alpha);

            if (currentPage > 0)
                prevPageButton.Draw(screenPos, theme, alpha);
            if (currentPage < GetTotalPages())
                nextPageButton.Draw(screenPos, theme, alpha);

            foreach (var btn in infoButtons)
                btn.Draw(screenPos, theme, alpha);
        }

        public override bool Update(bool cursorOccluded)
        {
            if ((noModsTexture == null || !noModsTexture.isValid) && totalMods.Count == 0)
            {
                DrawNoModsTexture();
            }

            foreach (var mod in mods)
            {
                if (mod.titleTexture == null || !mod.titleTexture.isValid)
                {
                    DrawTitleTexture(mod);
                }
                if (mod.descriptionTexture == null || !mod.descriptionTexture.isValid)
                {
                    DrawDescriptionTexture(mod);
                }

                mod.titleTexture.KeepAlive();
                mod.descriptionTexture.KeepAlive();
            }

            foreach (var mod in disabledMods)
            {
                if (mod.titleTexture == null || !mod.titleTexture.isValid)
                {
                    DrawTitleTexture(mod);
                }
                if (mod.descriptionTexture == null || !mod.descriptionTexture.isValid)
                {
                    DrawDescriptionTexture(mod);
                }

                mod.titleTexture.KeepAlive();
                mod.descriptionTexture.KeepAlive();
            }

            if (currentPage != lastPage)
            {
                DrawPageCountTexture();
                lastPage = currentPage;
            }
            pageCountTexture.KeepAlive();
            
            pageCountTexture.KeepAlive();

            if (!IsModalWindowOpen())
            {
                bool canInteract = !cursorOccluded && !base.IsMinimized;

                restartGameButton.Update(new Vec2(Pos.X + 2, Pos.Y + 26), canInteract);

                if (currentPage > 0)
                    prevPageButton.Update(new Vec2(Pos.X + 2, Pos.Y + 26), canInteract);

                if (currentPage < GetTotalPages())
                    nextPageButton.Update(new Vec2(Pos.X + 2, Pos.Y + 26), canInteract);

                foreach (var btn in infoButtons)
                    btn.Update(new Vec2(Pos.X + 2, Pos.Y + 26), canInteract);
            }
            
            if (Globals.restartPending)
            {
                Globals.restartPending = false;

                var msg = Globals.restartWillEnable
                    ? "It seems that you have enabled a mod. Do you want to restart the game to load this mod?"
                     : "It seems that you have disabled a mod. Do you want to restart the game to unload this mod?";

                ShowModalWindow(ModalWindow.ModalType.YesNo, msg,
                    delegate (ModalWindow.ModalResponse res)
                    {
                        if (res == ModalWindow.ModalResponse.Yes)
                        {
                            GameUtils.RestartGame();
                        }
                    });
            }
            return base.Update(cursorOccluded);
        }

        public override bool IsSameContent(TWMWindow window)
        {
            return window is ModListWindow;
        }

        private void DrawTitleTexture(ModItem mod)
        {
            mod.titleTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, mod.title);
        }

        private void DrawDescriptionTexture(ModItem mod)
        {
            var desc = mod.description;
            if (string.IsNullOrEmpty(desc))
                desc = "No description provided";
            else if (desc.Length < 40)
                desc = mod.description;
            else
                desc = mod.description.Substring(0, 40) + "...";
            mod.descriptionTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.Game, desc);
        }

        private void DrawNoModsTexture()
        {
            noModsTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, "You don't have any mods loaded!");
        }

        private void DrawPageCountTexture()
        {
            pageCountTexture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, (currentPage + 1).ToString());
        }

        private void OnInfoButtonClicked(ModItem mod)
        {
            Game1.windowMan.AddWindow(new ModInfoWindow(mod));
        }

        private void CreateInfoButtons()
        {
            int y = 8;
            infoButtons.Clear();
            foreach (var mod in GetModsForCurrentPage())
            {
                var btn = new TextButton("Info", new Vec2(252, y), delegate { OnInfoButtonClicked(mod); }, buttonWidth: 40);
                infoButtons.Add(btn);
                y += 32;
            }
        }

        private void CreateModIcons()
        {
            modIcons.Clear();
            foreach (var mod in GetModsForCurrentPage())
            {
                if (!string.IsNullOrEmpty(mod.iconPath) && File.Exists(mod.iconPath))
                {
                    using (var stream = File.OpenRead(mod.iconPath))
                    {
                        var texture = Texture2D.FromStream(Globals.monoGame.GraphicsDevice, stream);
                        modIcons.Add(texture);
                        continue;
                    }
                }

                modIcons.Add(null);
            }
        }

        private List<ModItem> GetModsForCurrentPage()
        {
            return totalMods
                .Skip(currentPage * ModsPerPage)
                .Take(ModsPerPage)
                .ToList();
        }

        private int GetTotalPages()
        {
            int totalPages = (int)Math.Ceiling(totalMods.Count * 1.0 / ModsPerPage);
            return totalPages - 1;
        }
    }
}
