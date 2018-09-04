using NeuralNetworkVisualizer.Preferences.Brushes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Preferences
{
    public class LayerPreference : IDisposable
    {
        private TextPreference _fontTitle = new TextPreference() { FontStyle = FontStyle.Bold };
        private Pen _border = Pens.Black;

        public BrushPreference Background { get; set; } = new SolidBrushPreference(Color.White);
        public TextPreference FontTitle { get => _fontTitle; set => _fontTitle = value; }
        public BrushPreference BackgroundTitle { get; set; } = new GradientBrushPreference(Color.LightSteelBlue, Color.LightSkyBlue, 90);
        public int HeightTitle { get; set; } = 20;
        public Pen Border { get => _border; set => _border = value; }

        public void Dispose()
        {
            Destroy.Disposable(ref _fontTitle);

            try
            {
                Destroy.Disposable(ref _border);
            }
            catch { }
        }
    }
}
