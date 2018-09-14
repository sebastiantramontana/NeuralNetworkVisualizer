﻿using NeuralNetworkVisualizer.Drawing.Cache;
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
            if (!this.IsHandleCreated)
                return;

            if (picCanvas.Image != null)
            {
                picCanvas.Image.Dispose();
                picCanvas.Image = null;
            }

            if (_InputLayer == null)
            {
                picCanvas.ClientSize = this.ClientSize;
                return;
            }

            picCanvas.Size = new Size((int)(_zoom * this.ClientSize.Width), (int)(_zoom * this.ClientSize.Height));

            Bitmap bmp = new Bitmap(picCanvas.ClientSize.Width, picCanvas.ClientSize.Height);
            Graphics graph = Graphics.FromImage(bmp);

            graph.Clear(this.BackColor);
            SetQuality(graph);
            DrawLayers(graph);

            picCanvas.Image = bmp;

            Destroy.Disposable(ref graph);
        }

        private Size _previousSize = Size.Empty;
        protected override void OnSizeChanged(EventArgs e)
        {
            if (!this.ClientSize.IsEmpty)
            {
                if (!_previousSize.IsEmpty)
                {
                    Redraw();
                }
            }

            _previousSize = this.ClientSize;
            base.OnSizeChanged(e);
        }

        private void ValidateModel(InputLayer inputLayer)
        {
            if (inputLayer == null)
            {
                ///it's right! InputLayer can be null.
                return;
            }

            LayerBase outputlayer = SearchOutputLayer(inputLayer);

            if (outputlayer == inputLayer)
            {
                throw new MissingOutputException(inputLayer);
            }

            ///Bias doesn´t make sense to be in the output layer
            ///TODO: Fix Model, make an OutputLayer
            if (outputlayer.Bias != null)
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
                var canvasRect = new Rectangle(x, 0, layersDrawingSize.Width, layersDrawingSize.Height);
                var layerCanvas = new NestedCanvas(canvasRect, graphCanvas);

                ILayerDrawing layerDrawing = null;

                if (layer == _InputLayer)
                {
                    layerDrawing = new InputLayerDrawing(layer as InputLayer, _preferences, layersCache, simpleNodesCache);
                }
                else
                {
                    layerDrawing = new PerceptronLayerDrawing(layer as PerceptronLayer, previousNodesDic, graphCanvas, _preferences, layersCache, perceptronCache, simpleNodesCache, edgesCache);
                }

                layerDrawing.Draw(layerCanvas);
                previousNodesDic = layerDrawing.NodesDrawing.ToDictionary(n => n.Node, n => n);

                x += layersDrawingSize.Width - 1;
            }
        }

        private Size GetLayersDrawingSize()
        {
            var layersCount = _InputLayer.LayersCount();

            var layerWidth = picCanvas.ClientSize.Width / layersCount;
            var layerHeight = picCanvas.ClientSize.Height;

            return new Size(layerWidth, layerHeight);
        }

        protected override void Dispose(bool disposing)
        {
            Destroy.Disposable(ref _preferences);
            PerceptronDrawing.DestroyActivationFunctionImagesCache();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}