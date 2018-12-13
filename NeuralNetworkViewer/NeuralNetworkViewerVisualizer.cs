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
                _zoom = 1f; //restart zoom
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
                if (_InputLayer == null)
                {
                    return; //nothing to do
                }

                _zoom = Constrain(0.1f, value, 10.0f); //limit the zoom value: Graphics will throw exception if not.
                Redraw();
            }
        }

        [Browsable(false)]
        public Image Image { get => SafeInvoke(() => picCanvas.Image?.Clone() as Image ?? new Bitmap(this.ClientSize.Width, this.ClientSize.Height)); } //Clone for safe handling

        public void Redraw()
        {
            RedrawInternalAsync((graph) => { DrawLayers(graph); return Task.CompletedTask; }).Wait();
        }

        private bool _isDrawing = false; //flag to avoid multiple parallel drawing
        public async Task RedrawAsync()
        {
            if (_isDrawing)
            {
                return;
            }

            _isDrawing = true;
            await SafeInvoke(async () =>
            {
                await RedrawInternalAsync(async (graph) => await DrawLayersAsync(graph));
            });

            _isDrawing = false;
        }

        private async Task RedrawInternalAsync(Func<Graphics, Task> drawLayersAction)
        {
            if (!PrepareCanvas())
                return;

            var graphAndImage = GetGraphics();

            await drawLayersAction(graphAndImage.Graph);
            picCanvas.Image = graphAndImage.Image;

            Destroy.Disposable(ref graphAndImage.Graph);
        }

        private bool PrepareCanvas()
        {
            if (!this.IsHandleCreated)
                return false;

            DestroyImageCanvas();

            if (!SetBlankCanvasIfNotInputValid())
                return false;

            ResizeCanvas();

            return true;
        }

        private (Graphics Graph, Image Image) GetGraphics()
        {
            Bitmap bmp = new Bitmap(picCanvas.ClientSize.Width, picCanvas.ClientSize.Height);
            Graphics graph = Graphics.FromImage(bmp);

            graph.Clear(this.BackColor);
            SetQuality(graph);

            return (graph, bmp);
        }

        private void ResizeCanvas()
        {
            picCanvas.Size = new Size((int)(_zoom * this.ClientSize.Width), (int)(_zoom * this.ClientSize.Height));
        }

        private bool SetBlankCanvasIfNotInputValid()
        {
            if (!ValidateInputLayer())
            {
                SetBlankCanvas();
                return false;
            }

            return true;
        }

        private bool ValidateInputLayer()
        {
            if (_InputLayer != null)
            {
                _InputLayer.Validate();
                return true;
            }

            return false;
        }

        private void SetBlankCanvas()
        {
            picCanvas.ClientSize = this.ClientSize;
            picCanvas.BackColor = this.BackColor;
        }

        private void DestroyImageCanvas()
        {
            if (picCanvas.Image != null) //Clear before anything
            {
                picCanvas.Image.Dispose();
                picCanvas.Image = null;
            }
        }

        private Size _previousSize = Size.Empty;
        protected override async void OnSizeChanged(EventArgs e)
        {
            _previousSize = this.ClientSize;

            if (!this.ClientSize.IsEmpty)
            {
                if (!_previousSize.IsEmpty)
                {
                    if (_preferences.AsyncRedrawOnResize)
                    {
                        await RedrawAsync();
                    }
                    else
                    {
                        Redraw();
                    }
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

        private void DrawLayers(Graphics graph)
        {
            DrawLayersGeneral(graph, (layerDrawing, layerCanvas) =>
            {
                layerDrawing.Draw(layerCanvas);
                return Task.CompletedTask;
            }).Wait();
        }

        private async Task DrawLayersAsync(Graphics graph)
        {
            await DrawLayersGeneral(graph, async (layerDrawing, layerCanvas) =>
            {
                await Task.Factory.StartNew((obj) =>
                {
                    var layerAndCanvas = obj as Tuple<ILayerDrawing, ICanvas>;
                    layerAndCanvas.Item1.Draw(layerAndCanvas.Item2);
                }, new Tuple<ILayerDrawing, ICanvas>(layerDrawing, layerCanvas));
            });
        }

        private async Task DrawLayersGeneral(Graphics graph, Func<ILayerDrawing, ICanvas, Task> drawLayerAction)
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

                await drawLayerAction(layerDrawing, layerCanvas);

                previousNodesDic = layerDrawing.NodesDrawing.ToDictionary(n => n.Node, n => n);

                x += layersDrawingSize.Width;
            }
        }

        private Size GetLayersDrawingSize()
        {
            var layersCount = _InputLayer.CountLayers();

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

        private T Constrain<T>(T low, T value, T max) where T : IComparable<T>
        {
            return (value.CompareTo(low) < 0 ? low : (value.CompareTo(max) > 0 ? max : value));
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
