using OneShotMG;
using OneShotMG.src.TWM;
using OneShotMG.src.Util;
using System.Collections.Generic;
using WorldMachineLoader.API.UI.Controls;

namespace WorldMachineLoader.API.UI
{
    /// <summary>
    /// Abstract mod window providing a base implementation for custom windows with control and theme support.
    /// </summary>
    public abstract class ModWindow : TWMWindow
    {
        private readonly List<Control> _controls = new List<Control>();

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

            foreach (var control in _controls)
                control.Draw(theme, screenPos, alpha);
            OnDraw(theme, screenPos, alpha);
        }

        /// <summary>
        /// Updates the state of all controls in the window.
        /// </summary>
        /// <param name="mouseInputWasConsumed">Whether mouse input was already consumed.</param>
        /// <returns>True if the window updated successfully; otherwise, false.</returns>
        public sealed override bool Update(bool mouseInputWasConsumed)
        {
            foreach (var control in _controls)
                if (!IsModalWindowOpen())
                {
                    bool canInteract = !mouseInputWasConsumed && !IsMinimized;
                    control.Update(Pos, canInteract);
                }
            OnUpdate();
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
    }
}
