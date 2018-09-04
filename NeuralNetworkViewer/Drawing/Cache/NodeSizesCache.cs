using System.Drawing;

namespace NeuralNetworkVisualizer.Drawing.Cache
{
    internal abstract class NodeSizesCache
    {
        internal Rectangle? EllipseRectangle { get; set; }
        internal Size? OutputSize { get; set; }
    }
}
