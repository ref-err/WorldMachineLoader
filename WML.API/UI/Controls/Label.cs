using OneShotMG;
using OneShotMG.src.EngineSpecificCode;
using OneShotMG.src.TWM;
using OneShotMG.src.Util;

namespace WorldMachineLoader.API.UI.Controls
{
    public class Label : Control
    {
        public string Text { get; set; }

        public Label(string text, Vec2 position) : base(position)
        {
            Text = text;
            Position = new Vec2(Position.X + 2, Position.Y - 4);
        }

        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            GameColor color = theme.Primary(alpha);

            Game1.gMan.TextBlit(GraphicsManager.FontType.OS, Position + screenPos, Text, color);
        }

        public override void Update(Vec2 pos, bool canInteract) { }
    }
}
