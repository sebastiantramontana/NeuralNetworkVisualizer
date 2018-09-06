using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Preferences.Brushes
{
    public interface IBrushPreference
    {
        Brush CreateBrush();
    }
}
