using OneShotMG;
using OneShotMG.src.TWM;
using OneShotMG.src.EngineSpecificCode;
using Microsoft.Xna.Framework.Graphics;

namespace WorldMachineLoader.API.UI.Controls
{
    public class Image : Control
    {
        public Texture2D Texture { get; private set; }

        public Image(Texture2D texture, Vec2 position) : base(position)
        {
            Texture = texture;
        }

        public void SetTexture(Texture2D texture)
        {
            Texture.Dispose();
            Texture = texture;
        }

        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            if (Texture != null)
            {
                var srcRect = new Rect(0, 0, Texture.Width, Texture.Height);
                Game1.gMan.MainBlit(Texture, (Position + screenPos) * 2, srcRect, 1f, 1f, alpha / 255f, 0, GraphicsManager.BlendMode.Normal, default, 1f, 1f, 1f, 0);
            }
        }

        public override void Update(Vec2 parentPos, bool canInteract) { }
    }
}
