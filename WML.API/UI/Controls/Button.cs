using OneShotMG;
using OneShotMG.src.TWM;
using System;

namespace WorldMachineLoader.API.UI.Controls
{
    /// <summary>
    /// Represents a button control wrapped around <see cref="TextButton"/> for use in the UI.
    /// </summary>
    public class Button : Control
    {
        private TextButton button;

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class with the specified text, position, action, and dimensions.
        /// </summary>
        /// <param name="text">The text displayed on the button.</param>
        /// <param name="position">The position of the button in the window.</param>
        /// <param name="buttonAction">The action performed when the button is clicked. </param>
        /// <param name="width">Button width (default 56).</param>
        /// <param name="height">Button height (default 16).</param>
        public Button(string text, Vec2 position, Action buttonAction, int width = 56, int height = 16) : base(position)
        {
            button = new TextButton(text, position, delegate { buttonAction(); }, width, height);
        }

        /// <summary>
        /// Draws the button using the given theme, screen position and transparency.
        /// </summary>
        /// <param name="theme">The theme to draw.</param>
        /// <param name="screenPos">Screen Position.</param>
        /// <param name="alpha">Transparency value (0-255).</param>
        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            button.Draw(screenPos, theme, alpha);
        }

        /// <summary>
        /// Updates the state of the button, given the position of the parent and whether the button can be interacted with.
        /// </summary>
        /// <param name="parentPos">Position of the parent element.</param>
        /// <param name="canInteract">Flag indicating whether the button can be interacted with.</param>
        public override void Update(Vec2 parentPos, bool canInteract)
        {
            button.Update(new Vec2(parentPos.X + 2, parentPos.Y + 26), canInteract);
        }
    }
}
