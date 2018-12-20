using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Model;
using System.Drawing;

namespace NeuralNetworkVisualizer.Selection
{
    internal interface ISelectionResolver
    {
        Element GetElementFromLocation(Point location);
        void SetCurrentRootCanvas(ICanvas currentRootCanvas);
    }
}
