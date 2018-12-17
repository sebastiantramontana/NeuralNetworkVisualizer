using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Drawing.Layers;
using NeuralNetworkVisualizer.Drawing.Nodes;
using NeuralNetworkVisualizer.Model.Layers;
using NeuralNetworkVisualizer.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Drawing.Controls
{
    internal class ControlDrawing : IControlDrawing
    {
        private readonly IControlCanvas _controlCanvas;

        public ControlDrawing(IControlCanvas controlCanvas)
        {
            _controlCanvas = controlCanvas;
        }

        public IControlCanvas ControlCanvas => _controlCanvas;

        public Image GetImage()
        {
            return _controlCanvas.Image;
        }

        public void Redraw()
        {
            if (!_controlCanvas.IsReady)
            {
                return;
            }

            RedrawInternalAsync((graph) => { DrawLayers(graph); return Task.CompletedTask; }).Wait();
        }

        private bool _isDrawing = false; //flag to avoid multiple parallel drawing
        public async Task RedrawAsync()
        {
            if (!_controlCanvas.IsReady || _isDrawing)
            {
                return;
            }

            _isDrawing = true;
            await _controlCanvas.SafeInvoke(async () =>
            {
                await RedrawInternalAsync(async (graph) => await DrawLayersAsync(graph));
            });

            _isDrawing = false;
        }

        private async Task RedrawInternalAsync(Func<Graphics, Task> drawLayersAction)
        {
            if (!ValidateInputLayer())
            {
                _controlCanvas.SetBlank();
            }
            else
            {
                var graphAndImage = _controlCanvas.GetGraphics();

                await drawLayersAction(graphAndImage.Graph);
                _controlCanvas.Image = graphAndImage.Image;

                Destroy.Disposable(ref graphAndImage.Graph);
            }
        }

        private bool ValidateInputLayer()
        {
            var inputLayer = _controlCanvas.Control.InputLayer;

            if (inputLayer != null)
            {
                inputLayer.Validate();
                return true;
            }

            return false;
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
            var layersDrawingSize = _controlCanvas.GetLayersDrawingSize();
            var graphCanvas = new GraphicsCanvas(graph, _controlCanvas.Size.Width, _controlCanvas.Size.Height);
            int x = 0;

            IDictionary<NodeBase, INodeDrawing> previousNodesDic = new Dictionary<NodeBase, INodeDrawing>();

            var inputLayer = _controlCanvas.Control.InputLayer;
            var preferences = _controlCanvas.Control.Preferences;

            LayerSizesPreCalc layersCache = new LayerSizesPreCalc(layersDrawingSize, inputLayer.GetMaxNodeCountInLayer(), preferences);
            SimpleNodeSizesPreCalc simpleNodesCache = new SimpleNodeSizesPreCalc();
            PerceptronSizesPreCalc perceptronCache = new PerceptronSizesPreCalc(preferences);
            EdgeSizesPreCalc edgesCache = new EdgeSizesPreCalc();

            for (LayerBase layer = inputLayer; layer != null; layer = layer.Next)
            {
                ILayerDrawing layerDrawing = null;

                if (layer == inputLayer)
                {
                    layerDrawing = new InputLayerDrawing(layer as InputLayer, preferences, layersCache, simpleNodesCache);
                }
                else
                {
                    layerDrawing = new PerceptronLayerDrawing(layer as PerceptronLayer, previousNodesDic, graphCanvas, preferences, layersCache, perceptronCache, simpleNodesCache, edgesCache);
                }

                var canvasRect = new Rectangle(x, 0, layersDrawingSize.Width, layersDrawingSize.Height);
                var layerCanvas = new NestedCanvas(canvasRect, graphCanvas);

                await drawLayerAction(layerDrawing, layerCanvas);

                previousNodesDic = layerDrawing.NodesDrawing.ToDictionary(n => n.Node, n => n);

                x += layersDrawingSize.Width;
            }
        }
    }
}
