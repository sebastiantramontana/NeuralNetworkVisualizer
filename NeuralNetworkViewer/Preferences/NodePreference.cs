using NeuralNetworkVisualizer.Preferences.Brushes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Preferences
{
    public class NodePreference : IDisposable
    {
        private Pen _border = Pens.Black;
        private TextPreference _text = new TextPreference();

        public BrushPreference Background { get; set; } = new SolidBrushPreference(Color.White);
        public TextPreference Text { get => _text; set => _text = value; }
        public Pen Border { get => _border; set => _border = value; }
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
