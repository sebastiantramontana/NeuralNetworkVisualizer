using NeuralNetworkVisualizer.Drawing.Nodes;
using System.Collections.Generic;

namespace NeuralNetworkVisualizer.Drawing.Layers
{
    internal interface ILayerDrawing : IDrawing
    {
        IEnumerable<INodeDrawing> NodesDrawing { get; }
    }
}
