using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Preferences.Brushes
{
    public abstract class BrushPreference
    {
        internal abstract Brush CreateBrush();
    }
}
