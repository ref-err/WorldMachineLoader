using System;
using OneShotMG;
using OneShotMG.src.TWM;

namespace WorldMachineLoader.API.UI.Controls
{
    public class ButtonIcon : Control
    {
        public string Icon
        {
            get => _iconButton.Icon;
            set => _iconButton.Icon = value;
        }

        private IconButton _iconButton;

        public ButtonIcon(string iconPath, Vec2 size, Vec2 position, Action action) : base(position)
        {
            _iconButton = new IconButton(iconPath, size, position, delegate { action(); }, OneShotMG.src.EngineSpecificCode.TextureCache.CacheType.TheWorldMachine);
        }

        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            _iconButton.Draw(screenPos, theme, alpha);
        }

        public override void Update(Vec2 parentPos, bool canInteract)
        {
            _iconButton.Update(new Vec2(parentPos.X + 2, parentPos.Y + 26), canInteract);
        }
    }
}
