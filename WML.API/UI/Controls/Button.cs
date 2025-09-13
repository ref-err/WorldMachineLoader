using OneShotMG;
using OneShotMG.src.TWM;
using System;

namespace WorldMachineLoader.API.UI.Controls
{
    public class Button : Control
    {
        private TextButton button;

        public Button(string text, Vec2 position, Action buttonAction, int width = 56, int height = 16) : base(position)
        {
            button = new TextButton(text, position, delegate { buttonAction(); }, width, height);
        }

        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            button.Draw(screenPos, theme, alpha);
        }

        public override void Update(Vec2 parentPos, bool canInteract)
        {
            button.Update(new Vec2(parentPos.X + 2, parentPos.Y + 26), canInteract);
        }
    }
}
