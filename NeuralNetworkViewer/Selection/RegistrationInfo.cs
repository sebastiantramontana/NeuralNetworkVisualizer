using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Model;
using System.Drawing;

namespace NeuralNetworkVisualizer.Selection
{
    internal class RegistrationInfo
    {
        public RegistrationInfo(Element element, ICanvas canvas, Region region, int zIndex)
        {
            this.Element = element;
            this.Canvas = canvas;
            this.Region = region;
            this.ZIndex = zIndex;
        }

        public Element Element { get; }
        public ICanvas Canvas { get; }
        public Region Region { get; }
        public int ZIndex { get; }
    }
}
