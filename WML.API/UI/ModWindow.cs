using OneShotMG;
using OneShotMG.src.TWM;
using OneShotMG.src.Util;
using System;
using System.Collections.Generic;
using WorldMachineLoader.API.UI.Controls;
using WorldMachineLoader.API.Utils;

namespace WorldMachineLoader.API.UI
{
    /// <summary>
    /// Abstract mod window providing a base implementation for custom windows with control and theme support.
    /// </summary>
    public abstract class ModWindow : TWMWindow
    {
        private readonly List<Control> _controls = new List<Control>();

        private readonly List<Control> _controlsToRemove = new List<Control>();

        private Logger logger = new Logger("API/ModWindow");

        /// <summary>
        /// Initializes a new mod window with the specified parameters.
        /// </summary>
        /// <param name="title">Window title.</param>
        /// <param name="icon">Window icon.</param>
        /// <param name="width">Content width.</param>
        /// <param name="height">Content height.</param>
        /// <param name="addCloseButton">Whether to add a close button.</param>
        /// <param name="addMinimizeButton">Whether to add a minimize button.</param>
        protected ModWindow(string title, string icon, int width, int height,
                            bool addCloseButton = true, bool addMinimizeButton = true)
        {
            WindowTitle = title;
            WindowIcon = icon;
            ContentsSize = new Vec2(width, height);

            if (addCloseButton)
                AddButton(TWMWindowButtonType.Close);
            if (addMinimizeButton)
                AddButton(TWMWindowButtonType.Minimize);
        }

        /// <summary>
        /// Called when drawing the window contents. Can be overridden for custom drawing.
        /// </summary>
        /// <param name="theme">Theme instance.</param>
        /// <param name="pos">Window position on screen.</param>
        /// <param name="alpha">Transparency.</param>
        protected virtual void OnDraw(TWMTheme theme, Vec2 pos, byte alpha) { }

        /// <summary>
        /// Called when updating the window state. Can be overridden for custom logic.
        /// </summary>
        protected virtual void OnUpdate() { }

        /// <summary>
        /// Draws the window contents, including background and all controls.
        /// </summary>
        /// <param name="theme">Theme instance.</param>
        /// <param name="screenPos">Window position on screen.</param>
        /// <param name="alpha">Transparency.</param>
        public sealed override void DrawContents(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            GameColor bgColor = theme.Background(alpha);
            GameColor fgColor = theme.Primary(alpha);

            Rect boxRect = new Rect(screenPos.X, screenPos.Y, ContentsSize.X, ContentsSize.Y);
            Game1.gMan.ColorBoxBlit(boxRect, bgColor);

            try
            {
                foreach (var control in _controls)
                    control.Draw(theme, screenPos, alpha);
                OnDraw(theme, screenPos, alpha);
            }
            catch (Exception ex)
            {
                logger.Log($"Exception while drawing window {WindowTitle}: {ex.Message}\nStacktrace:\n{ex.StackTrace}", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                Game1.windowMan.RemoveWindow(this);
            }
        }

        /// <summary>
        /// Updates the state of all controls in the window.
        /// </summary>
        /// <param name="mouseInputWasConsumed">Whether mouse input was already consumed.</param>
        /// <returns>True if the window updated successfully; otherwise, false.</returns>
        public sealed override bool Update(bool mouseInputWasConsumed)
        {
            try
            {
                foreach (var control in _controls)
                    if (!IsModalWindowOpen())
                    {
                        bool canInteract = !mouseInputWasConsumed && !IsMinimized;
                        control.Update(Pos, canInteract);
                    }
                if (_controlsToRemove.Count > 0)
                {
                    foreach (var control in _controlsToRemove)
                        _controls.Remove(control);
                    _controlsToRemove.Clear();
                }
                OnUpdate();
            }
            catch (Exception ex)
            {
                logger.Log($"Exception while updating window {WindowTitle}: {ex.Message}\nStacktrace:\n{ex.StackTrace}", Logger.LogLevel.Error, Logger.VerbosityLevel.Minimal);
                Game1.windowMan.ShowModalWindow(ModalWindow.ModalType.Error, $"An error occured while updating window \"{WindowTitle}\". Window will be closed. See console for more info.");
                Game1.windowMan.RemoveWindow(this);
            }
            return base.Update(mouseInputWasConsumed);
        }

        /// <summary>
        /// Checks if the content of this window matches another window.
        /// </summary>
        /// <param name="window">Other window to compare.</param>
        /// <returns>True if the content matches; otherwise, false.</returns>
        public sealed override bool IsSameContent(TWMWindow window)
        {
            return window.GetType() == GetType();
        }

        /// <summary>
        /// Adds a control to the window.
        /// </summary>
        /// <param name="control">Control to add.</param>
        protected void AddControl(Control control)
        {
            _controls.Add(control);
        }

        /// <summary>
        /// Removes a control from the window.
        /// </summary>
        /// <param name="control">Control to remove.</param>
        protected void RemoveControl(Control control)
        {
            if (control == null) return;

            if (_controls.Contains(control))
            {
                _controlsToRemove.Add(control);
            }
        }
    }
}
