using OneShotMG;
using OneShotMG.src.TWM;
using OneShotMG.src.Util;
using OneShotMG.src.EngineSpecificCode;

namespace WorldMachineLoader.API.UI.Controls
{
    public class CheckBox : Control
    {
        public bool IsChecked { get; set; } = false;

        private string Text { get; set; }

        private Rect bounds;

        public CheckBox(string text, Vec2 position) : base(position)
        {
            Text = text;
            bounds = new Rect(Position.X + 2, Position.Y + 26, 16, 16);
        }

        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            GameColor bgColor = theme.Background();
            GameColor fgColor = theme.Primary();

            Rect outerRect = new Rect(Position.X + screenPos.X, Position.Y + screenPos.Y, 16, 16);
            Rect innerRect = new Rect(Position.X + screenPos.X + 1, Position.Y + screenPos.Y + 1, 14, 14);

            Game1.gMan.ColorBoxBlit(outerRect, fgColor);
            Game1.gMan.ColorBoxBlit(innerRect, bgColor);

            Game1.gMan.TextBlit(GraphicsManager.FontType.OS, (new Vec2(21, -1) + Position) + screenPos, Text, fgColor);

            if (IsChecked)
                Game1.gMan.MainBlit("the_world_machine/window_buttons", Position + screenPos, new Rect(32, 0, 16, 16), fgColor, 0, GraphicsManager.BlendMode.Normal, 2);
        }

        public override void Update(Vec2 parentPos, bool canInteract)
        {
            if (canInteract)
            {
                Vec2 v = Game1.mouseCursorMan.MousePos - parentPos; bool hovering = bounds.IsVec2InRect(v);
                if (hovering)
                    Game1.mouseCursorMan.SetState(OneShotMG.src.MouseCursorManager.State.Clickable);

                if (hovering && Game1.mouseCursorMan.MouseClicked)
                    IsChecked = !IsChecked;
            }
        }
    }
}
