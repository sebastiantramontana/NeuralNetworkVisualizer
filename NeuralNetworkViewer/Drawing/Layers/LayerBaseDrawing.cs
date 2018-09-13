﻿using NeuralNetworkVisualizer.Drawing.Cache;
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
        private readonly Preference _preferences;
        private readonly LayerSizesPreCalc _cache;
        private readonly SimpleNodeSizesPreCalc _biasCache;
        private readonly IList<INodeDrawing> _nodesDrawing;

        internal LayerBaseDrawing(TLayer layer, Preference preferences, LayerSizesPreCalc cache, SimpleNodeSizesPreCalc biasCache) : base(layer)
        {
            _preferences = preferences;
            _cache = cache;
            _biasCache = biasCache;
            _nodesDrawing = new List<INodeDrawing>(layer.GetAllNodes().Count());
        }

        public IEnumerable<INodeDrawing> NodesDrawing { get { return _nodesDrawing; } }

        public override void Draw(ICanvas canvas)
        {
            var rect = new Rectangle(0, 0, canvas.MaxWidth, canvas.MaxHeight);

            using (var brush = _preferences.Layers.Background.CreateBrush())
            {
                canvas.DrawRectangle(rect, _preferences.Layers.Border, brush);
            }

            DrawTitle(canvas);
            DrawNodes(canvas);
        }

        private void DrawNodes(ICanvas canvas)
        {
            int y = _cache.StartingY + (_cache.TotalNodesHeight - _cache.NodeHeight * this.Element.GetAllNodes().Count()) / 2;

            if (this.Element.Bias != null)
            {
                var biasDrawing = new BiasDrawing(this.Element.Bias, _preferences, _biasCache);
                _nodesDrawing.Add(biasDrawing);

                y = DrawNode(biasDrawing, canvas, y);
            }

            foreach (var node in this.Element.Nodes)
            {
                var nodeDrawing = CreateDrawingNode(node);
                _nodesDrawing.Add(nodeDrawing);

                y = DrawNode(nodeDrawing, canvas, y);
            }
        }

        private int DrawNode(INodeDrawing nodeDrawing, ICanvas parentCanvas, int y)
        {
            var newCanvas = new NestedCanvas(new Rectangle(0, y, _cache.NodeWidth, _cache.NodeEllipseHeight), parentCanvas);
            nodeDrawing.Draw(newCanvas);

            return y + _cache.NodeHeight;
        }

        private void DrawTitle(ICanvas canvas)
        {
            if (_preferences.Layers.Title.Height <= 0)
            {
                return;
            }

            var rectTitle = new Rectangle(0, 0, canvas.MaxWidth, _preferences.Layers.Title.Height);

            if (_preferences.Layers.Title.Background is GradientBrushPreference backTitle)
            {
                backTitle.Rectangle = rectTitle;
            }

            using (var backgroundTitle = _preferences.Layers.Title.Background.CreateBrush())
            {
                canvas.DrawRectangle(rectTitle, null, backgroundTitle);
            }

            using (var brush = _preferences.Layers.Title.Font.Brush.CreateBrush())
            {
                canvas.DrawText(this.Element.Id, _preferences.Layers.Title.Font.CreateFontInfo(), rectTitle, brush, _preferences.Layers.Title.Font.Format);
            }
        }

        protected abstract INodeDrawing CreateDrawingNode(TNode node);
    }
}
