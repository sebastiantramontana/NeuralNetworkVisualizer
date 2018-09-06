using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;
using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Drawing.Nodes
{
    internal interface INodeDrawing : IDrawing
    {
        ICanvas Canvas { get; }
        Point EdgeStartPosition { get; }
        NodeBase Node { get; }
    }

    internal abstract class NodeBaseDrawing<TNode> : DrawingBase<TNode>, INodeDrawing where TNode : NodeBase
    {
        private readonly NodePreference _preferences;
        private readonly NodeSizesCache _cache;

        internal NodeBaseDrawing(TNode element, NodePreference preferences, NodeSizesCache cache) : base(element)
        {
            _preferences = preferences;
            _cache = cache;
        }

        public Point EdgeStartPosition { get; private set; }
        public ICanvas Canvas { get; private set; }

        public override void Draw(ICanvas canvas)
        {
            if (!_cache.EllipseRectangle.HasValue)
            {
                var side = Math.Min(canvas.MaxWidth, canvas.MaxHeight);

                var x_centered = (canvas.MaxWidth - side) / 2;
                var y_centered = (canvas.MaxHeight - side) / 2;

                _cache.EllipseRectangle = new Rectangle(x_centered, y_centered, side, side);
            }

            this.Canvas = canvas;
            this.EdgeStartPosition = new Point(_cache.EllipseRectangle.Value.X + _cache.EllipseRectangle.Value.Width, _cache.EllipseRectangle.Value.Y + (_cache.EllipseRectangle.Value.Height / 2));

            using (var backBrush = _preferences.Background.CreateBrush())
            {
                canvas.DrawEllipse(_cache.EllipseRectangle.Value, _preferences.Border, backBrush);
            }

            DrawContent(canvas, _cache.EllipseRectangle.Value);
        }

        public NodeBase Node => this.Element;
        protected abstract void DrawContent(ICanvas canvas, Rectangle rect);
    }
}
