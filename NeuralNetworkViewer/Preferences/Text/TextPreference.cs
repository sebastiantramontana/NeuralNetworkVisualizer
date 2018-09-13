using NeuralNetworkVisualizer.Drawing;
using NeuralNetworkVisualizer.Preferences.Brushes;
using System;
using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences.Text
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

        private IBrushPreference _brush = new SolidBrushPreference(Color.Black);
        public IBrushPreference Brush
        {
            get => _brush ?? (_brush = new SolidBrushPreference(Color.Transparent));
            set { _brush = value; }
        }

        private StringFormat _format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.Character };
        public StringFormat Format
        {
            get => _format ?? (_format = StringFormat.GenericDefault);
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
