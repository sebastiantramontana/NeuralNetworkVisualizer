using System.Drawing;

namespace NeuralNetworkVisualizer.Drawing.Canvas
{
    internal interface ICanvas
    {
        int MaxWidth { get; }
        int MaxHeight { get; }

        Point Translate(Point point, ICanvas destination);
        void DrawEllipse(Rectangle rect, Pen pen, Brush brush);
        void DrawLine(Point p1, Point p2, Pen pen);
        void DrawRectangle(Rectangle rect, Pen pen, Brush brush);
        void DrawText(string text, Font font, Point position, Brush brush, StringFormat format);
        void DrawText(string text, FontInfo font, Rectangle rect, Brush brush, StringFormat format);
        void DrawText(string text, FontInfo font, Rectangle rect, Brush brush, StringFormat format, float angle);
        void DrawImage(Image image, Point position, Size size);
        Size MeasureText(string text, Font font, Point position, StringFormat format);
    }
}
