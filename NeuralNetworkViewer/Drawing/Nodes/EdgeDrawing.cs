using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;
using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Drawing.Nodes
{
    internal class EdgeDrawing : DrawingBase<Edge>
    {
        private readonly EdgePreference _preferences;
        private readonly Point _fromPosition;
        private readonly Point _toPosition;
        private readonly int _textHeight;
        private readonly EdgeSizesCache _cache;

        internal EdgeDrawing(Edge element, EdgePreference preferences, Point fromPosition, Point toPosition, int textHeight, EdgeSizesCache cache) : base(element)
        {
            _preferences = preferences;
            _fromPosition = fromPosition;
            _toPosition = toPosition;
            _textHeight = textHeight;
            _cache = cache;
        }

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawLine(_fromPosition, _toPosition, _preferences.Connector);
            DrawWeight(canvas);
        }

        private void DrawWeight(ICanvas canvas)
        {
            if (!this.Element.Weight.HasValue)
                return;

            var weightValue = Math.Round(this.Element.Weight.Value, _preferences.RoundingDigits).ToString();
            var sizesPositions = GetSizesPositions();

            if (_preferences.Text != null)
            {
                using (var brush = _preferences.Text.Brush.CreateBrush())
                {
                    canvas.DrawText(weightValue, _preferences.Text.CreateFontInfo(), sizesPositions.TextRectangle, brush, _preferences.Text.Format, sizesPositions.Angle);
                }
            }
        }

        private (Rectangle TextRectangle, float Angle) GetSizesPositions()
        {
            if (!_cache.IsInitilized)
            {
                int totalWidth = _toPosition.X - _fromPosition.X;
                double xOffset = (totalWidth - totalWidth / 3);
                double widthPortion = xOffset / totalWidth;

                int textWidth = (int)(totalWidth / 4d);

                _cache.Initialize(totalWidth, widthPortion, textWidth);
            }

            int totalHeight = _toPosition.Y - _fromPosition.Y;

            int y;
            if (totalHeight > 0)
            {
                y = (int)(_fromPosition.Y + Math.Round(_cache.WidthPortion * totalHeight));
            }
            else
            {
                var totalHeightPositive = totalHeight * -1; //make positive
                y = (int)(_toPosition.Y + Math.Round(totalHeightPositive - (_cache.WidthPortion * totalHeightPositive)));
            }

            float angle = (float)(Math.Atan2(totalHeight, _cache.TotalWidth) * (180d / Math.PI));
            int x = (int)(_toPosition.X - (_cache.TotalWidth / 3d));

            return (new Rectangle(x, y, _cache.TextWidth, _textHeight), angle);
        }
    }
}
