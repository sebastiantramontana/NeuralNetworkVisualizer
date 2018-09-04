using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Drawing.Nodes;
using NeuralNetworkVisualizer.Model.Layers;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;
using System.Collections.Generic;

namespace NeuralNetworkVisualizer.Drawing.Layers
{
    internal class PerceptronLayerDrawing : LayerBaseDrawing<PerceptronLayer, Perceptron>
    {
        private readonly IDictionary<NodeBase, INodeDrawing> _previousNodes;
        private readonly ICanvas _edgesCanvas;
        private readonly Preference _preferences;
        private readonly PerceptronSizesCache _perceptronCache;
        private readonly EdgeSizesCache _edgesCache;

        internal PerceptronLayerDrawing(PerceptronLayer layer, IDictionary<NodeBase, INodeDrawing> previousNodes, ICanvas edgesCanvas, int maxNodes, Preference preferences, PerceptronSizesCache perceptronCache, SimpleNodeSizesCache biasCache, EdgeSizesCache edgesCache) : base(layer, maxNodes, preferences, biasCache)
        {
            _previousNodes = previousNodes;
            _edgesCanvas = edgesCanvas;
            _preferences = preferences;
            _perceptronCache = perceptronCache;
            _edgesCache = edgesCache;
        }

        protected override INodeDrawing CreateDrawingNode(Perceptron node)
        {
            return new PerceptronDrawing(node, _previousNodes, _edgesCanvas, _preferences, _perceptronCache, _edgesCache);
        }
    }
}
