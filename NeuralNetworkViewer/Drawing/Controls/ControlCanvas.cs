using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace NeuralNetworkVisualizer.Drawing.Controls
{
    internal class ControlCanvas : IControlCanvas
    {
        private readonly PictureBox _pictureBox;
        private readonly NeuralNetworkVisualizerControl _control;

        internal ControlCanvas(PictureBox pictureBox, NeuralNetworkVisualizerControl control)
        {
            _pictureBox = pictureBox;
            _control = control;
        }

        public NeuralNetworkVisualizerControl Control => _control;

        public Size Size
        {
            get => _pictureBox.ClientSize;
            set => _pictureBox.ClientSize = value;
        }

        public Image Image
        {
            get { return SafeInvoke(() => _pictureBox.Image?.Clone() as Image ?? new Bitmap(_control.ClientSize.Width, _control.ClientSize.Height)); } //Clone for safe handling
            set => _pictureBox.Image = value;
        }

        public bool IsReady => _control.IsHandleCreated;

        public void SafeInvoke(Action action)
        {
            if (_control.InvokeRequired)
            {
                _control.Invoke(action);
            }
            else
            {
                action();
            }
        }

        public T SafeInvoke<T>(Func<T> action)
        {
            return (_control.InvokeRequired ? (T)_control.Invoke(action) : action());
        }

        public void SetBlank()
        {
            DestroyImageCanvas();

            _pictureBox.ClientSize = _control.ClientSize;
            _pictureBox.BackColor = _control.BackColor;
        }

        public (Graphics Graph, Image Image) GetGraphics()
        {
            PrepareToDrawing();

            Bitmap bmp = new Bitmap(_pictureBox.ClientSize.Width, _pictureBox.ClientSize.Height);
            Graphics graph = Graphics.FromImage(bmp);

            graph.Clear(_pictureBox.Parent.BackColor);
            SetQuality(graph);

            return (graph, bmp);
        }

        public Size GetLayersDrawingSize()
        {
            var layersCount = _control.InputLayer.CountLayers();

            var layerWidth = _pictureBox.ClientSize.Width / layersCount;
            var layerHeight = _pictureBox.ClientSize.Height;

            return new Size(layerWidth, layerHeight);
        }

        private void PrepareToDrawing()
        {
            DestroyImageCanvas();
            ResizeCanvas();
        }

        private void ResizeCanvas()
        {
            _pictureBox.Size = new Size((int)(_control.Zoom * _pictureBox.Parent.ClientSize.Width), (int)(_control.Zoom * _pictureBox.Parent.ClientSize.Height));
        }

        private void DestroyImageCanvas()
        {
            if (_pictureBox.Image != null) //Clear before anything
            {
                _pictureBox.Image.Dispose();
                _pictureBox.Image = null;
            }
        }

        private void SetQuality(Graphics graphics)
        {
            switch (_control.Preferences.Quality)
            {
                case RenderQuality.Low:
                    graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.SmoothingMode = SmoothingMode.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.Low;
                    graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                    break;
                case RenderQuality.Medium:
                    graphics.PixelOffsetMode = PixelOffsetMode.Half;
                    graphics.CompositingQuality = CompositingQuality.AssumeLinear;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.Bicubic;
                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                    break;
                case RenderQuality.High:
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    break;
                default:
                    throw new InvalidOperationException($"Quality not implemented: {_control.Preferences.Quality}");
            }
        }
    }
}
