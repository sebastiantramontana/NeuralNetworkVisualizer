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
            get => _background ?? (_background = new SolidBrushPreference(Color.White));
            set => _background = value;
        }

        private Pen _border;
        public Pen Border
        {
            get => _border ?? (_border = Pens.Black);
            set => _border = value;
        }

        private LayerTitlePreference _title;
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
