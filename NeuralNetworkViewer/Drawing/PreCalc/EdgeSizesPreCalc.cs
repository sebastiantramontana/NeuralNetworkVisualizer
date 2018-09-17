namespace NeuralNetworkVisualizer.Drawing.Cache
{
    internal class EdgeSizesPreCalc
    {
        private int _xPositionFromPrevious = -1;
        private EdgeSizesPreCalcValues previousValues;

        internal EdgeSizesPreCalcValues GetValues(int xPositionFrom, int xPositionTo)
        {
            if (xPositionFrom == _xPositionFromPrevious)
                return previousValues;

            previousValues = PreCalc(xPositionFrom, xPositionTo);
            _xPositionFromPrevious = xPositionFrom;

            return previousValues;
        }

        private EdgeSizesPreCalcValues PreCalc(int xPositionFrom, int xPositionTo)
        {
            var totalWidth = xPositionTo - xPositionFrom;
            var textWidth = (int)(totalWidth / 4.0);

            double xOffsetFar = totalWidth - totalWidth / 3;
            var widthPortionFar = xOffsetFar / totalWidth;
            var farX = xPositionTo - totalWidth / 3;

            double xOffsetNear = totalWidth / 4.0;
            var widthPortionNear = xOffsetNear / totalWidth;
            var nearX = (int)(xPositionFrom + xOffsetNear);

            return new EdgeSizesPreCalcValues(totalWidth, widthPortionNear, widthPortionFar, nearX, farX, textWidth);
        }

        internal class EdgeSizesPreCalcValues
        {
            internal EdgeSizesPreCalcValues(int totalWidth, double widthPortionNear, double widthPortionFar, int nearX, int farX, int textWidth)
            {
                this.TotalWidth = totalWidth;
                this.WidthPortionNear = widthPortionNear;
                this.WidthPortionFar = widthPortionFar;
                this.NearX = nearX;
                this.FarX = farX;
                this.TextWidth = textWidth;
            }

            internal int TotalWidth { get; private set; }
            internal double WidthPortionNear { get; private set; }
            internal double WidthPortionFar { get; private set; }
            internal int NearX { get; private set; }
            internal int FarX { get; private set; }
            internal int TextWidth { get; private set; }
        }
    }
}
