using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Drawing.Layers;
using NeuralNetworkVisualizer.Drawing.Nodes;
using NeuralNetworkVisualizer.Model.Layers;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetworkVisualizer
{
    public partial class NeuralNetworkVisualizerControl : UserControl
    {
        public NeuralNetworkVisualizerControl()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = true;
            this.BackColor = Color.White;
        }

        private Preference _preferences = new Preference();
        [Browsable(false)]
        public Preference Preferences
        {
            get { return _preferences; }
        }

        private InputLayer _InputLayer = null;
        [Browsable(false)]
        public InputLayer InputLayer
        {
            get
            {
                return _InputLayer;
            }
            set
            {
                _InputLayer = value;
                Redraw();
            }
        }

        private float _zoom = 1;
        [Browsable(false)]
        public float Zoom
        {
            get => _zoom;
            set
            {
                _zoom = value;
                Redraw();
            }
        }

        [Browsable(false)]
        public Image Image { get => SafeInvoke(() => picCanvas.Image.Clone() as Image); } //Clone for safe handling

        public void Redraw()
        {
            var task = SafeInvoke(() =>
             {
                 return RedrawInternal();
             });

            if (task.Exception != null)
            {
                _InputLayer = null;
                throw task.Exception.InnerException;
            }
        }

        public async Task RedrawAsync()
        {
#pragma warning disable CS1998
            var task = SafeInvoke(async () =>
            {
                return RedrawInternal();
            });
#pragma warning restore CS1998

            try
            {
                await task.ConfigureAwait(true).GetAwaiter().GetResult();
            }
            catch
            {
                _InputLayer = null;
                throw;
            }
        }

        private async Task RedrawInternal()
        {
            if (!this.IsHandleCreated)
                return;

            if (picCanvas.Image != null) //Clear before anything
            {
                picCanvas.Image.Dispose();
                picCanvas.Image = null;
            }

            if (_InputLayer != null)
            {
                _InputLayer.Validate();
            }
            else
            {
                picCanvas.ClientSize = this.ClientSize;
                picCanvas.BackColor = this.BackColor;
                return;
            }

            picCanvas.Size = new Size((int)(_zoom * this.ClientSize.Width), (int)(_zoom * this.ClientSize.Height));

            Bitmap bmp = new Bitmap(picCanvas.ClientSize.Width, picCanvas.ClientSize.Height);
            Graphics graph = Graphics.FromImage(bmp);

            graph.Clear(this.BackColor);
            SetQuality(graph);

            await DrawLayers(graph);

            picCanvas.Image = bmp;
            Destroy.Disposable(ref graph);
        }

        private Size _previousSize = Size.Empty;
        protected async override void OnSizeChanged(EventArgs e)
        {
            _previousSize = this.ClientSize;

            if (!this.ClientSize.IsEmpty)
            {
                if (!_previousSize.IsEmpty)
                {
                    await RedrawAsync();
                }
            }

            base.OnSizeChanged(e);
        }

        private void SetQuality(Graphics graphics)
        {
            switch (_preferences.Quality)
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
                    throw new InvalidOperationException($"Quality not implemented: {_preferences.Quality}");
            }
        }

        private async Task DrawLayers(Graphics graph)
        {
            var layersDrawingSize = GetLayersDrawingSize();
            var graphCanvas = new GraphicsCanvas(graph, picCanvas.ClientSize.Width, picCanvas.ClientSize.Height);
            int x = 0;

            IDictionary<NodeBase, INodeDrawing> previousNodesDic = new Dictionary<NodeBase, INodeDrawing>();

            LayerSizesPreCalc layersCache = new LayerSizesPreCalc(layersDrawingSize, _InputLayer.GetMaxNodeCountInLayer(), _preferences);
            SimpleNodeSizesPreCalc simpleNodesCache = new SimpleNodeSizesPreCalc();
            PerceptronSizesPreCalc perceptronCache = new PerceptronSizesPreCalc(_preferences);
            EdgeSizesPreCalc edgesCache = new EdgeSizesPreCalc();

            for (LayerBase layer = _InputLayer; layer != null; layer = layer.Next)
            {
                ILayerDrawing layerDrawing = null;

                if (layer == _InputLayer)
                {
                    layerDrawing = new InputLayerDrawing(layer as InputLayer, _preferences, layersCache, simpleNodesCache);
                }
                else
                {
                    layerDrawing = new PerceptronLayerDrawing(layer as PerceptronLayer, previousNodesDic, graphCanvas, _preferences, layersCache, perceptronCache, simpleNodesCache, edgesCache);
                }

                var canvasRect = new Rectangle(x, 0, layersDrawingSize.Width, layersDrawingSize.Height);
                var layerCanvas = new NestedCanvas(canvasRect, graphCanvas);

                await Task.Factory.StartNew((object canvasLayerParam) =>
                {
                    var canvasLayer = canvasLayerParam as Tuple<ILayerDrawing, ICanvas>; //I prefer rather than ValueTuple
                    SafeInvoke(() => { canvasLayer.Item1.Draw(canvasLayer.Item2); });

                }, new Tuple<ILayerDrawing, ICanvas>(layerDrawing, layerCanvas));

                previousNodesDic = layerDrawing.NodesDrawing.ToDictionary(n => n.Node, n => n);

                x += layersDrawingSize.Width;
            }
        }

        private Size GetLayersDrawingSize()
        {
            var layersCount = _InputLayer.LayersCount();

            var layerWidth = picCanvas.ClientSize.Width / layersCount;
            var layerHeight = picCanvas.ClientSize.Height;

            return new Size(layerWidth, layerHeight);
        }

        private void SafeInvoke(Action action)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private T SafeInvoke<T>(Func<T> action)
        {
            return (this.InvokeRequired ? (T)this.Invoke(action) : action());
        }

        protected override void Dispose(bool disposing)
        {
            Destroy.Disposable(ref _preferences);

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
