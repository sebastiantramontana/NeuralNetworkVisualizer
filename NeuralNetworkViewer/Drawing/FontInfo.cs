using System.Drawing;

namespace NeuralNetworkVisualizer.Drawing
{
    internal class FontInfo
    {
        internal FontInfo(string family, FontStyle style)
        {
            this.Family = family;
            this.Style = style;
        }

        internal string Family { get; private set; }
        internal FontStyle Style { get; private set; }
    }
}
