using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences.Brushes
{
    public class SolidBrushPreference : IBrushPreference
    {
        private readonly Color _color;

        public SolidBrushPreference(Color color)
        {
            this._color = color;
        }

        public Brush CreateBrush()
        {
            return new SolidBrush(_color);
        }
    }
}
