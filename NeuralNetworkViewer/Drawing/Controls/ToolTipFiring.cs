using NeuralNetworkVisualizer.Model;
using NeuralNetworkVisualizer.Model.Layers;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Selection;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NeuralNetworkVisualizer.Drawing.Controls
{
    internal class ToolTipFiring : IToolTipFiring
    {
        private readonly NeuralNetworkVisualizerControl _control;
        private readonly Control _controlToToolTip;
        private readonly ISelectionResolver _selectionResolver;

        private System.Timers.Timer timeout = null;
        private ToolTip tipInfo = null;
        private Point lastToolTipLocation;

        internal ToolTipFiring(NeuralNetworkVisualizerControl control, Control controlToToolTip, ISelectionResolver selectionResolver)
        {
            _control = control;
            _controlToToolTip = controlToToolTip;
            _selectionResolver = selectionResolver;
        }

        public void Show(Point location)
        {
            if (!Validate(location))
                return;

            Destroy.Disposable(ref timeout);
            Destroy.Disposable(ref tipInfo);

            timeout = new System.Timers.Timer();
            timeout.AutoReset = false;
            timeout.Enabled = true;
            timeout.Interval = 500;
            timeout.Elapsed += (s, ev) =>
            {
                Destroy.Disposable(ref timeout);
                Destroy.Disposable(ref tipInfo);

                var elem = _selectionResolver.GetElementFromLocation(location);

                if (elem != null)
                {
                    tipInfo = new ToolTip
                    {
                        AutomaticDelay = 0,
                        AutoPopDelay = 0,
                        InitialDelay = 0,
                        ReshowDelay = 0,
                        ToolTipIcon = ToolTipIcon.Info,
                        UseFading = true,

                        ToolTipTitle = elem.Id
                    };

                    string text = GetElementText(elem);
                    Action action = () => tipInfo?.Show(text, _controlToToolTip);

                    if (_controlToToolTip.InvokeRequired)
                    {
                        _controlToToolTip.Invoke(action);
                    }
                    else
                    {
                        action();
                    }

                    lastToolTipLocation = location;
                }
            };

            timeout.Start();
        }

        public void Hide()
        {
            Destroy.Disposable(ref timeout);
            Destroy.Disposable(ref tipInfo);
        }

        private string GetElementText(Element element)
        {
            StringBuilder builder = new StringBuilder();

            if (element is InputLayer inputLayer)
                AddInputLayerText(inputLayer, builder);
            else if (element is PerceptronLayer perceptronLayer)
                AddPerceptronLayerText(perceptronLayer, builder);
            else if (element is Bias bias)
                AddBiasText(bias, builder);
            else if (element is Input input)
                AddInputText(input, builder);
            else if (element is Perceptron perceptron)
                AddPerceptronText(perceptron, builder);
            else if (element is Edge edge)
                AddEdgeText(edge, builder);

            AddElementText(element, builder);
            return builder.ToString();
        }

        private void AddInputLayerText(InputLayer layer, StringBuilder builder)
        {
            AddLayerText(layer, builder);
        }

        private void AddPerceptronLayerText(PerceptronLayer layer, StringBuilder builder)
        {
            AddLayerText(layer, builder);
            builder.AppendLine("Previous layer: " + layer.Previous.Id);
        }

        private void AddBiasText(Bias bias, StringBuilder builder)
        {
            AddNodeText(bias, builder);
        }

        private void AddInputText(Input input, StringBuilder builder)
        {
            AddNodeText(input, builder);
        }

        private void AddPerceptronText(Perceptron perceptron, StringBuilder builder)
        {
            string actFunc = perceptron.ActivationFunction.ToString();
            string sumValue = perceptron.SumValue?.ToString() ?? "(none)";
            string edgesCount = perceptron.Edges.Count().ToString();

            builder.AppendLine("Activation function: " + actFunc);
            builder.AppendLine("Sum value: " + sumValue);

            AddNodeText(perceptron, builder);

            builder.AppendLine("Edges: " + edgesCount);
        }

        private void AddEdgeText(Edge edge, StringBuilder builder)
        {
            string source = edge.Source.Id;
            string destination = edge.Destination.Id;
            string weight = edge.Weight?.ToString() ?? "(none)";

            builder.AppendLine("Source: " + source);
            builder.AppendLine("Destination: " + destination);
            builder.AppendLine("Weight: " + weight);
        }

        private void AddNodeText(NodeBase node, StringBuilder builder)
        {
            string outputValue = node.OutputValue?.ToString() ?? "(none)";
            string layer = node.Layer.Id;

            builder.AppendLine("Output value: " + outputValue);
            builder.AppendLine("Layer: " + layer);
        }

        private void AddLayerText(LayerBase layer, StringBuilder builder)
        {
            string bias = layer.Bias?.Id ?? "(none)";
            string nodesCount = layer.GetAllNodes().Count().ToString();
            string next = layer.Next?.Id ?? "(this is the output layer)";

            builder.AppendLine("Bias: " + bias);
            builder.AppendLine("Nodes count: " + nodesCount);
            builder.AppendLine("Next layer: " + next);
        }

        private void AddElementText(Element element, StringBuilder builder)
        {
            string strobj = element.Object?.ToString() ?? "(none)";
            builder.AppendLine("Object: " + strobj);
        }

        private bool Validate(Point location)
        {
            return _controlToToolTip.IsHandleCreated && _control.InputLayer != null && ValidateLocation(location);
        }

        private bool ValidateLocation(Point location)
        {
            return Math.Abs(location.X - lastToolTipLocation.X) > 5 || Math.Abs(location.Y - lastToolTipLocation.Y) > 5;
        }
    }
}
