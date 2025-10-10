using OneShotMG;
using OneShotMG.src.EngineSpecificCode;
using OneShotMG.src.TWM;

namespace WorldMachineLoader.API.UI.Controls
{
    public class TempImage : Control
    {
        public TempTexture Texture;

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

        public void SetTexture(TempTexture texture)
        {
            Dispose();
            Texture = texture;
        }

        public void Dispose()
        {
            Texture?.renderTarget?.Dispose();
            Texture = null;
        }
    }
}
