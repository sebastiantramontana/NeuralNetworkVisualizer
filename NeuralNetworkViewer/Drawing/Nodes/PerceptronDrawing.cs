using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Model;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NeuralNetworkVisualizer.Drawing.Nodes
{
    internal class PerceptronDrawing : NodeBaseDrawing<Perceptron>
    {
        private readonly IDictionary<NodeBase, INodeDrawing> _previousNodes;
        private readonly ICanvas _edgesCanvas;
        private readonly Preference _preferences;
        private readonly PerceptronSizesPreCalc _cache;
        private readonly EdgeSizesPreCalc _edgesCache;

        internal PerceptronDrawing(Perceptron element, IDictionary<NodeBase, INodeDrawing> previousNodes, ICanvas edgesCanvas, Preference preferences, PerceptronSizesPreCalc cache, EdgeSizesPreCalc edgesCache) : base(element, preferences.Perceptrons, cache)
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
            var sumRectangle = new Rectangle(new Point(valuesX, (activationFunctionPosition.Y - _cache.SumSize.Height) - _preferences.NodeMargins), _cache.SumSize);
            var outputRectangle = new Rectangle(new Point(valuesX, _cache.GetOutputPositionY(rect.Y)), _cache.OutputSize.Value);

            return (sumRectangle, activationFunctionPosition, _cache.ActivationFunctionSize, outputRectangle, inputPosition);
        }

        private void DrawActivationFunction(Point position, Size size, ICanvas canvas)
        {
            if (size.Width <= 8 || size.Height <= 8)
            {
                return;
            }

            switch (this.Element.ActivationFunction)
            {
                case ActivationFunction.Relu:
                    DrawStrokedActivationFunction(size, (pen, stroke) =>
                    {
                        canvas.DrawLine(new Point(position.X, (position.Y + size.Height) - stroke), new Point(position.X + size.Width / 2, (position.Y + size.Height) - stroke), pen);
                        canvas.DrawLine(new Point(position.X + size.Width / 2 - stroke / 2, (position.Y + size.Height) - stroke), new Point((position.X + size.Width) - stroke, position.Y), pen);

                    });
                    break;

                case ActivationFunction.None:
                    //Don't draw anything!
                    break;

                case ActivationFunction.BinaryStep:
                    DrawStrokedActivationFunction(size, (pen, stroke) =>
                    {
                        canvas.DrawLine(new Point(position.X + size.Width / 2, position.Y + stroke / 2), new Point(position.X + size.Width, position.Y + stroke / 2), pen);
                        canvas.DrawLine(new Point(position.X + size.Width / 2, position.Y), new Point(position.X + size.Width / 2, position.Y + size.Height), pen);
                        canvas.DrawLine(new Point(position.X, position.Y + size.Height - stroke / 2), new Point(position.X + size.Width / 2, position.Y + size.Height - stroke / 2), pen);
                    });
                    break;

                case ActivationFunction.Linear:
                    DrawStrokedActivationFunction(size, (pen, stroke) =>
                    {
                        canvas.DrawLine(new Point(position.X, position.Y + size.Height), new Point(position.X + size.Width, position.Y), pen);
                    });
                    break;

                case ActivationFunction.LeakyRelu:
                    DrawStrokedActivationFunction(size, (pen, stroke) =>
                    {
                        canvas.DrawLine(new Point(position.X, (position.Y + size.Height) - stroke), new Point(position.X + size.Width / 2, (position.Y + size.Height - size.Height / 10) - stroke), pen);
                        canvas.DrawLine(new Point(position.X + size.Width / 2 - stroke / 2, (position.Y + size.Height - size.Height / 10) - stroke), new Point((position.X + size.Width) - stroke, position.Y), pen);
                    });
                    break;

                case ActivationFunction.Softmax:
                    DrawStrokedActivationFunction(size, (pen, stroke) =>
                    {
                        var x_centered = position.X + size.Width / 2 - stroke / 2;

                        canvas.DrawLine(new Point(x_centered - (int)(stroke * 0.5) - (int)(stroke * 1.5), position.Y), new Point(x_centered - (int)(stroke * 0.5) + (int)(stroke * 1.5), position.Y), pen);
                        canvas.DrawLine(new Point(x_centered - (int)(stroke * 0.5), position.Y), new Point(x_centered - (int)(stroke * 0.5), position.Y + size.Height), pen);
                        canvas.DrawLine(new Point(x_centered - (int)(stroke * 0.5) - (int)(stroke * 1.5), position.Y + size.Height), new Point(x_centered - (int)(stroke * 0.5) + (int)(stroke * 1.5), position.Y + size.Height), pen);
                        canvas.DrawLine(new Point(x_centered + (int)(stroke * 2.5), position.Y + (int)(size.Height * 0.2)), new Point(x_centered + (int)(stroke * 2.5), position.Y + size.Height + stroke / 2), pen);
                    });

                    break;

                case ActivationFunction.Sigmoid:
                    DrawByCharActivationFunction('\u0283', "Tahoma", position, size, canvas);
                    break;

                case ActivationFunction.Tanh:
                    DrawByCharActivationFunction('\u222B', "Cursiva", position, size, canvas);
                    break;

                default:
                    throw new InvalidOperationException("Inexistent drawing for the following activation function: " + this.Element.ActivationFunction);
            }
        }

        private void DrawStrokedActivationFunction(Size size, Action<Pen, int> drawAction)
        {
            var stroke = Math.Min(size.Width, size.Height) / 15;
            using (var pen = new Pen(Color.Black, stroke))
            {
                drawAction(pen, stroke);
            }
        }

        private void DrawByCharActivationFunction(char character, string fontfamily, Point position, Size size, ICanvas canvas)
        {
            var factor = size.Height / 5;
            var factorSize = factor * 2 - factor / 3;

            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.None })
            {
                canvas.DrawText(character.ToString(), new FontInfo(fontfamily, FontStyle.Italic), new Rectangle(position.X - factor, position.Y - factor, size.Width + factorSize, size.Height + factorSize), Brushes.Black, format);
            }
        }
    }
}
