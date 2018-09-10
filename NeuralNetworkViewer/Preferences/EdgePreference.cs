using NeuralNetworkVisualizer.Preferences.Text;
using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class EdgePreference : IDisposable
    {
        private TextValueFormatter _valueFormatter;
        public TextValueFormatter ValueFormatter
        {
            get => _valueFormatter ?? (_valueFormatter = new TextValueFormatter());
            set => _valueFormatter = value;
        }

        private Pen _connector = Pens.Black;
        public Pen Connector
        {
            get => _connector ?? (_connector = Pens.Transparent);
            set => _connector = value;
        }

        public byte RoundingDigits { get; set; } = 3;

        public void Dispose()
        {
            try
            {
                Destroy.Disposable(ref _connector);
            }
            catch { }
        }
    }
}
