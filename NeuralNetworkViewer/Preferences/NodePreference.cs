using NeuralNetworkVisualizer.Preferences.Brushes;
using NeuralNetworkVisualizer.Preferences.Formatting;
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

        private Formatter<TextPreference> _outputValueFormatter;
        public Formatter<TextPreference> OutputValueFormatter
        {
            get => _outputValueFormatter ?? (_outputValueFormatter = new Formatter<TextPreference>(() => new TextPreference()));
            set => _outputValueFormatter = value;
        }

        private Pen _border = Pens.Black;
        /// <summary>
        /// The Pen for border: Don't use a System Pen, but clone it!
        /// </summary>
        public Pen Border
        {
            get => _border ?? (_border = new Pen(Color.Transparent));
            set => _border = value;
        }

        public byte RoundingDigits { get; set; } = 3;

        public void Dispose()
        {
            Destroy.Disposable(ref _border);
        }
    }
}
