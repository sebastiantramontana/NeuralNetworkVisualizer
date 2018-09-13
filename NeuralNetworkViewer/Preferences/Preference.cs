using NeuralNetworkVisualizer.Preferences.Brushes;
using NeuralNetworkVisualizer.Preferences.Text;
using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class Preference : IDisposable
    {
        private LayerPreference _layers = new LayerPreference
        {
            Background = new SolidBrushPreference(Color.White),
            Title = new LayerTitlePreference() { Background = new GradientBrushPreference(Color.LightSteelBlue, Color.LightSkyBlue, 90), Font = new TextPreference { FontStyle = FontStyle.Bold }, Height = 20 },
            Border = new Pen(Color.Black)
        };

        private NodePreference _inputs;
        private PerceptronPreference _perceptrons;
        private NodePreference _biases;
        private EdgePreference _edges;

        public LayerPreference Layers
        {
            get => _layers ?? (_layers = new LayerPreference());
            set => _layers = value;
        }

        public NodePreference Inputs
        {
            get => _inputs ?? (_inputs = new NodePreference { Background = new SolidBrushPreference(Color.FromArgb(240, 255, 240)), Border = new Pen(Color.FromArgb(216, 230, 173), 3f) });
            set => _inputs = value;
        }

        public PerceptronPreference Perceptrons
        {
            get => _perceptrons ?? (_perceptrons = new PerceptronPreference { Background = new SolidBrushPreference(Color.Azure), Border = new Pen(Color.LightBlue, 3f) });
            set => _perceptrons = value;
        }

        public NodePreference Biases
        {
            get => _biases ?? (_biases = new NodePreference { Background = new SolidBrushPreference(Color.FromArgb(255, 240, 255)), Border = new Pen(Color.LightPink, 3f) });
            set => _biases = value;
        }

        public EdgePreference Edges
        {
            get => _edges ?? (_edges = new EdgePreference());
            set => _edges = value;
        }

        public byte NodeMargins { get; set; } = 5;
        public RenderQuality Quality { get; set; } = RenderQuality.Medium;

        public void Dispose()
        {
            Destroy.Disposable(ref _layers);
            Destroy.Disposable(ref _inputs);
            Destroy.Disposable(ref _perceptrons);
            Destroy.Disposable(ref _biases);
        }
    }
}
