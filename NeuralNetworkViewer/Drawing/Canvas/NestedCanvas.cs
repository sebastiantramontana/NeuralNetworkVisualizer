using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Drawing.Canvas
{
    internal class NestedCanvas : ICanvas
    {
        private readonly Rectangle _Rect;
        private readonly ICanvas _Container;
        public NestedCanvas(Rectangle rect, ICanvas container)
        {
            _Rect = rect;
            _Container = container;
        }

        public int MaxWidth => _Rect.Width;
        public int MaxHeight => _Rect.Height;

        public void DrawEllipse(Rectangle rect, Pen pen, Brush brush)
        {
            rect.Offset(_Rect.Location);
            rect.Width = Math.Min(this.MaxWidth, rect.Width);
            rect.Height = Math.Min(this.MaxHeight, rect.Height);

            _Container.DrawEllipse(rect, pen, brush);
        }

        public void DrawLine(Point p1, Point p2, Pen pen)
        {
            p1.Offset(_Rect.Location);
            p2.Offset(_Rect.Location);

            _Container.DrawLine(p1, p2, pen);
        }

        public void DrawRectangle(Rectangle rect, Pen pen, Brush brush)
        {
            rect.Offset(_Rect.Location);
            rect.Width = Math.Min(this.MaxWidth, rect.Width);
            rect.Height = Math.Min(this.MaxHeight, rect.Height);

            _Container.DrawRectangle(rect, pen, brush);
        }

        public void DrawText(string text, Font font, Point position, Brush brush, StringFormat format)
        {
            position.Offset(_Rect.Location);
            _Container.DrawText(text, font, position, brush, format);
        }

        public void DrawText(string text, FontInfo font, Rectangle rect, Brush brush, StringFormat format)
        {
            rect.Offset(_Rect.Location);
            _Container.DrawText(text, font, rect, brush, format);
        }

        public void DrawText(string text, FontInfo font, Rectangle rect, Brush brush, StringFormat format, float angle)
        {
            rect.Offset(_Rect.Location);
            _Container.DrawText(text, font, rect, brush, format, angle);
        }

        public Size MeasureText(string text, Font font, Point position, StringFormat format)
        {
            position.Offset(_Rect.Location);
            return _Container.MeasureText(text, font, position, format);
        }

        public void DrawImage(Image image, Point position, Size size)
        {
            position.Offset(_Rect.Location);
            size.Width = Math.Min(this.MaxWidth, size.Width);
            size.Height = Math.Min(this.MaxHeight, size.Height);

            _Container.DrawImage(image, position, size);
        }

        public Point Translate(Point position, ICanvas destination)
        {
            if (destination == this)
                return position;

            position.Offset(_Rect.Location.X, _Rect.Location.Y);
            return _Container.Translate(position, destination);
        }
    }
}
