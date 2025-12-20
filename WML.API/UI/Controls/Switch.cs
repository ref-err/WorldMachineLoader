using OneShotMG;
using OneShotMG.src.TWM;
using System;

namespace WorldMachineLoader.API.UI.Controls
{
    public class Switch : Control
    {
        public bool Value { get; set; } = false;

        private Button _button;

        public Switch(Vec2 position) : base(position)
        {
            _button = new Button(" ", position, () =>
            {
                Value = !Value;
                Console.WriteLine(Value.ToString());
            }, 16, 16);
        }

        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            Rect rect = new Rect(Position.X + screenPos.X, Position.Y + screenPos.Y + 6, 34, 4);
            Rect rect2 = new Rect(Position.X + screenPos.X + 1, Position.Y + screenPos.Y + 7, 32, 2);

            Game1.gMan.ColorBoxBlit(rect, theme.Primary());
            Game1.gMan.ColorBoxBlit(rect2, theme.Background());

            var pos = Value ? screenPos + new Vec2(18, 0) : screenPos;
            _button.Draw(theme, pos, alpha);
        }

        public override void Update(Vec2 parentPos, bool canInteract)
        {
            var pos = Value ? parentPos + new Vec2(18, 0) : parentPos;
            _button.Update(pos, canInteract);
        }
    }
}
