using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NeuralNetworkVisualizer.Drawing.Canvas
{
    internal class GraphicsCanvas : ICanvas
    {
        private Graphics _Graph;
        internal GraphicsCanvas(Graphics graph, int maxWidth, int maxHeight)
        {
            _Graph = graph;
            this.MaxWidth = maxWidth;
            this.MaxHeight = maxHeight;
        }

        public int MaxWidth { get; private set; }
        public int MaxHeight { get; private set; }

        internal void SetGraphics(Graphics graph)
        {
            _Graph = graph;
        }

        public void DrawRectangle(Rectangle rect, Pen pen, Brush brush)
        {
            rect.Width = Math.Min(this.MaxWidth, rect.Width);
            rect.Height = Math.Min(this.MaxHeight, rect.Height);

            if (Validate(brush))
                _Graph.FillRectangle(brush, rect);

            if (Validate(pen))
                _Graph.DrawRectangle(pen, rect);

        }

        public void DrawEllipse(Rectangle rect, Pen pen, Brush brush)
        {
            rect.Width = Math.Min(this.MaxWidth, rect.Width);
            rect.Height = Math.Min(this.MaxHeight, rect.Height);

            if (Validate(pen))
                _Graph.DrawEllipse(pen, rect);

            if (Validate(brush))
                _Graph.FillEllipse(brush, rect);
        }

        public void DrawText(string text, Font font, Point position, Brush brush, StringFormat format)
        {
            if (Validate(brush))
                _Graph.DrawString(text, font, brush, position, format); //if the other args are null throw the exception
        }

        public void DrawText(string text, FontInfo fontInfo, Rectangle rect, Brush brush, StringFormat format)
        {
            if (Validate(brush))
                DrawAdjustedFontString(text, fontInfo, rect.Size, (font) => _Graph.DrawString(text, font, brush, rect, format));
        }

        public void DrawText(string text, FontInfo fontInfo, Rectangle rect, Brush brush, StringFormat format, float angle)
        {
            if (!Validate(brush))
                return;

            DrawAdjustedFontString(text, fontInfo, rect.Size, (font) =>
            {
                var transform = _Graph.Transform;

                _Graph.TranslateTransform(rect.X, rect.Y);

                _Graph.RotateTransform(angle);
                _Graph.DrawString(text, font, brush, new Rectangle(0, 0, rect.Width, rect.Height), format);

                _Graph.Transform = transform;
            });
        }

        public Size MeasureText(string text, Font font, Point position, StringFormat format)
        {
            return Size.Ceiling(_Graph.MeasureString(text, font, position, format));
        }

        public void DrawLine(Point p1, Point p2, Pen pen)
        {
            if (Validate(pen))
                _Graph.DrawLine(pen, p1, p2);
        }

        public void DrawImage(Image image, Point position, Size size)
        {
            var interpolMode = _Graph.InterpolationMode;
            _Graph.InterpolationMode = InterpolationMode.Bilinear;

            _Graph.DrawImage(image, position.X, position.Y, size.Width, size.Height);

            _Graph.InterpolationMode = interpolMode;
        }

        public Point Translate(Point position, ICanvas destination)
        {
            if (destination == this )
                return position;

            var posTranslated = destination.Translate(new Point(0, 0), this);
            position = new Point(position.X - posTranslated.X, position.Y - posTranslated.Y);
            return position;
        }

        private void DrawAdjustedFontString(string text, FontInfo fontInfo, Size containerSize, Action<Font> drawAction)
        {
            var font = GetAdjustedFont(text, fontInfo, containerSize);

            if (font != null)
            {
                drawAction(font);
                font.Dispose();
            }
        }

        private Font GetAdjustedFont(string text, FontInfo font, Size containerSize)
        {
            //8px is the minimum font width
            for (int adjustedWidth = containerSize.Width; adjustedWidth >= 8; adjustedWidth--)
            {
                Font testFont = new Font(font.Family, adjustedWidth, font.Style, GraphicsUnit.Pixel);

                // Test the string with the new size
                SizeF adjustedSizeNew = _Graph.MeasureString(text, testFont);

                if (containerSize.Width >= (int)adjustedSizeNew.Width
                    && (containerSize.Height) >= (int)adjustedSizeNew.Height)
                {
                    return testFont;
                }

                testFont.Dispose();
            }

            return null;
        }

        private bool Validate(Brush brush)
        {
            var solid = brush as SolidBrush;

            return brush != null && (solid == null || solid.Color.ToArgb() != Color.Transparent.ToArgb());
        }

        private bool Validate(Pen pen)
        {
            return pen != null && pen.Color.ToArgb() != Color.Transparent.ToArgb();
        }
    }
}
