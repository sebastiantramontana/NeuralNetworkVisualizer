using NeuralNetworkVisualizer.Preferences.Brushes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Preferences
{
    public class Preference : IDisposable
    {
        private LayerPreference _layers = new LayerPreference();
        private NodePreference _inputs = new NodePreference { Background = new SolidBrushPreference(Color.FromArgb(240, 255, 240)), Border = new Pen(Color.FromArgb(216, 230, 173), 3f) };
        private NodePreference _perceptrons = new NodePreference { Background = new SolidBrushPreference(Color.Azure), Border = new Pen(Color.LightBlue, 3f) };
        private NodePreference _biases = new NodePreference { Background = new SolidBrushPreference(Color.FromArgb(255, 240, 255)), Border = new Pen(Color.LightPink, 3f) };
        private EdgePreference _edges = new EdgePreference();

        public LayerPreference Layers { get => _layers; set => _layers = value; }
        public NodePreference Inputs { get => _inputs; set => _inputs = value; }
        public NodePreference Perceptrons { get => _perceptrons; set => _perceptrons = value; }
        public NodePreference Biases { get => _biases; set => _biases = value; }
        public EdgePreference Edges { get => _edges; set => _edges = value; }
        public byte Margins { get; set; } = 5;
        public RenderQuality Quality { get; set; } = RenderQuality.Medium;

        public void Dispose()
        {
            Destroy.Disposable(ref _layers);
            Destroy.Disposable(ref _inputs);
            Destroy.Disposable(ref _perceptrons);
            Destroy.Disposable(ref _biases);
            Destroy.Disposable(ref _edges);
        }
    }
}
