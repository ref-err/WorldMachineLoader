using OneShotMG;
using OneShotMG.src.TWM;
using OneShotMG.src.Util;
using OneShotMG.src.EngineSpecificCode;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace WorldMachineLoader.API.UI.Controls
{
    /// <summary>
    /// Single-line text input control.
    /// </summary>
    public class InputBox : Control
    {
        /// <summary>
        /// The current text value of the input box.
        /// </summary>
        public string Text
        {
            get => _text.ToString();
            set
            {
                _text.Clear();
                _text.Append(value);
            }
        }

        /// <summary>
        /// Placeholder text shown when the value is empty and not focused.
        /// </summary>
        public string Placeholder { get; set; } = "Enter text...";

        /// <summary>
        /// Width in pixels of the input box.
        /// </summary>
        public int Width { get; set; } = 100;

        /// <summary>
        /// Maximum number of characters allowed in the input.
        /// </summary>
        public int Limit { get; set; } = 30;

        /// <summary>
        /// Whether the input box currently has keyboard focus.
        /// </summary>
        public bool IsFocused { get; set; } = false;

        private KeyboardState prevState;
        private StringBuilder _text = new StringBuilder();
        private Rect bounds;

        /// <summary>
        /// Create an InputBox at position with the specified width.
        /// </summary>
        /// <param name="position">Position of the control in the window.</param>
        /// <param name="width">Width in pixels for the input box.</param>
        public InputBox(Vec2 position, int width) : base(position)
        {
            Width = width;
            bounds = new Rect(Position.X + 2, Position.Y + 26, Width, 18);
        }

        /// <summary>
        /// Create an InputBox at position with the specified width and character limit.
        /// </summary>
        /// <param name="position">Position of the control in the window.</param>
        /// <param name="width">Width in pixels for the input box.</param>
        /// <param name="limit">Maximum characters allowed.</param>
        public InputBox(Vec2 position, int width, int limit) : this(position, width)
        {
            Limit = limit;
        }

        /// <summary>
        /// Create an InputBox at position with the specified width, character limit and placeholder text.
        /// </summary>
        /// <param name="position">Position of the control in the window.</param>
        /// <param name="width">Width in pixels for the input box.</param>
        /// <param name="limit">Maximum characters allowed.</param>
        /// <param name="placeholder">Placeholder string to show when empty.</param>
        public InputBox(Vec2 position, int width, int limit, string placeholder) : this(position, width, limit)
        {
            Placeholder = placeholder;
        }

        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            GameColor bgColor = theme.Background();
            GameColor fgColor = theme.Primary();

            Rect outerRect = new Rect(Position.X + screenPos.X, Position.Y + screenPos.Y, Width + 2, 18);
            Rect innerRect = new Rect(Position.X + screenPos.X + 1, Position.Y + screenPos.Y + 1, Width, 16);

            Game1.gMan.ColorBoxBlit(outerRect, fgColor);
            Game1.gMan.ColorBoxBlit(innerRect, bgColor);

            GameColor placeholderColor = fgColor;
            placeholderColor.a = 127;

            if (string.IsNullOrEmpty(Text) && IsFocused)
                Game1.gMan.TextBlit(GraphicsManager.FontType.OS, new Vec2(Position.X + screenPos.X + 4, Position.Y + screenPos.Y), "_", fgColor);
            else if (string.IsNullOrEmpty(Text))
                Game1.gMan.TextBlit(GraphicsManager.FontType.OS, new Vec2(Position.X + screenPos.X + 4, Position.Y + screenPos.Y), Placeholder, placeholderColor);
            else
            {
                string textToDisplay;

                if (IsFocused)
                    textToDisplay = _text.Length >= Limit ? Text : Text + "_";
                else
                    textToDisplay = Text;
                Game1.gMan.TextBlit(GraphicsManager.FontType.OS, new Vec2(Position.X + screenPos.X + 4, Position.Y + screenPos.Y), textToDisplay, fgColor);
            }
        }

        public override void Update(Vec2 parentPos, bool canInteract)
        {
            if (canInteract)
            {
                Vec2 v = Game1.mouseCursorMan.MousePos - parentPos;
                bool hovering = bounds.IsVec2InRect(v);
                if (hovering && IsFocused)
                    Game1.mouseCursorMan.SetState(OneShotMG.src.MouseCursorManager.State.Normal);
                else if (hovering)
                    Game1.mouseCursorMan.SetState(OneShotMG.src.MouseCursorManager.State.Clickable);

                if (hovering && Game1.mouseCursorMan.MouseClicked)
                    IsFocused = true;
            }
            UpdateInput();
        }

        private void UpdateInput()
        {
            KeyboardState keyState = Keyboard.GetState();

            if (!IsFocused)
            {
                prevState = keyState;
                return;
            }

            foreach (var key in keyState.GetPressedKeys())
            {
                bool wasDown = prevState.IsKeyDown(key);

                if (!wasDown)
                {
                    if (key == Keys.Back && _text.Length > 0)
                    {
                        _text.Remove(_text.Length - 1, 1);
                    }
                    else if (key == Keys.Space && _text.Length < Limit)
                    {
                        _text.Append(' ');
                    }
                    else if (key == Keys.Enter || key == Keys.Escape)
                    {
                        IsFocused = false;
                    }
                    else
                    {
                        char? c = KeyToChar(key, keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift), keyState.CapsLock);
                        if (c.HasValue && _text.Length < Limit)
                            _text.Append(c.Value);
                    }
                }
            }

            prevState = keyState;
        }

        private char? KeyToChar(Keys key, bool shift, bool capsLock)
        {
            if (key >= Keys.A && key <= Keys.Z)
            {
                char c = (char)('a' + (key - Keys.A));
                if (capsLock)
                    return shift ? c : char.ToUpper(c);
                else
                    return shift ? char.ToUpper(c) : c;
            }
            else if (key >= Keys.D0 && key <= Keys.D9)
            {
                string normal = "0123456789";
                string shifted = ")!@#$%^&*(";
                int i = key - Keys.D0;
                return shift ? shifted[i] : normal[i];
            }
            else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
            {
                char c = (char)('0' + (key - Keys.NumPad0));
                return c;
            }

                switch (key)
                {
                    case Keys.OemComma: return shift ? '<' : ',';
                    case Keys.OemPeriod: return shift ? '>' : '.';
                    case Keys.OemMinus: return shift ? '_' : '-';
                    case Keys.OemPlus: return shift ? '+' : '=';
                    case Keys.OemQuestion: return shift ? '?' : '/';
                    case Keys.OemSemicolon: return shift ? ':' : ';';
                    case Keys.OemQuotes: return shift ? '"' : '\'';
                    case Keys.OemOpenBrackets: return shift ? '{' : '[';
                    case Keys.OemCloseBrackets: return shift ? '}' : ']';
                    case Keys.OemBackslash: return shift ? '|' : '\\';
                }

            return null;
        }
    }
}
