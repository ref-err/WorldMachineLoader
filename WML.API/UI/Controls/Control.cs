using OneShotMG;
using OneShotMG.src.TWM;

namespace WorldMachineLoader.API.UI.Controls
{
    /// <summary>
    /// Basic abstract class for UI controls.
    /// Defines position, size, and basic rendering and update methods.
    /// </summary>
    public abstract class Control
    {
        /// <summary>
        /// Control's position relative to its parent.
        /// </summary>
        public Vec2 Position { get; set; }

        /// <summary>
        /// Control size.
        /// </summary>
        public Vec2 Size { get; set; }

        /// <summary>
        /// Initializes a new instance of class <see cref="Control"/> with the given position.
        /// </summary>
        /// <param name="position">Control position.</param>
        protected Control(Vec2 position)
        {
            Position = position;
        }

        /// <summary>
        /// Draws a control using the specified theme, window position, and transparency.
        /// </summary>
        /// <param name="theme">Theme.</param>
        /// <param name="screenPos">Position in the window.</param>
        /// <param name="alpha">Transparency value (0-255).</param>
        public abstract void Draw(TWMTheme theme, Vec2 screenPos, byte alpha);

        /// <summary>
        /// Updates the state of the control.
        /// </summary>
        /// <param name="parentPos">Position of the parent element.</param>
        /// <param name="canInteract">Flag indicating whether the control can be interacted with.</param>
        public abstract void Update(Vec2 parentPos, bool canInteract);
    }
}
