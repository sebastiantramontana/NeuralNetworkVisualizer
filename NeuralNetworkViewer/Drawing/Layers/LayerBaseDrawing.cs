using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Drawing.Nodes;
using NeuralNetworkVisualizer.Model.Layers;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;
using NeuralNetworkVisualizer.Preferences.Brushes;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NeuralNetworkVisualizer.Drawing.Layers
{
    internal interface ILayerDrawing : IDrawing
    {
        IEnumerable<INodeDrawing> NodesDrawing { get; }
    }

    internal abstract class LayerBaseDrawing<TLayer, TNode> : DrawingBase<TLayer>, ILayerDrawing
        where TLayer : LayerBase<TNode>
        where TNode : NodeBase
    {
        private readonly int _maxNodes;
        private readonly Preference _preferences;
        private readonly SimpleNodeSizesCache _biasCache;
        private readonly IList<INodeDrawing> _nodesDrawing;

        internal LayerBaseDrawing(TLayer layer, int maxNodes, Preference preferences, SimpleNodeSizesCache biasCache) : base(layer)
        {
            _maxNodes = maxNodes;
            _preferences = preferences;
            _biasCache = biasCache;
            _nodesDrawing = new List<INodeDrawing>(layer.GetAllNodes().Count());
        }

        public IEnumerable<INodeDrawing> NodesDrawing { get { return _nodesDrawing; } }

        public override void Draw(ICanvas canvas)
        {
            var rect = new Rectangle(0, 0, canvas.MaxWidth, canvas.MaxHeight);

            using (var brush = _preferences.Layers.Background?.CreateBrush())
            {
                canvas.DrawRectangle(rect, _preferences.Layers.Border, brush);
            }

            DrawTitle(canvas);
            DrawNodes(canvas);
        }

        private void DrawNodes(ICanvas canvas)
        {
            var nodeWidth = canvas.MaxWidth - _preferences.Margins * 2;
            var nodeHeight = ((canvas.MaxHeight - _preferences.Layers.HeightTitle) / _maxNodes) - (_preferences.Margins + _preferences.Margins / _maxNodes);
            var y = ((canvas.MaxHeight - _preferences.Layers.HeightTitle) - (nodeHeight * this.Element.GetAllNodes().Count())) / 2;
            var minimumY = _preferences.Layers.HeightTitle + _preferences.Margins;
            y = (y < minimumY ? minimumY : y);

            if (this.Element.Bias != null)
            {
                var biasDrawing = new BiasDrawing(this.Element.Bias, _preferences, _biasCache);
                _nodesDrawing.Add(biasDrawing);

                y = DrawNode(biasDrawing, canvas, y, nodeWidth, nodeHeight);
            }

            foreach (var node in this.Element.Nodes)
            {
                var nodeDrawing = CreateDrawingNode(node);
                _nodesDrawing.Add(nodeDrawing);

                y = DrawNode(nodeDrawing, canvas, y, nodeWidth, nodeHeight);
            }
        }

        private int DrawNode(INodeDrawing nodeDrawing, ICanvas parentCanvas, int y, int nodeWidth, int nodeHeight)
        {
            var newCanvas = new NestedCanvas(new Rectangle(0, y, nodeWidth, nodeHeight), parentCanvas);
            nodeDrawing.Draw(newCanvas);

            return y + nodeHeight + _preferences.Margins;
        }
        private void DrawTitle(ICanvas canvas)
        {
            var rectTitle = new Rectangle(0, 0, canvas.MaxWidth, _preferences.Layers.HeightTitle);

            //TODO: review design
            if (_preferences.Layers.BackgroundTitle is GradientBrushPreference backTitle)
            {
                backTitle.Rectangle = rectTitle;
            }

            using (var backgroundTitle = _preferences.Layers.BackgroundTitle?.CreateBrush())
            {
                canvas.DrawRectangle(rectTitle, null, backgroundTitle);
            }

            using (var brush = _preferences.Layers.FontTitle.Brush?.CreateBrush())
            {
                canvas.DrawText(this.Element.Id, _preferences.Layers.FontTitle.CreateFontInfo(), rectTitle, brush, _preferences.Layers.FontTitle.Format);
            }
        }

        protected abstract INodeDrawing CreateDrawingNode(TNode node);
    }
}
