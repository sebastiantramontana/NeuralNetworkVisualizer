using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;
using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Drawing.Nodes
{
    internal abstract class SimpleNodeDrawing<TNode> : NodeBaseDrawing<TNode> where TNode : NodeBase
    {
        private readonly NodePreference _preferences;
        private readonly SimpleNodeSizesCache _cache;

        internal SimpleNodeDrawing(TNode element, NodePreference preferences, SimpleNodeSizesCache cache) : base(element, preferences, cache)
        {
            _preferences = preferences;
            _cache = cache;
        }

        protected override void DrawContent(ICanvas canvas, Rectangle rect)
        {
            if (this.Element.OutputValue.HasValue)
            {
                var outputRectangle = GetOutputRectangle(rect);

                using (var fontBrush = _preferences.Text.Brush?.CreateBrush())
                {
                    canvas.DrawText(Math.Round(this.Element.OutputValue.Value, _preferences.RoundingDigits).ToString(), _preferences.Text.CreateFontInfo(), outputRectangle, fontBrush, _preferences.Text.Format);
                }
            }
        }

        private Rectangle GetOutputRectangle(Rectangle rect)
        {
            if (_cache.OutputSize == null)
            {
                var side = rect.Width;
                var div3 = side / 3d;

                _cache.YCenteringOffeset = side / 2 - div3 / 2;
                _cache.OutputSize = new Size(side, (int)div3);
            }

            var outputRectangle = new Rectangle(new Point(rect.X, rect.Y + (int)_cache.YCenteringOffeset), _cache.OutputSize.Value);

            return outputRectangle;
        }
    }
}
