using OneShotMG;
using OneShotMG.src.TWM;

namespace WorldMachineLoader.API.UI.Controls
{
    public abstract class Control
    {
        public Vec2 Position { get; set; }
        public Vec2 Size { get; protected set; }

        protected Control(Vec2 position)
        {
            Position = position;
        }

        public abstract void Draw(TWMTheme theme, Vec2 pos, byte alpha);

        public abstract void Update();
    }
}
