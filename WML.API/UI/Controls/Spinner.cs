using OneShotMG;
using OneShotMG.src.TWM;

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

        public Spinner(int minValue, int maxValue, int defaultValue, Vec2 position) : base(position)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Value = defaultValue;
            _label = new Label(Value.ToString(), position + new Vec2(20, 3));
            _decreaseButton = new ButtonIcon("the_world_machine/window_icons/arrow_left", new Vec2(16, 16), Position + new Vec2(0, 0), Decrease);
            _increaseButton = new ButtonIcon("the_world_machine/window_icons/arrow_right", new Vec2(16, 16), Position + new Vec2(32, 0), Increase);
        }

        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            _decreaseButton.Draw(theme, screenPos, alpha);
            _increaseButton.Draw(theme, screenPos, alpha);

            Rect rect1 = new Rect(Position.X + screenPos.X + 16, Position.Y + screenPos.Y, 16, 16);
            Rect rect2 = new Rect(Position.X + screenPos.X + 17, Position.Y + screenPos.Y + 1, 14, 14);

            Game1.gMan.ColorBoxBlit(rect1, theme.Primary());
            Game1.gMan.ColorBoxBlit(rect2, theme.Background());

            _label.Draw(theme, screenPos, alpha);
            //Game1.gMan.TextBlit(GraphicsManager.FontType.OS, new Vec2(Position.X + screenPos.X + 22, Position.Y + screenPos.Y - 1), Value.ToString(), theme.Primary(), );
        }

        public override void Update(Vec2 parentPos, bool canInteract)
        {
            _label.Update(parentPos, canInteract);
            _decreaseButton.Update(parentPos, canInteract);
            _increaseButton.Update(parentPos, canInteract);
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
