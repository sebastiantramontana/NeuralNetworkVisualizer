using System.Drawing;

namespace NeuralNetworkVisualizer.Drawing.Controls
{
    internal interface ISelectionEventFiring
    {
        void FireSelectionEvent(Point location);
    }
}
