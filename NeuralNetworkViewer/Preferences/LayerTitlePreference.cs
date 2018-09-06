using NeuralNetworkVisualizer.Preferences.Brushes;
using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class LayerTitlePreference : IDisposable
    {
        private TextPreference _font;
        public TextPreference Font
        {
            get => _font ?? (_font = new TextPreference() { FontStyle = FontStyle.Bold });
            set => _font = value;
        }

        private IBrushPreference _background;
        public IBrushPreference Background
        {
            get => _background ?? (Background = new GradientBrushPreference(Color.LightSteelBlue, Color.LightSkyBlue, 90));
            set => _background = value;
        }

        public int Height { get; set; } = 20;

        public void Dispose()
        {
            Destroy.Disposable(ref _font);
        }
    }
}
