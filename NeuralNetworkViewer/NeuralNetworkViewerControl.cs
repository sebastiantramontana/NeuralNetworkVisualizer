using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Drawing.Layers;
using NeuralNetworkVisualizer.Drawing.Nodes;
using NeuralNetworkVisualizer.Exceptions;
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
using System.Windows.Forms;

namespace NeuralNetworkVisualizer
{
    public partial class NeuralNetworkVisualizerControl : UserControl
    {
        public NeuralNetworkVisualizerControl()
        {
            InitializeComponent();
            this.BackColor = Color.White;
        }

        private Preference _Preferences = new Preference();
        [Browsable(false)]
        public Preference Preferences
        {
            get { return _Preferences; }
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
                ValidateModel(value);
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
        public Image Image { get => picCanvas.Image; }

        public void Redraw()
        {
            if (picCanvas.Image != null)
            {
                picCanvas.Image.Dispose();
                picCanvas.Image = null;
            }

            if (_InputLayer == null)
            {
                picCanvas.Size = this.Size;
                return;
            }

            picCanvas.Size = new Size((int)(_zoom * this.ClientSize.Width - 1), (int)(_zoom * this.ClientSize.Height - 1));

            Bitmap bmp = new Bitmap(picCanvas.ClientSize.Width, picCanvas.ClientSize.Height);
            Graphics graph = Graphics.FromImage(bmp);

            graph.ScaleTransform(_zoom, _zoom);

            graph.Clear(this.BackColor);
            SetQuality(graph);
            DrawLayers(graph);

            picCanvas.Image = bmp;

            Destroy.Disposable(ref graph);
        }

        private Size _previousSize = Size.Empty;
        protected override void OnSizeChanged(EventArgs e)
        {
            if (!this.ClientRectangle.Size.IsEmpty)
            {
                if (!_previousSize.IsEmpty)
                {
                    Redraw();
                }
            }

            _previousSize = this.ClientRectangle.Size;
            base.OnSizeChanged(e);
        }

        private void ValidateModel(InputLayer inputLayer)
        {
            if(inputLayer==null)
            {
                ///it's right! InputLayer can be null.
                return;
            }

            LayerBase outputlayer = SearchOutputLayer(inputLayer);

            if (outputlayer == inputLayer)
            {
                throw new MissingOutputException(inputLayer);
            }

            if(outputlayer.Bias!=null)
            {
                throw new InvalidOutputBiasException(outputlayer);
            }
        }

        private LayerBase SearchOutputLayer(InputLayer inputLayer)
        {
            LayerBase outputlayer;
            for (outputlayer = inputLayer; outputlayer.Next != null; outputlayer = outputlayer.Next) ;

            return outputlayer;
        }

        private void SetQuality(Graphics graphics)
        {
            switch (_Preferences.Quality)
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
                    break;
            }
        }

        private void DrawLayers(Graphics graph)
        {
            var layersSize = GetLayersSize();
            var graphCanvas = new GraphicsCanvas(graph, this.ClientSize.Width, this.ClientSize.Height);
            int x = this.Preferences.Margins;
            var maxNodes = _InputLayer.GetMaxNodesInLayer();

            IDictionary<NodeBase, INodeDrawing> previousNodesDic = new Dictionary<NodeBase, INodeDrawing>();
            SimpleNodeSizesCache simpleNodesCache = new SimpleNodeSizesCache();
            PerceptronSizesCache perceptronCache = new PerceptronSizesCache(this.Preferences);
            EdgeSizesCache edgesCache = new EdgeSizesCache();

            for (LayerBase layer = _InputLayer; layer != null; layer = layer.Next)
            {
                var canvasRect = new Rectangle(x, this.Preferences.Margins, layersSize.Width, layersSize.Height);
                var layerCanvas = new NestedCanvas(canvasRect, graphCanvas);

                ILayerDrawing layerDrawing = null;

                if (layer == _InputLayer)
                {
                    layerDrawing = new InputLayerDrawing(layer as InputLayer, this.Preferences, simpleNodesCache);
                }
                else
                {
                    layerDrawing = new PerceptronLayerDrawing(layer as PerceptronLayer, previousNodesDic, graphCanvas, maxNodes, this.Preferences, perceptronCache, simpleNodesCache, edgesCache);
                }

                layerDrawing.Draw(layerCanvas);
                previousNodesDic = layerDrawing.NodesDrawing.ToDictionary(n => n.Node, n => n);

                x += layersSize.Width;
            }
        }

        private Size GetLayersSize()
        {
            var countLayers = _InputLayer.CountLayers();

            var layerWidth = this.ClientSize.Width / countLayers - (_Preferences.Margins * 2);
            var layerWWeight = this.ClientSize.Height - (_Preferences.Margins * 2);

            return new Size(layerWidth, layerWWeight);
        }

        protected override void Dispose(bool disposing)
        {
            Destroy.Disposable(ref this._Preferences);
            PerceptronDrawing.DestroyActivationFunctionImagesCache();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
