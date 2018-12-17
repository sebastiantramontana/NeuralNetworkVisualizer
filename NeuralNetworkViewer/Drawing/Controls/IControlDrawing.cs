using NeuralNetworkVisualizer.Model.Layers;
using System.Drawing;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Drawing.Controls
{
    internal interface IControlDrawing
    {
        Task RedrawAsync();
        void Redraw();
        Image GetImage();
    }
}
