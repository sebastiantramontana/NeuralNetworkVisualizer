using NeuralNetworkVisualizer.Preferences.Brushes;
using NeuralNetworkVisualizer.Preferences.Formatting;
using NeuralNetworkVisualizer.Preferences.Pens;
using NeuralNetworkVisualizer.Preferences.Text;
using System;
using Draw = System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class NodePreference : IDisposable
    {
        private IBrushPreference _background;
        public IBrushPreference Background
        {
            get => _background ?? (_background = new SolidBrushPreference(Draw.Color.Transparent));
            set => _background = value;
        }

        private IBrushPreference _backgroundSelected;
        public IBrushPreference BackgroundSelected
        {
            get => _backgroundSelected ?? (_backgroundSelected = new SolidBrushPreference(Draw.Color.Transparent));
            set => _backgroundSelected = value;
        }

        private IFormatter<TextPreference> _outputValueFormatter;
        public IFormatter<TextPreference> OutputValueFormatter
        {
            get => _outputValueFormatter ?? (_outputValueFormatter = new BasicFormatter<TextPreference>(() => new TextPreference()));
            set => _outputValueFormatter = value;
        }

        private IPen _border = new SimplePen(Draw.Pens.Black);
        public IPen Border
        {
            get => _border ?? (_border = new SimplePen(Draw.Pens.Transparent));
            set => _border = value;
        }

        private IPen _borderSelected = new SimplePen(Draw.Pens.Orange);
        public IPen BorderSelected
        {
            get => _borderSelected ?? (_borderSelected = new SimplePen(Draw.Pens.Transparent));
            set => _borderSelected = value;
        }

        public byte RoundingDigits { get; set; } = 3;

        public void Dispose()
        {
            Destroy.Disposable(ref _border);
        }
    }
}
