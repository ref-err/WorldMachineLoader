using OneShotMG;
using OneShotMG.src.TWM;
using System;

namespace WorldMachineLoader.API.UI.Controls
{
    public class Chooser : Control
    {
        public string[] Items { get; set; }

        private ButtonIcon _backButton;
        private ButtonIcon _forwardButton;
        private Label _currentItemLabel;

        private Vec2 _itemTextSize;

        private int _index = 0;

        public Chooser(string[] items, int defaultItemIndex, Vec2 position) : base(position)
        {
            Items = items;
            _index = defaultItemIndex;
            _backButton = new ButtonIcon("the_world_machine/window_icons/arrow_left", new Vec2(16, 16), position, Back);
            _forwardButton = new ButtonIcon("the_world_machine/window_icons/arrow_right", new Vec2(16, 16), position + new Vec2(32, 0), Forward);
            _currentItemLabel = new Label(Items[_index], position + new Vec2(20, 3));
        }

        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            _itemTextSize = Game1.gMan.TextSize(OneShotMG.src.EngineSpecificCode.GraphicsManager.FontType.OS, Items[_index]);

            _backButton.Draw(theme, screenPos, alpha);
            _forwardButton.Draw(theme, screenPos + new Vec2(_itemTextSize.X - 6, 0), alpha);

            Rect rect1 = new Rect(Position.X + screenPos.X + 16, Position.Y + screenPos.Y, _itemTextSize.X + 10, 16);
            Rect rect2 = new Rect(Position.X + screenPos.X + 17, Position.Y + screenPos.Y + 1, _itemTextSize.X + 8, 14);

            Game1.gMan.ColorBoxBlit(rect1, theme.Primary());
            Game1.gMan.ColorBoxBlit(rect2, theme.Background());

            _currentItemLabel.Draw(theme, screenPos, alpha);
        }

        public override void Update(Vec2 parentPos, bool canInteract)
        {
            _backButton.Update(parentPos, canInteract);
            _forwardButton.Update(parentPos + new Vec2(_itemTextSize.X - 6, 0), canInteract);
            _currentItemLabel.Update(parentPos, canInteract);
        }

        private void Back()
        {
            if (_index - 1 < 0)
            {
                _index = Items.Length - 1;
            }
            else
            {
                _index--;
            }

            _currentItemLabel.Text = Items[_index];
        }

        private void Forward()
        {
            if (_index + 1 > Items.Length - 1)
            {
                _index = 0;
            }
            else
            {
                _index++;
            }

            _currentItemLabel.Text = Items[_index];
        }
    }
}
