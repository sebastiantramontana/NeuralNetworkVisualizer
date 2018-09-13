using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Drawing.Nodes;
using NeuralNetworkVisualizer.Model.Layers;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;

namespace NeuralNetworkVisualizer.Drawing.Layers
{
    internal class InputLayerDrawing : LayerBaseDrawing<InputLayer, Input>
    {
        private readonly Preference _preference;
        private readonly SimpleNodeSizesPreCalc _simpleNodeCache;

        internal InputLayerDrawing(InputLayer layer, Preference preferences, LayerSizesPreCalc cache, SimpleNodeSizesPreCalc simpleNodeCache) : base(layer, preferences, cache, simpleNodeCache)
        {
            _preference = preferences;
            _simpleNodeCache = simpleNodeCache;
        }

        protected override INodeDrawing CreateDrawingNode(Input node)
        {
            return new InputDrawing(node, _preference, _simpleNodeCache);
        }
    }
}
