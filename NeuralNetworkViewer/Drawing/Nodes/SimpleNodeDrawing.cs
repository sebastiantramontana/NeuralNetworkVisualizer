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
        private readonly SimpleNodeSizesPreCalc _cache;

        internal SimpleNodeDrawing(TNode element, NodePreference preferences, SimpleNodeSizesPreCalc cache) : base(element, preferences, cache)
        {
            _preferences = preferences;
            _cache = cache;
        }

        protected override void DrawContent(ICanvas canvas, Rectangle rect)
        {
            if (!this.Element.OutputValue.HasValue)
                return;

            var outputRectangle = GetOutputRectangle(rect);

            using (var valueFormat = _preferences.OutputValueFormatter.GetFormat(this.Element.OutputValue.Value))
            using (var fontBrush = valueFormat.Brush.CreateBrush())
            {
                canvas.DrawText(Math.Round(this.Element.OutputValue.Value, _preferences.RoundingDigits).ToString(), valueFormat.CreateFontInfo(), outputRectangle, fontBrush, valueFormat.Format);
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
