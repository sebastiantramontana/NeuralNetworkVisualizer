using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Preferences
{
    public class EdgePreference : IDisposable
    {
        private TextPreference _text = new TextPreference();
        private Pen _connector = Pens.Black;

        public TextPreference Text { get => _text; set => _text = value; }
        public Pen Connector { get => _connector; set => _connector = value; }
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
