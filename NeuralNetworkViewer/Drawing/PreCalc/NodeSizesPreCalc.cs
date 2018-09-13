using System.Drawing;

namespace NeuralNetworkVisualizer.Drawing.Cache
{
    internal abstract class NodeSizesPreCalc
    {
        internal Rectangle? EllipseRectangle { get; set; }
        internal Size? OutputSize { get; set; }
    }
}
