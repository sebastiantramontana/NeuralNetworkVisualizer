using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Drawing.Nodes;
using NeuralNetworkVisualizer.Model.Layers;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;
using NeuralNetworkVisualizer.Selection;
using System.Collections.Generic;

namespace NeuralNetworkVisualizer.Drawing.Layers
{
    internal class PerceptronLayerDrawing : LayerBaseDrawing<PerceptronLayer, Perceptron>
    {
        private readonly IDictionary<NodeBase, INodeDrawing> _previousNodes;
        private readonly ICanvas _edgesCanvas;
        private readonly Preference _preferences;
        private readonly PerceptronSizesPreCalc _perceptronCache;
        private readonly EdgeSizesPreCalc _edgesCache;
        private readonly IElementSelectionChecker _selectionChecker;
        private readonly ISelectableElementRegister _selectableElementRegister;

        internal PerceptronLayerDrawing(PerceptronLayer layer, IDictionary<NodeBase, INodeDrawing> previousNodes, ICanvas edgesCanvas, Preference preferences, LayerSizesPreCalc cache, PerceptronSizesPreCalc perceptronCache, SimpleNodeSizesPreCalc biasCache, EdgeSizesPreCalc edgesCache, IElementSelectionChecker selectionChecker, ISelectableElementRegister selectableElementRegister) : base(layer, preferences, cache, biasCache, selectionChecker, selectableElementRegister)
        {
            _previousNodes = previousNodes;
            _edgesCanvas = edgesCanvas;
            _preferences = preferences;
            _perceptronCache = perceptronCache;
            _edgesCache = edgesCache;
            this._selectionChecker = selectionChecker;
            this._selectableElementRegister = selectableElementRegister;
        }

        protected override INodeDrawing CreateDrawingNode(Perceptron node)
        {
            return new PerceptronDrawing(node, _previousNodes, _edgesCanvas, _preferences, _perceptronCache, _edgesCache, _selectionChecker, _selectableElementRegister);
        }
    }
}
