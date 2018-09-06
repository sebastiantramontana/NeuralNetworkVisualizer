using NeuralNetworkVisualizer.Preferences.Brushes;
using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class NodePreference : IDisposable
    {
        private IBrushPreference _background;
        public IBrushPreference Background
        {
            get => _background ?? (_background = new SolidBrushPreference(Color.White));
            set => _background = value;
        }

        private TextPreference _text;
        public TextPreference Text
        {
            get => _text ?? (_text = new TextPreference());
            set => _text = value;
        }

        private Pen _border;
        public Pen Border
        {
            get => _border ?? (_border = Pens.Black);
            set => _border = value;
        }

        public byte RoundingDigits { get; set; } = 3;

        public void Dispose()
        {
            Destroy.Disposable(ref _text);

            try
            {
                Destroy.Disposable(ref _border);
            }
            catch { }
        }
    }
}
