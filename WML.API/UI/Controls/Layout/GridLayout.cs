using OneShotMG;
using OneShotMG.src.TWM;
using System.Collections.Generic;

namespace WorldMachineLoader.API.UI.Controls.Layout
{
    public class GridLayout : Control
    {
        public int Gap { get; set; }

        public Dictionary<Control, Vec2> Controls { get; } = new Dictionary<Control, Vec2>();

        public GridLayout(Vec2 position, int gap) : base(position)
        {
            Gap = gap;
        }

        public override void Draw(TWMTheme theme, Vec2 screenPos, byte alpha)
        {
            foreach (var control in Controls)
            {
                control.Key.Draw(theme, GetPos(control.Key.Size, control.Value) + screenPos, alpha);
            }
        }

        public override void Update(Vec2 parentPos, bool canInteract)
        {
            foreach (var control in Controls)
            {
                control.Key.Update(GetPos(control.Key.Size, control.Value) + parentPos, canInteract);
            }
        }

        public void Add(Control control, Vec2 gridPosition)
        {
            if (!Controls.ContainsKey(control))
            {
                Controls.Add(control, gridPosition);
            }
        }

        public Vec2 GetPos(Vec2 controlSize, Vec2 gridPosition)
        {
            return new Vec2(
                Position.X + gridPosition.X * (controlSize.X + Gap),
                Position.Y + gridPosition.Y * (controlSize.Y + Gap)
            );
        }
    }
}
