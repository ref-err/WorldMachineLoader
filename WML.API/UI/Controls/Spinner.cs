using OneShotMG;
using OneShotMG.src.TWM;
using OneShotMG.src.EngineSpecificCode;

namespace WorldMachineLoader.API.UI.Controls
{
    public class Spinner : Control
    {
        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public int Value { get; set; }

        private Label _label;

        private ButtonIcon _decreaseButton;
        private ButtonIcon _increaseButton;

        private Vec2 _valueTextSize;

        public Spinner(int minValue, int maxValue, int defaultValue, Vec2 position) : base(position)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Value = defaultValue;
            _label = new Label(Value.ToString(), position + new Vec2(18, 3));
            _decreaseButton = new ButtonIcon("the_world_machine/window_icons/arrow_left", new Vec2(16, 16), Position + new Vec2(0, 0), Decrease);
            _increaseButton = new ButtonIcon("the_world_machine/window_icons/arrow_right", new Vec2(16, 16), Position + new Vec2(32, 0), Increase);
        }

        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            _valueTextSize = Game1.gMan.TextSize(GraphicsManager.FontType.OS, Value.ToString());

            _decreaseButton.Draw(theme, screenPos, alpha);
            _increaseButton.Draw(theme, screenPos + new Vec2(_valueTextSize.X - 6, 0), alpha);

            Rect rect1 = new Rect(Position.X + screenPos.X + 16, Position.Y + screenPos.Y, _valueTextSize.X + 10, 16);
            Rect rect2 = new Rect(Position.X + screenPos.X + 17, Position.Y + screenPos.Y + 1, _valueTextSize.X + 8, 14);

            Game1.gMan.ColorBoxBlit(rect1, theme.Primary());
            Game1.gMan.ColorBoxBlit(rect2, theme.Background());

            _label.Draw(theme, screenPos + new Vec2(1, 0), alpha);
        }

        public override void Update(Vec2 parentPos, bool canInteract)
        {
            _label.Update(parentPos, canInteract);
            _decreaseButton.Update(parentPos, canInteract);
            _increaseButton.Update(parentPos + new Vec2(_valueTextSize.X - 6, 0), canInteract);
        }

        private void Increase()
        {
            if (Value < MaxValue)
                Value++;

            _label.Text = Value.ToString();
        }

        private void Decrease()
        {
            if (Value > MinValue)
                Value--;

            _label.Text = Value.ToString();
        }
    }
}
