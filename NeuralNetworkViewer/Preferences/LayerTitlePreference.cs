using NeuralNetworkVisualizer.Preferences.Brushes;
using NeuralNetworkVisualizer.Preferences.Text;
using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class LayerTitlePreference : IDisposable
    {
        private TextPreference _font;
        public TextPreference Font
        {
            get => _font ?? (_font = new TextPreference());
            set => _font = value;
        }

        private IBrushPreference _background;
        public IBrushPreference Background
        {
            get => _background ?? (_background = new SolidBrushPreference(Color.Transparent));
            set => _background = value;
        }

        public int Height { get; set; }

        public void Dispose()
        {
            Destroy.Disposable(ref _font);
        }
    }
}
