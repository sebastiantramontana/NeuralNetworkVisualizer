using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Drawing.Nodes;
using NeuralNetworkVisualizer.Model.Layers;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;

namespace NeuralNetworkVisualizer.Drawing.Layers
{
    internal class InputLayerDrawing : LayerBaseDrawing<InputLayer, Input>
    {
        private readonly Preference _preference;
        private readonly SimpleNodeSizesCache _cache;

        internal InputLayerDrawing(InputLayer layer, Preference preferences, SimpleNodeSizesCache cache) : base(layer, layer.GetMaxNodesInLayer(), preferences, cache)
        {
            _preference = preferences;
            _cache = cache;
        }

        protected override INodeDrawing CreateDrawingNode(Input node)
        {
            return new InputDrawing(node, _preference, _cache);
        }
    }
}
