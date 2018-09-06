using NeuralNetworkVisualizer.Preferences.Brushes;
using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class LayerPreference : IDisposable
    {
        private IBrushPreference _background;
        public IBrushPreference Background
        {
            get => _background ?? (_background = new SolidBrushPreference(Color.Transparent));
            set => _background = value;
        }

        private Pen _border = Pens.Black;
        public Pen Border
        {
            get => _border;
            set => _border = value;
        }

        private LayerTitlePreference _title = new LayerTitlePreference() { Background = new GradientBrushPreference(Color.LightSteelBlue, Color.LightSkyBlue, 90), Font = { FontStyle = FontStyle.Bold }, Height = 20 };
        public LayerTitlePreference Title
        {
            get => _title ?? (_title = new LayerTitlePreference());
            set => _title = value;
        }

        public void Dispose()
        {
            Destroy.Disposable(ref _title);

            try
            {
                Destroy.Disposable(ref _border);
            }
            catch { }
        }
    }
}
