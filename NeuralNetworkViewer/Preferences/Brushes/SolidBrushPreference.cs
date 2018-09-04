using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Preferences.Brushes
{
    public class SolidBrushPreference : BrushPreference
    {
        private readonly Color _color;

        public SolidBrushPreference(Color color)
        {
            this._color = color;
        }

        internal override Brush CreateBrush()
        {
            return new SolidBrush(_color);
        }
    }
}
