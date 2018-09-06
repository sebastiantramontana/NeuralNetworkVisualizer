using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class EdgePreference : IDisposable
    {
        private TextPreference _text;
        public TextPreference Text
        {
            get => _text ?? (_text = new TextPreference());
            set => _text = value;
        }

        private Pen _connector;
        public Pen Connector
        {
            get => _connector ?? (_connector = Pens.Black);
            set => _connector = value;
        }

        public byte RoundingDigits { get; set; } = 3;

        public void Dispose()
        {
            Destroy.Disposable(ref _text);

            try
            {
                Destroy.Disposable(ref _connector);
            }
            catch { }
        }
    }
}
