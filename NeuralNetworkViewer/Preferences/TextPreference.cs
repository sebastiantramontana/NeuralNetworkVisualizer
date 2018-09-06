using NeuralNetworkVisualizer.Drawing;
using NeuralNetworkVisualizer.Preferences.Brushes;
using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class TextPreference : IDisposable
    {
        private string _fontFamily;
        public string FontFamily
        {
            get => _fontFamily ?? (_fontFamily = "Tahoma");
            set => _fontFamily = value;
        }

        public FontStyle FontStyle { get; set; } = FontStyle.Regular;

        private IBrushPreference _brush;
        public IBrushPreference Brush
        {
            get => _brush ?? (_brush = new SolidBrushPreference(Color.Black));
            set { _brush = value; }
        }

        private StringFormat _format;
        public StringFormat Format
        {
            get => _format ?? (_format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.Character });
            set => _format = value;
        }

        internal FontInfo CreateFontInfo()
        {
            return new FontInfo(this.FontFamily, this.FontStyle);
        }

        public void Dispose()
        {
            Destroy.Disposable(ref _format);
        }
    }
}
