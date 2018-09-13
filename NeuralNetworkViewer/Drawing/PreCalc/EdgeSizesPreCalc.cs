namespace NeuralNetworkVisualizer.Drawing.Cache
{
    internal class EdgeSizesPreCalc
    {
        internal void Initialize(int totalWidth, double widthPortion, int textWidth)
        {
            this.TotalWidth = totalWidth;
            this.WidthPortion = widthPortion;
            this.TextWidth = textWidth;
            IsInitilized = true;
        }

        internal bool IsInitilized { get; private set; } = false;
        internal int TotalWidth { get; private set; }
        internal double WidthPortion { get; private set; }
        internal int TextWidth { get; private set; }
    }
}
