using OneShotMG;
using OneShotMG.src.EngineSpecificCode;
using OneShotMG.src.TWM;
using OneShotMG.src.Util;

namespace WorldMachineLoader.API.UI.Controls
{
    public enum FontType
    {
        OS,
        Terminus
    }

    /// <summary>
    /// Represents a control for displaying text on the screen.
    /// </summary>
    public class Label : Control
    {
        /// <summary>
        /// Gets or sets the displayed label text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Font that will be used for rendering text.
        /// </summary>
        public FontType Font { get; set; } = FontType.OS;

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class with the given text and position.
        /// </summary>
        /// <param name="text">The text to be displayed.</param>
        /// <param name="position">The position of the label in the window.</param>
        public Label(string text, Vec2 position) : base(position)
        {
            Text = text;
            Position = new Vec2(Position.X + 2, Position.Y - 4);
        }

        /// <summary>
        /// Draws the label using the given theme, screen position, and transparency.
        /// </summary>
        /// <param name="theme">The theme for the text color.</param>
        /// <param name="screenPos">The position on the screen relative to the parent.</param>
        /// <param name="alpha">Transparency value (0-255).</param>
        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            GameColor color = theme.Primary(alpha);

            switch (Font)
            {
                case FontType.OS:
                    Game1.gMan.TextBlit(GraphicsManager.FontType.OS, Position + screenPos, Text, color);
                    break;
                case FontType.Terminus:
                    Game1.gMan.TextBlit(GraphicsManager.FontType.Game, (Position + screenPos) * 2, Text, color, scale: 1);
                    break;
            }
        }

        /// <summary>
        /// Updates the state of the label. The method is not implemented for <see cref="Label"/>.
        /// </summary>
        /// <param name="pos">Position of parent element.</param>
        /// <param name="canInteract">Ability to interact with element.</param>
        public override void Update(Vec2 pos, bool canInteract) { }
    }
}
