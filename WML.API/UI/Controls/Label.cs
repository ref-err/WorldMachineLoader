using OneShotMG;
using OneShotMG.src.EngineSpecificCode;
using OneShotMG.src.TWM;
using OneShotMG.src.Util;

namespace WorldMachineLoader.API.UI.Controls
{
    public class Label : Control
    {
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    _dirty = true;
                }
            }
        }
        public GameColor Color { get; set; }
        private TempTexture _texture;
        private string _text;
        private bool _dirty = true;

        public Label(string text, Vec2 position) : base(position)
        {
            Text = text;
        }

        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            Color = theme.Primary(alpha);

            Game1.gMan.MainBlit(_texture, (Position + screenPos) * 2,  Color, 0, GraphicsManager.BlendMode.Normal, 1, xCentered: false);
        }

        public override void Update()
        {
            if (_dirty || _texture == null || !_texture.isValid)
            {
                _texture = Game1.gMan.TempTexMan.GetSingleLineTexture(GraphicsManager.FontType.OS, _text);
                _dirty = false;
            }

            _texture.KeepAlive();
        }
    }
}
