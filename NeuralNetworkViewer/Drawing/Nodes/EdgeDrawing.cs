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
        private readonly EdgeSizesPreCalc _cache;

        internal EdgeDrawing(Edge element, EdgePreference preferences, Point fromPosition, Point toPosition, int textHeight, EdgeSizesPreCalc cache) : base(element)
        {
            _preferences = preferences;
            _fromPosition = fromPosition;
            _toPosition = toPosition;
            _textHeight = textHeight;
            _cache = cache;
        }

        public override void Draw(ICanvas canvas)
        {
            ///Don't use a system Pen!
            using (var pen = _preferences.Connector.GetFormat(this.Element.Weight))
            {
                canvas.DrawLine(_fromPosition, _toPosition, pen);
            }

            DrawWeight(canvas);
        }

        private void DrawWeight(ICanvas canvas)
        {
            if (!this.Element.Weight.HasValue)
                return;

            var weightValue = Math.Round(this.Element.Weight.Value, _preferences.RoundingDigits).ToString();
            var sizesPositions = GetSizesPositions();

            using (var valueFormat = _preferences.ValueFormatter.GetFormat(this.Element.Weight.Value))
            using (var brush = valueFormat.Brush.CreateBrush())
            {
                canvas.DrawText(weightValue, valueFormat.CreateFontInfo(), sizesPositions.TextRectangle, brush, valueFormat.Format, sizesPositions.Angle);
            }
        }

        private (Rectangle TextRectangle, float Angle) GetSizesPositions()
        {
            int totalHeight = _toPosition.Y - _fromPosition.Y;
            var sizePosValues = _cache.GetValues(_fromPosition.X, _toPosition.X);

            int y;
            int x;
            float angle = (float)(Math.Atan2(totalHeight, sizePosValues.TotalWidth) * (180d / Math.PI));

            if (angle >= 0) //it depends on connector angle
            {
                y = (int)(_fromPosition.Y + sizePosValues.WidthPortionNear * totalHeight);
                x = sizePosValues.NearX;
            }
            else
            {
                var totalHeightPositive = totalHeight * -1; //turn positive
                y = (int)(_toPosition.Y + Math.Round(totalHeightPositive - (sizePosValues.WidthPortionFar * totalHeightPositive)));
                x = sizePosValues.FarX;
            }

            return (new Rectangle(x, y, sizePosValues.TextWidth, _textHeight), angle);
        }
    }
}
