using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Model;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;
using NeuralNetworkVisualizer.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace NeuralNetworkVisualizer.Drawing.Nodes
{
    internal class PerceptronDrawing : NodeBaseDrawing<Perceptron>
    {
        private readonly IDictionary<NodeBase, INodeDrawing> _previousNodes;
        private readonly ICanvas _edgesCanvas;
        private readonly Preference _preferences;
        private readonly PerceptronSizesCache _cache;
        private readonly EdgeSizesCache _edgesCache;

        private static IDictionary<ActivationFunction, Image> _activationFunctionImageCache;

        static PerceptronDrawing()
        {
            _activationFunctionImageCache = new Dictionary<ActivationFunction, Image>(8);
            PreloadActivationFunctionImages();
        }

        internal PerceptronDrawing(Perceptron element, IDictionary<NodeBase, INodeDrawing> previousNodes, ICanvas edgesCanvas, Preference preferences, PerceptronSizesCache cache, EdgeSizesCache edgesCache) : base(element, preferences.Perceptrons, cache)
        {
            _previousNodes = previousNodes;
            _edgesCanvas = edgesCanvas;
            _preferences = preferences;
            _cache = cache;
            _edgesCache = edgesCache;
        }

        protected override void DrawContent(ICanvas canvas, Rectangle rect)
        {
            var sizesPositions = GetSizePositions(rect);

            var roundingDigits = _preferences.Perceptrons.RoundingDigits;
            //var perceptronFont = _preferences.Perceptrons.Text.CreateFontInfo();
            //var fontFormatPreference = _preferences.Perceptrons.Text.Format;

            if (this.Element.SumValue.HasValue)
            {
                using (var sumFormat = _preferences.Perceptrons.SumValueFormatter.GetFormat(this.Element.SumValue.Value))
                using (var sumBrushFontPreference = sumFormat.Brush.CreateBrush())
                {

                    canvas.DrawText('\u2211' + " " + Math.Round(this.Element.SumValue.Value, roundingDigits).ToString(), sumFormat.CreateFontInfo(), sizesPositions.SumRectangle, sumBrushFontPreference, sumFormat.Format);
                }
            }

            DrawActivationFunction(sizesPositions.ActivationFunctionPosition, sizesPositions.ActivationFunctionSize, canvas);

            if (this.Element.OutputValue.HasValue)
            {
                using (var outputFormat = _preferences.Perceptrons.OutputValueFormatter.GetFormat(this.Element.OutputValue.Value))
                using (var outputBrushFontPreference = outputFormat.Brush.CreateBrush())
                {

                    canvas.DrawText(Math.Round(this.Element.OutputValue.Value, roundingDigits).ToString(), outputFormat.CreateFontInfo(), sizesPositions.OutputRectangle, outputBrushFontPreference, outputFormat.Format);
                }
            }

            DrawEdges(sizesPositions.InputPosition, canvas, sizesPositions.OutputRectangle.Height);
        }

        private void DrawEdges(Point inputPosition, ICanvas canvas, int textEdgeHeight)
        {
            foreach (var edge in this.Element.Edges)
            {
                var previousNode = _previousNodes[edge.Source];
                var outputPositionTrans = previousNode.Canvas.Translate(previousNode.EdgeStartPosition, _edgesCanvas);
                var inputPositionTrans = canvas.Translate(inputPosition, _edgesCanvas);

                var edgeDrawing = new EdgeDrawing(edge, _preferences.Edges, outputPositionTrans, inputPositionTrans, textEdgeHeight, _edgesCache);
                edgeDrawing.Draw(_edgesCanvas);
            }
        }

        private (Rectangle SumRectangle, Point ActivationFunctionPosition, Size ActivationFunctionSize, Rectangle OutputRectangle, Point InputPosition)
            GetSizePositions(Rectangle rect)
        {
            if (!_cache.OutputSize.HasValue)
            {
                _cache.OutputSize = _cache.SumSize; //the same size
            }

            var side = rect.Width;

            var inputPosition = new Point(rect.X, _cache.GetInputPositionY(rect.Y));
            var valuesX = (rect.X + side / 2) - (_cache.SumSize.Width / 2);

            var activationFunctionPosition = _cache.GetActivationFunctionPosition(rect);
            var sumRectangle = new Rectangle(new Point(valuesX, (activationFunctionPosition.Y - _cache.SumSize.Height) - _preferences.Margins), _cache.SumSize);
            var outputRectangle = new Rectangle(new Point(valuesX, _cache.GetOutputPositionY(rect.Y)), _cache.OutputSize.Value);

            return (sumRectangle, activationFunctionPosition, _cache.ActivationFunctionSize, outputRectangle, inputPosition);
        }

        private void DrawActivationFunction(Point position, Size size, ICanvas canvas)
        {
            if (size.Width > 8 && size.Height > 8)
            {
                var bmp = GetActivationFunctionImage(this.Element.ActivationFunction);
                canvas.DrawImage(bmp, position, size);
            }
        }

        internal static void DestroyActivationFunctionImagesCache()
        {
            if (_activationFunctionImageCache == null)
                return;

            foreach (var imgcache in _activationFunctionImageCache.Values)
            {
                if (imgcache != null)
                    imgcache.Dispose();
            }

            _activationFunctionImageCache.Clear();
            _activationFunctionImageCache = null;
        }

        private static void PreloadActivationFunctionImages()
        {
            var actFuncs = Enum.GetValues(typeof(ActivationFunction));

            foreach (var af in actFuncs)
            {
                PreloadActivationFunctionImage((ActivationFunction)af);
            }
        }

        private static void PreloadActivationFunctionImage(ActivationFunction activationFunction)
        {
            byte[] bytes;

            switch (activationFunction)
            {
                case ActivationFunction.None:
                    bytes = Resources.None;
                    break;
                case ActivationFunction.BinaryStep:
                    bytes = Resources.Step;
                    break;
                case ActivationFunction.Linear:
                    bytes = Resources.Linear;
                    break;
                case ActivationFunction.Sigmoid:
                    bytes = Resources.Sigmoide;
                    break;
                case ActivationFunction.Tanh:
                    bytes = Resources.Tanh;
                    break;
                case ActivationFunction.Relu:
                    bytes = Resources.Relu;
                    break;
                case ActivationFunction.LeakyRelu:
                    bytes = Resources.LeakyRelu;
                    break;
                case ActivationFunction.Softmax:
                    bytes = Resources.Softmax;
                    break;
                default:
                    throw new InvalidOperationException("Inexistent icon for the following activation function: " + activationFunction);
            }

            using (var stream = new MemoryStream(bytes))
            {
                var image = new Bitmap(stream);
                _activationFunctionImageCache.Add(activationFunction, image);
            }
        }

        private static Image GetActivationFunctionImage(ActivationFunction activationFunction)
        {
            if (!_activationFunctionImageCache.TryGetValue(activationFunction, out Image image))
            {
                throw new InvalidOperationException("Inexistent preloaded icon for the following activation function: " + activationFunction);
            }

            return image;
        }
    }
}
