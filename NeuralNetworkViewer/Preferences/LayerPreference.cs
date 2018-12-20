using NeuralNetworkVisualizer.Preferences.Brushes;
using NeuralNetworkVisualizer.Preferences.Pens;
using System;
using Draw = System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class LayerPreference : IDisposable
    {
        private IBrushPreference _background;
        public IBrushPreference Background
        {
            get => _background ?? (_background = new SolidBrushPreference(Draw.Color.Transparent));
            set => _background = value;
        }

        private IPen _border;
        public IPen Border
        {
            get => _border ?? (_border = new SimplePen(Draw.Pens.Transparent));
            set => _border = value;
        }

        private IPen _borderSelected;
        public IPen BorderSelected
        {
            get => _borderSelected ?? (_borderSelected = new SimplePen(Draw.Pens.Transparent));
            set => _borderSelected = value;
        }

        private IBrushPreference _backgroundSelected;
        public IBrushPreference BackgroundSelected
        {
            get => _backgroundSelected ?? (_backgroundSelected = new SolidBrushPreference(Draw.Color.Transparent));
            set => _backgroundSelected = value;
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
            Destroy.Disposable(ref _border);
        }
    }
}
