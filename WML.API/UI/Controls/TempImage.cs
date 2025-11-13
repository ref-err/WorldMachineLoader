using OneShotMG;
using OneShotMG.src.EngineSpecificCode;
using OneShotMG.src.TWM;

namespace WorldMachineLoader.API.UI.Controls
{
    /// <summary>
    /// A control that displays a <see cref="TempTexture"/>.
    /// </summary>
    public class TempImage : Control
    {
        /// <summary>
        /// THe temporary texture to render. May be <c>null</c>.
        /// </summary>
        public TempTexture Texture;

        /// <summary>
        /// Create a TempImage at the given position with the provided <see cref="TempTexture"/>.
        /// </summary>
        /// <param name="texture">Texture to display.</param>
        /// <param name="position">Position of the control in the window.</param>
        public TempImage(TempTexture texture, Vec2 position) : base(position)
        {
            Texture = texture;
        }

        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            if (Texture != null)
            {
                Game1.gMan.MainBlit(Texture, (Position + screenPos) * 2, theme.Primary(), 0, GraphicsManager.BlendMode.Normal, 1, false);
            }
        }

        public override void Update(Vec2 parentPos, bool canInteract)
        {
            Texture?.KeepAlive();
        }

        /// <summary>
        /// Replace the current texture with a new one, disposing the previous render target if present.
        /// </summary>
        /// <param name="texture">New texture to use.</param>
        public void SetTexture(TempTexture texture)
        {
            Dispose();
            Texture = texture;
        }

        /// <summary>
        /// Dispose of the held <see cref="TempTexture"/>'s render target (if any) and clear the reference.
        /// </summary>
        public void Dispose()
        {
            Texture?.renderTarget?.Dispose();
            Texture = null;
        }
    }
}
