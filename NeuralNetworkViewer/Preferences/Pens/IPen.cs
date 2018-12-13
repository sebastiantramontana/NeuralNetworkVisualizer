using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences.Pens
{
    public interface IPen : IDisposable
    {
        Pen CreatePen();
    }
}
