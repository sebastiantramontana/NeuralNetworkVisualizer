using NeuralNetworkVisualizer.Preferences.Brushes;
using NeuralNetworkVisualizer.Preferences.Pens;
using NeuralNetworkVisualizer.Preferences.Text;
using System;
using Draw = System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class Preference : IDisposable
    {
        private LayerPreference _layers = new LayerPreference
        {
            Background = new SolidBrushPreference(Draw.Color.White),
            Title = new LayerTitlePreference() { Background = new GradientBrushPreference(Draw.Color.LightSteelBlue, Draw.Color.LightSkyBlue, 90), Font = new TextPreference { FontStyle = Draw.FontStyle.Bold }, Height = 20 },
            Border = new SimplePen(Draw.Pens.Black),
            BorderSelected = new SimplePen(Draw.Pens.Orange),
            BackgroundSelected = new SolidBrushPreference(Draw.Color.WhiteSmoke)
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
            get => _inputs ?? (_inputs = new NodePreference
            {
                Background = new SolidBrushPreference(Draw.Color.FromArgb(240, 255, 240)),
                BackgroundSelected = new SolidBrushPreference(Draw.Color.FromArgb(200, 215, 200)),
                Border = new SimplePen(new Draw.Pen(Draw.Color.FromArgb(216, 230, 173), 3f)),
                BorderSelected = new SimplePen(new Draw.Pen(Draw.Color.FromArgb(166, 180, 123), 3f))
            });
            set => _inputs = value;
        }

        public PerceptronPreference Perceptrons
        {
            get => _perceptrons ?? (_perceptrons = new PerceptronPreference
            {
                Background = new SolidBrushPreference(Draw.Color.Azure),
                BackgroundSelected = new SolidBrushPreference(Draw.Color.LightSteelBlue),
                Border = new SimplePen(new Draw.Pen(Draw.Color.LightBlue, 3f)),
                BorderSelected = new SimplePen(new Draw.Pen(Draw.Color.DeepSkyBlue, 3f))
            });
            set => _perceptrons = value;
        }

        public NodePreference Biases
        {
            get => _biases ?? (_biases = new NodePreference
            {
                Background = new SolidBrushPreference(Draw.Color.FromArgb(255, 240, 255)),
                BackgroundSelected = new SolidBrushPreference(Draw.Color.FromArgb(215, 200, 215)),
                Border = new SimplePen(new Draw.Pen(Draw.Color.LightPink, 3f)),
                BorderSelected = new SimplePen(new Draw.Pen(Draw.Color.Pink, 3f))
            });
            set => _biases = value;
        }

        public EdgePreference Edges
        {
            get => _edges ?? (_edges = new EdgePreference());
            set => _edges = value;
        }

        public byte NodeMargins { get; set; } = 5;
        public RenderQuality Quality { get; set; } = RenderQuality.Medium;
        public bool AsyncRedrawOnResize { get; set; } = false;

        public void Dispose()
        {
            Destroy.Disposable(ref _layers);
            Destroy.Disposable(ref _inputs);
            Destroy.Disposable(ref _perceptrons);
            Destroy.Disposable(ref _biases);
        }
    }
}
