using System.Drawing;

namespace NeuralNetworkVisualizer.Drawing.Controls
{
    internal interface IToolTipFiring
    {
        void Show(Point location);
        void Hide();
    }
}
