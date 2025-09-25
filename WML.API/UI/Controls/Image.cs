using OneShotMG;
using OneShotMG.src.TWM;
using OneShotMG.src.EngineSpecificCode;
using Microsoft.Xna.Framework.Graphics;

namespace WorldMachineLoader.API.UI.Controls
{
    /// <summary>
    /// UI element for displaying an image using <see cref="Texture2D"/>.
    /// </summary>
    public class Image : Control
    {
        /// <summary>
        /// The texture displayed by this element.
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Creates a new <see cref="Image"/> instance with the specified texture and position.
        /// </summary>
        /// <param name="texture">The texture to display.</param>
        /// <param name="position">The position of the element in the window.</param>
        public Image(Texture2D texture, Vec2 position) : base(position)
        {
            Texture = texture;
        }

        /// <summary>
        /// Sets a new texture for the image, disposing the previous one.
        /// </summary>
        /// <param name="texture">The new texture.</param>
        public void SetTexture(Texture2D texture)
        {
            Texture.Dispose();
            Texture = texture;
        }

        /// <summary>
        /// Draws the image on the screen, considering theme, position, and transparency.
        /// </summary>
        /// <param name="theme">The UI theme.</param>
        /// <param name="screenPos">The position on the screen.</param>
        /// <param name="alpha">Transparency value (0-255).</param>
        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            if (Texture != null)
            {
                var srcRect = new Rect(0, 0, Texture.Width, Texture.Height);
                Game1.gMan.MainBlit(Texture, (Position + screenPos) * 2, srcRect, 1f, 1f, alpha / 255f, 0, GraphicsManager.BlendMode.Normal, default, 1f, 1f, 1f, 0);
            }
        }

        /// <summary>
        /// Updates the state of the element. No update is required for <see cref="Image"/>.
        /// </summary>
        /// <param name="parentPos">The position of the parent element.</param>
        /// <param name="canInteract">Whether interaction is possible.</param>
        public override void Update(Vec2 parentPos, bool canInteract) { }
    }
}
