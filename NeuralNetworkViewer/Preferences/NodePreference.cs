using NeuralNetworkVisualizer.Preferences.Brushes;
using NeuralNetworkVisualizer.Preferences.Text;
using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class NodePreference : IDisposable
    {
        private IBrushPreference _background;
        public IBrushPreference Background
        {
            get => _background ?? (_background = new SolidBrushPreference(Color.Transparent));
            set => _background = value;
        }

        private TextValueFormatter _outputValueFormatter;
        public TextValueFormatter OutputValueFormatter
        {
            get => _outputValueFormatter ?? (_outputValueFormatter = new TextValueFormatter());
            set => _outputValueFormatter = value;
        }

        private Pen _border = Pens.Black;
        public Pen Border
        {
            get => _border ?? (_border = Pens.Transparent);
            set => _border = value;
        }

        public byte RoundingDigits { get; set; } = 3;

        public void Dispose()
        {
            try
            {
                Destroy.Disposable(ref _border);
            }
            catch { }
        }
    }
}
