using OneShotMG;
using OneShotMG.src.TWM;
using OneShotMG.src.Util;
using System.Collections.Generic;
using WorldMachineLoader.API.UI.Controls;

namespace WorldMachineLoader.API.UI
{
    public abstract class ModWindow : TWMWindow
    {
        private readonly List<Control> _controls = new List<Control>();

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

        protected virtual void OnDraw(TWMTheme theme, Vec2 pos, byte alpha) { }

        protected virtual void OnUpdate() { }

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

        public sealed override bool Update(bool mouseInputWasConsumed)
        {
            foreach (var control in _controls)
                control.Update();
            OnUpdate();
            return base.Update(mouseInputWasConsumed);
        }

        public sealed override bool IsSameContent(TWMWindow window)
        {
            return window.GetType() == GetType();
        }

        protected void AddControl(Control control)
        {
            _controls.Add(control);
        }
    }
}
